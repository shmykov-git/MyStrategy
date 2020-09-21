using MyStrategy.Extensions;
using Suit;
using Suit.Logs;

namespace MyStrategy.DataModel.Acts
{
    public class Attack : IPairAct
    {
        private readonly ILog log = IoC.Get<ILog>();

        public void Do(Unit main, Unit enemy)
        {
            if (!main.IsAtAttackDistance(enemy))
                return;

            if (main.Round.AttackCount >= main.AttackCount)
                return;

            DoAttack(main, enemy);

            main.Round.AttackCount++;
        }

        public int Key { get; set; }

        private void DoAttack(Unit main, Unit enemy)
        {
            if (enemy.Hp <= main.Damage)
            {
                enemy.Hp = 0;
                enemy.Kill();
                main.RemovePairAct<Attack>();
                main.RemoveSelfAct<MoveToFight>();
                log.Debug($"{main.Id} kill {enemy.Id}");
            }
            else
            {
                enemy.Hp -= main.Damage.ToInt();
                log.Debug($"{main.Id} damage {enemy.Id} Hp: {enemy.Hp}");
            }
        }
    }
}