using System;
using Suit.Logs;

namespace MyStrategy.Logs
{
    public class NoLog : ILog
    {
        public void Warn(string msg)
        { }

        public void Exception(Exception e)
        { }

        public void Info(string msg)
        { }

        public void Debug(string msg)
        { }

        public void Trace(string msg)
        { }

        public void Error(string msg)
        { }

        public void Fatal(string msg)
        { }
    }
}