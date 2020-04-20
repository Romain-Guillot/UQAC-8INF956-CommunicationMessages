using System;
using RabbitMQ.Client;
using System.Text;

namespace LogEmitter
{
    public class RabbitLogger : ILogger
    {
        private IConnection _connection;
        private IModel _channel;
        
        public void Init()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "logs", type: "direct");
            Console.WriteLine("RabbitMQ logger initialized");
        }

        public void Log(string message, LogSeverity severity)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "logs",
                                  routingKey: severity.Value,
                                  basicProperties: null,
                                  body: body);
        }

        public void Close()
        {
            _channel.Close();
            _connection.Close();
            Console.WriteLine("RabbitMQ logger closed");
        }
    }
}