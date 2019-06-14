namespace ImpoDoc.Common.Logger
{
    public interface ILogWriter
    {
        string LogType { get; }
        void Write(object message);
    }
}