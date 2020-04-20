using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LogEmitter;
using LogReceiver;

namespace LogAnalysis
{
    class Program
    {
        private static readonly Dictionary<LogSeverity, int> LogAnalysis = new Dictionary<LogSeverity, int> {
            { LogSeverity.Info, 0 },
            { LogSeverity.Warning, 0 },
            { LogSeverity.Error, 0 },
            { LogSeverity.Critical, 0 },
        };
        private const int RefreshRate = 1000;  // 1sec

        static void Main(string[] args)
        {
            ILogReceiver receiver = new RabbitMQReceiver();
            receiver.Init();
            foreach (var severity in LogSeverity.Values())
            {
                receiver.BindRoute(severity);
            }
            receiver.Listen((severity, message) => {
                LogAnalysis[severity]++;
            });
            new Task(WriteLogStats).Start();
            
            Console.ReadKey();
            receiver.Close();
        }

        static void WriteLogStats()
        {
            while (true)
            {
                Console.Clear();
                foreach (var (severity, counter) in LogAnalysis)
                {
                    Console.WriteLine($"{severity.Value} : {counter} logs");
                }
                Console.WriteLine("Press any key to exit.");
                Thread.Sleep(RefreshRate);
            }
        }
    }
}