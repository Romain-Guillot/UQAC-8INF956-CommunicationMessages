namespace LogEmitter
{

    public enum LogSeverity
    {
        Info,
        Warning,
        Error,
        Critical
    }
    
    public interface ILogger
    {
        public void Init();

        public void Log(string message, LogSeverity severity = LogSeverity.Info);

        public void Close();
    }
}