using MyStrategy.DataModel;
using MyStrategy.Extensions;
using Suit;
using Suit.Logs;

namespace MyStrategy.Acts
{
    public class AttackEnemy : IEnemyAct
    {
        private readonly ILog log = IoC.Get<ILog>();

        public void Clean()
        { }

        public Unit Unit { get; }
        public Unit Enemy { get; }

        public AttackEnemy(Unit unit, Unit enemy)
        {
            Unit = unit;
            Enemy = enemy;
        }

        public void Do()
        {
            if (!Unit.IsAtAttackDistance(Enemy))
                return;

            if (Unit.Round.AttackCount >= Unit.AttackCount)
                return;

            Attack();

            Unit.Round.AttackCount++;
        }

        private void Attack()
        {
            if (Enemy.Hp <= Unit.Damage)
            {
                Enemy.Hp = 0;
                Enemy.Kill();
                log.Debug($"{Unit.Id} kill {Enemy.Id}");
            }
            else
            {
                Enemy.Hp -= Unit.Damage.ToInt();
                log.Debug($"{Unit.Id} damage {Enemy.Id} Hp: {Enemy.Hp}");
            }
        }
    }
}