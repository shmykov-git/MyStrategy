using System.Linq;
using MyStrategy.DataModel;
using MyStrategy.Extensions;
using MyStrategy.Tools;
using Suit;
using Suit.Logs;

namespace MyStrategy.Acts
{
    public class Kill : IAct
    {
        private readonly ILog log = IoC.Get<ILog>();
        private readonly IViewer viewer = IoC.Get<IViewer>();

        public void Clean()
        { }

        public Unit Unit { get; }

        public Kill(Unit unit)
        {
            Unit = unit;
        }

        public void Do()
        {
            Unit.Acts.ToList().ForEach(act => Unit.RemoveAct(act));
            Unit.Scene.Units.SelectMany(unit => unit.Acts.Select(act => act as IEnemyAct).Where(act => act != null))
                .Where(act => act.Enemy == Unit).ToList().ForEach(act => act.Unit.RemoveAct(act));
            Unit.Clan.Units.Remove(Unit);

            log.Debug($"{Unit.Id} is dead");
            viewer.OnKill(Unit);

            Unit.IsActive = false;
        }
    }
}