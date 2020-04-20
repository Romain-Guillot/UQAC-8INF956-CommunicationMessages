using System;
using System.Linq;
using System.Text;
using LogEmitter;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LogReceiver
{
    public class RabbitMQReceiver : ILogReceiver
    {
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        public void Init()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "logs", type: "direct");
            _queueName = _channel.QueueDeclare().QueueName;
        }

        public void BindRoute(LogSeverity severity)
        {
            _channel.QueueBind(queue: _queueName, exchange: "logs", routingKey: severity.Value);
        }

        public void Listen(Action<LogSeverity, string> onReceived)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                var severity = LogSeverity.Values().First(s => s.Value == ea.RoutingKey);
                onReceived(severity, message);
            };
            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        }

        public void Close()
        {
            _channel.Close();
            _connection.Close();
            Console.WriteLine("Connection closed.");
        }
    }
}