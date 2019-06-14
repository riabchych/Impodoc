using System;

namespace ImpoDoc.Common.Logger
{
    public class LoggerFactory
    {
        protected LoggerFactory()
        {
        }

        public static T Create<T>() where T : ILogger, new()
        {
            return Activator.CreateInstance<T>();
        }
    }
}