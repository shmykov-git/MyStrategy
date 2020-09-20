using MyStrategy.Tools;
using Suit;
using Suit.Logs;

namespace MyStrategy.DataModel.Acts
{
    public class Kill : ISelfAct
    {
        private readonly ILog log = IoC.Get<ILog>();
        private readonly IViewer viewer = IoC.Get<IViewer>();

        public void Do(Unit unit)
        {
            unit.Clan.Units.Remove(unit);
            log.Debug($"{unit.Id} is dead");
            viewer.OnKill(unit);
        }

        public int Key { get; set; }
    }
}