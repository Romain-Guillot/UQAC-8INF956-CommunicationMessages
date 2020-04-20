using System;
using System.IO;
using LogEmitter;
using LogReceiver;

namespace FileDump
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var w = File.AppendText("application.log"))
            {
                ILogReceiver receiver = new RabbitMQReceiver();
                receiver.Init();
                var severities = new[] {LogSeverity.Warning, LogSeverity.Error, LogSeverity.Critical};
                foreach (var severity in severities)
                {
                    receiver.BindRoute(severity);
                }
                receiver.Listen((severity, message) => {
                    w.WriteLine($"[{DateTime.Now:dd-MM-yyyy H:mm:ss}][{severity.Value.ToUpper()}]: {message}");
                });
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                receiver.Close();
            }
        }
    }
}