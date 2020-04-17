using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LogAnalysis
{
    class Program
    {
        static Dictionary<string, int> logAnalysis = new Dictionary<string, int>() {
            { "Info", 0 },
            { "Warning", 0 },
            { "Error", 0 },
            { "Critical", 0 },
        };
        static void Main(string[] args)
        {

            Action<object> action = (object obj) =>
            {
                while (true)
                {
                    Thread.Sleep(10000);
                    Console.Clear();

                    foreach (var item in logAnalysis)
                    {
                        Console.WriteLine($"{item.Key} : {item.Value} logs");
                    }

                }
            };
            Task t1 = new Task(action, "alpha");
            t1.Start();

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);


                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: queueName, exchange: "logs", routingKey: "");
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    //Console.WriteLine($"[{DateTime.Now:dd-MM-yyyy H:mm:ss}][{ea.RoutingKey.ToUpper()}]: {message}");


                    if (ea.RoutingKey.Contains("info"))
                    {
                        logAnalysis["Info"]++;
                    }
                    else if (ea.RoutingKey.Contains("warning"))
                    {
                        logAnalysis["Warning"]++;

                    }
                    else if (ea.RoutingKey.Contains("critical"))
                    {
                        logAnalysis["Critical"]++;

                    }
                    else if (ea.RoutingKey.Contains("error"))
                    {
                        logAnalysis["Error"]++;

                    }



                };
                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);






                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }


        }
    }
}