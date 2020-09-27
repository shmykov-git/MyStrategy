using System;
using Suit.Logs;

namespace MyStrategy.Logs
{
    public class TraceLog : ILog
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
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }

        public void Error(string msg)
        { }

        public void Fatal(string msg)
        { }
    }
}