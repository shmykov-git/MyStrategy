using System;
using Suit.Logs;

namespace MyStrategy.Test
{
    class LogToConsoleAndDebug : ILog
    {
        public void Warn(string msg)
        {
            Console.WriteLine(msg);
            System.Diagnostics.Debug.WriteLine(msg);
        }

        public void Exception(Exception e)
        {
            Console.WriteLine(e.ToString());
            System.Diagnostics.Debug.WriteLine(e.ToString());
        }

        public void Info(string msg)
        {
            Console.WriteLine(msg);
            System.Diagnostics.Debug.WriteLine(msg);
        }

        public void Debug(string msg)
        {
            Console.WriteLine(msg);
            System.Diagnostics.Debug.WriteLine(msg);
        }

        public void Trace(string msg)
        {
            Console.WriteLine(msg);
            System.Diagnostics.Debug.WriteLine(msg);
        }

        public void Error(string msg)
        {
            Console.WriteLine(msg);
            System.Diagnostics.Debug.WriteLine(msg);
        }

        public void Fatal(string msg)
        {
            Console.WriteLine(msg);
            System.Diagnostics.Debug.WriteLine(msg);
        }
    }
}
