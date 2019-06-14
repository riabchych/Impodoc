using System.Diagnostics;

namespace ImpoDoc.Common.Logger
{
    public class ErrorTraceWriter : ILogWriter
    {
        public string LogType { get; } = "ERROR";

        public void Write(object message)
        {
            Trace.WriteLine(message, LogType);
        }
    }
}