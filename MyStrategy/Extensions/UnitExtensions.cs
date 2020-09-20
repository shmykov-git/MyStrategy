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
        public static IEnumerable<Unit> GetActUnits(this Unit main)
        {
            return main.Scene.Units.Where(unit => unit.Clan != main.Clan);
        }

        public static IEnumerable<Unit> GetUnderSightOpponents(this Unit main)
        {
            var sightDistance2 = main.SightDistance * main.SightDistance;

            foreach (var unit in main.GetActUnits())
            {
                if ((unit.Position - main.Position).Length2 <= sightDistance2)
                    yield return unit;
            }
        }

        public static TSelfAct FindSelfAct<TSelfAct>(this Unit unit) where TSelfAct : class, ISelfAct
        {
            return unit.SelfActs.OfType<TSelfAct>().FirstOrDefault(a => a.Key == unit.Id);
        }

        public static void RemoveSelfAct<TSelfAct>(this Unit unit, TSelfAct selfAct) where TSelfAct : class, ISelfAct
        {
            unit.SelfActs.Remove(selfAct);
        }

        public static void AddSelfAct<TSelfAct>(this Unit unit, TSelfAct selfAct) where TSelfAct : class, ISelfAct
        {
            unit.SelfActs.Add(selfAct);
        }

        public static void Kill(this Unit unit)
        {
            var kill = unit.FindSelfAct<Kill>();
            if (kill == null)
                unit.AddSelfAct(new Kill() {Key = unit.Id});
        }

        public static bool IsAtDistance(this Unit main, Vector position, float distance)
        {
            return (position - main.Position).Length2 <= distance * distance;
        }

        public static bool IsAtSightDistance(this Unit main, Unit slave)
        {
            return main.IsAtDistance(slave.Position, main.SightDistance);
        }

        public static bool IsAtAttackDistance(this Unit main, Unit slave)
        {
            return main.IsAtDistance(slave.Position, main.DamageDistance);
        }
    }
}
