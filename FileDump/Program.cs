using System;
using System.IO;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FileDump
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            using (var w = File.AppendText("application.log"))
            {
                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: queueName,
                    exchange: "logs",
                    routingKey: "warning");
                channel.QueueBind(queue: queueName,
                    exchange: "logs",
                    routingKey: "error");
                channel.QueueBind(queue: queueName,
                    exchange: "logs",
                    routingKey: "critical");

                Console.WriteLine(" [*] Waiting for logs.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                       w.WriteLine($"[{DateTime.Now:dd-MM-yyyy H:mm:ss}][{ea.RoutingKey.ToUpper()}]: {message}");

                    
                };
                channel.BasicConsume(queue: queueName,
                    autoAck: true,
                    consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}