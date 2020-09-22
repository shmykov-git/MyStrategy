using System.Linq;
using MyStrategy.Extensions;
using Suit;
using Suit.Logs;

namespace MyStrategy.DataModel.Acts
{
    public class FindAndMoveToAttack : IAct
    {
        private readonly ILog log = IoC.Get<ILog>();

        public void Clean()
        {
            Unit.RemoveAct<MoveToAttack>();
        }

        public Unit Unit { get; }

        public FindAndMoveToAttack(Unit unit)
        {
            Unit = unit;
        }

        public void Do()
        {
            var moveToAttack = Unit.GetAct<MoveToAttack>();

            if (moveToAttack != null)
            {
                if (!Unit.IsAtSightDistance(moveToAttack.Enemy))
                {
                    Unit.RemoveAct(moveToAttack);
                }
            }
            
            if (moveToAttack == null)
            {
                var enemy = Unit.GetUnderSightEnemies().FirstOrDefault();
                if (enemy == null)
                    return;

                Unit.AddAct(new MoveToAttack(Unit, enemy));

                log.Debug($"{Unit.Id} found enemy {enemy.Id}");
            }
        }
    }
}