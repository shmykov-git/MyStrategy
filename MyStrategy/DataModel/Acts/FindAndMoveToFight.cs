using System.Linq;
using MyStrategy.Extensions;
using Suit;
using Suit.Logs;

namespace MyStrategy.DataModel.Acts
{
    public class FindAndMoveToFight : IPairAct
    {
        private readonly ILog log = IoC.Get<ILog>();

        public void Do(Unit main, Unit enemy)
        {
            var myAttack = main.FindSelfAct<MoveToFight>();

            if (!main.IsAtSightDistance(enemy))
                return;

            if (myAttack != null)
                return;

            main.AddSelfAct(new MoveToFight(enemy.PositionFn) {Key = main.Id});

            log.Debug($"{main.Id} saw {enemy.Id}");
        }

        public int Key { get; set; }
    }
}