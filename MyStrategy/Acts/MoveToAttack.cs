using MyStrategy.DataModel;
using MyStrategy.Extensions;
using Suit;
using Suit.Logs;

namespace MyStrategy.Acts
{
    public class MoveToAttack: IEnemyAct
    {
        private readonly ILog log = IoC.Get<ILog>();

        public void Clean()
        {
            Unit.RemoveAct<AttackEnemy>();
        }

        public Unit Unit { get; set; }
        public Unit Enemy { get; }

        public MoveToAttack(Unit unit, Unit enemy)
        {
            this.Unit = unit;
            this.Enemy = enemy;
        }

        private Vector correctMove;
        private int correctCount;

        public void Do()
        {
            var attackEnemy = Unit.GetAct<AttackEnemy>();

            if (Unit.IsAtAttackDistance(Enemy))
            {
                if (attackEnemy == null)
                    Unit.AddAct(new AttackEnemy(Unit, Enemy));
            }
            else
            {
                if (attackEnemy != null)
                    Unit.RemoveAct(attackEnemy);

                var move = (Enemy.Position - Unit.Position).ToLength(Unit.Speed);
                Unit.Position += move + Unit.GetMoveIntersectCorrection(move);

                log.Debug($"{Unit.Id} move to Enemy {Enemy.Id}");
            }
        }
    }
}