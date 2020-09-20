using MyStrategy.Extensions;
using Suit;
using Suit.Logs;

namespace MyStrategy.DataModel.Acts
{
    public class MoveToFight: ISelfAct
    {
        private readonly ILog log = IoC.Get<ILog>();

        private readonly VectorFn aimFn;

        public MoveToFight(VectorFn aimFn)
        {
            this.aimFn = aimFn;
        }

        public void Do(Unit unit)
        {
            var aimPosition = aimFn();

            if (!unit.IsAtDistance(aimPosition, unit.DamageDistance))
            {
                unit.Position += (aimPosition - unit.Position).ToLength(unit.Speed);

                log.Debug($"{unit.Id} move to fight to {aimPosition}");
            }
        }

        public int Key { get; set; }
    }
}