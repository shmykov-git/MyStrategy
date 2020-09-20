using Suit;
using Suit.Logs;

namespace MyStrategy.DataModel.Acts
{
    public class Kill : ISelfAct
    {
        private readonly ILog log = IoC.Get<ILog>();

        public void Do(Unit unit)
        {
            unit.Clan.Units.Remove(unit);
            log.Debug($"{unit.Id} is dead");
        }

        public int Key { get; set; }
    }
}