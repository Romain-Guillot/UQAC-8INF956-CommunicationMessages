using System;

namespace LogEmitter
{
    // Enum-like
    public class LogSeverity
    {
        private LogSeverity(string value) { Value = value; }

        public string Value { get; set; }

        public static LogSeverity Info => new LogSeverity("info");
        public static LogSeverity Warning => new LogSeverity("warning");
        public static LogSeverity Error => new LogSeverity("error");
        public static LogSeverity Critical => new LogSeverity("critical");

        public static LogSeverity[] Values() => new[] {Info, Warning, Error, Critical};
}
    
    public interface ILogger
    {
        public void Init();

        public void Log(string message, LogSeverity severity);

        public void Close();
    }
}