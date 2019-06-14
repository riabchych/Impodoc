using System.Diagnostics;

namespace ImpoDoc.Common.Logger
{
    public class InfoTraceWriter : ILogWriter
    {
        public string LogType { get; } = "INFO";

        public void Write(object message)
        {
            Trace.WriteLine(message, LogType);
        }
    }
}