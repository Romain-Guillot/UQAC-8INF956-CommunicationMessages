using System;
using LogEmitter;

namespace LogReceiver
{
    public interface ILogReceiver
    {
        void Init();

        void BindRoute(LogSeverity severity);

        void Listen(Action<LogSeverity, string> onReceived);
        
        void Close();
    }
}