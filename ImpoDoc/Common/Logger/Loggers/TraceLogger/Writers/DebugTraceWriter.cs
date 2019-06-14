using System.Diagnostics;

namespace ImpoDoc.Common.Logger
{
    public class DebugTraceWriter : ILogWriter
    {
        public string LogType { get; } = "DEBUG";

        public void Write(object message)
        {
            Trace.WriteLine(message, LogType);
        }
    }
}