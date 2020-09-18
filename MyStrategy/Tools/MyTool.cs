using System;
using System.Threading.Tasks;
using Suit.Aspects;
using Suit.Logs;

namespace MyStrategy.Tools
{
    public class MyTool
    {
        [LoggingAspect(LoggingRule.Full | LoggingRule.Performance | LoggingRule.Stabilize | LoggingRule.NoCut)]
        public void Start(string message)
        {
            Task.Delay(TimeSpan.FromSeconds(1)).Wait();
        }

        #region IoC

        private readonly ILog log;
        private readonly IMyToolSettings settings;

        public MyTool(ILog log, IMyToolSettings settings)
        {
            this.log = log;
            this.settings = settings;
        }

        #endregion
    }
}
