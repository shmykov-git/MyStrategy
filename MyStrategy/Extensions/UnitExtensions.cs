using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyStrategy.DataModel;
using MyStrategy.DataModel.Acts;

namespace MyStrategy.Extensions
{
    public static class UnitExtensions
    {
        // TODO: do not iterate all units
        public static IEnumerable<Unit> GetEnemies(this Unit unit)
        {
            return unit.Scene.Units.Where(enemy => enemy.Clan != unit.Clan);
        }

        public static IEnumerable<Unit> GetUnderSightEnemies(this Unit unit)
        {
            var sightDistance2 = unit.SightDistance * unit.SightDistance;

            foreach (var enemy in unit.GetEnemies())
            {
                if ((unit.Position - enemy.Position).Length2 <= sightDistance2)
                    yield return enemy;
            }
        }

        public static TSelfAct GetAct<TSelfAct>(this Unit unit) where TSelfAct : class, IAct
        {
            return unit.Acts.OfType<TSelfAct>().FirstOrDefault();
        }

        public static void RemoveAct<TAct>(this Unit unit, TAct act = null) where TAct : class, IAct
        {
            var actToRemove = act ?? unit.GetAct<TAct>();
            if (actToRemove != null)
            {
                unit.Acts.Remove(actToRemove);
                actToRemove.Clean();
            }
        }

        public static void AddAct<TAct>(this Unit unit, TAct act) where TAct : class, IAct
        {
            unit.Acts.Add(act);
        }

        public static void Kill(this Unit unit)
        {
            var kill = unit.GetAct<Kill>();
            if (kill == null)
                unit.AddAct(new Kill(unit));
        }

        public static bool IsAtDistance(this Unit unit, Vector position, float distance)
        {
            return (position - unit.Position).Length2 <= distance * distance;
        }

        public static bool IsAtSightDistance(this Unit unit, Unit enemy)
        {
            return unit.IsAtDistance(enemy.Position, unit.SightDistance);
        }

        public static bool IsAtAttackDistance(this Unit unit, Unit enemy)
        {
            return unit.IsAtDistance(enemy.Position, unit.DamageDistance);
        }
    }
}
