using System;

namespace LogEmitter
{
    // Enum-like
    public class LogSeverity
    {
        private LogSeverity(string value) { Value = value; }

        public string Value { get; }

        public static LogSeverity Info => new LogSeverity("info");
        public static LogSeverity Warning => new LogSeverity("warning");
        public static LogSeverity Error => new LogSeverity("error");
        public static LogSeverity Critical => new LogSeverity("critical");

        public static LogSeverity[] Values() => new[] {Info, Warning, Error, Critical};

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj) => obj != null && GetType() == obj.GetType() && ((LogSeverity) obj).Value.Equals(Value);
    }
    
    public interface ILogger
    {
        public void Init();

        public void Log(string message, LogSeverity severity);

        public void Close();
    }
}