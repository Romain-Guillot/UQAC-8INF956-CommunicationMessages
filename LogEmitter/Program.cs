using System;
using System.Collections.Generic;
using System.Threading;

namespace LogEmitter
{
    class Program
    {
        private static Random _random = new Random();
        private static List<string> _randomSentence = new List<string>
        {
            "Would you like a drink of my smoothie?", "Hold on I need at least a few minutes!", "He landed a big trout.",
            "I must get myself a new pair of glasses?", "It was a big one.", "What a big truck!",
            "Tom has some pretty big shoes to fill.", "Why is it such a big deal?", "Why are your ears so big?",
            "Very good job, I am so proud of you!", "We already know the truth.", "Do you know women who don’t want to dance?",
            "What do you need it for?", "Shape up, or ship out!", "Do you understand?", "Would you like to have some coffee?",
            "My legs feel like noodles!", "I’m not opening the door?", "Your biggest enemy is yourself.", "I can't wait to eat dinner!"
        };
        private static  Array _severities = Enum.GetValues(typeof(LogSeverity));
        
        private static void Main(string[] args)
        {
            ILogger logger = new RabbitLogger();
            logger.Init();

            Console.CancelKeyPress += (_, __) => logger.Close(); // just to catch CTRL-C to close our logger

            while (true)
            {
                var severity = RandomLogSeverity();
                var message = RandomMessage();
                logger.Log(message, severity = severity);
                Thread.Sleep(2000);
            }
        }

        private static LogSeverity RandomLogSeverity()
        {
            return (LogSeverity) _severities.GetValue(_random.Next(_severities.Length));
        }

        private static string RandomMessage()
        {
            return _randomSentence[_random.Next(_randomSentence.Count)];
        }
    }

}