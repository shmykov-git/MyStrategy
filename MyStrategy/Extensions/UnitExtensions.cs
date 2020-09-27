using System.Collections.Generic;
using System.Linq;
using MyStrategy.Acts;
using MyStrategy.DataModel;

namespace MyStrategy.Extensions
{
    public static class UnitExtensions
    {
        public static IEnumerable<Unit> GetActives(this Unit unit)
        {
            return unit.Scene.Net.GetActives(unit);
        }

        public static IEnumerable<Unit> GetEnemies(this Unit unit)
        {
            return unit.GetActives().Where(enemy => enemy.Clan != unit.Clan);
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
            return unit.Position.IsAtDistance(position, distance);
        }

        public static bool IsAtSightDistance(this Unit unit, Unit enemy)
        {
            return unit.IsAtDistance(enemy.Position, unit.SightDistance);
        }

        public static bool IsAtAttackDistance(this Unit unit, Unit enemy)
        {
            return unit.IsAtDistance(enemy.Position, unit.DamageDistance);
        }

        public static bool IsIntersected(this Unit one, Unit two)
        {
            return one.IsAtDistance(two.Position, one.Radius + two.Radius);
        }

        public static IEnumerable<Unit> GetIntersectedUnits(this Unit unit)
        {
            return unit.GetActives().Where(u => IsIntersected(unit, u));
        }

        public static Vector GetMoveIntersectCorrection(this Unit unit, Vector move)
        {
            var nextUnitPosition = unit.Position + move;

            return unit.GetIntersectedUnits()
                .Select(u => new { V = nextUnitPosition - u.Position, R = u.Radius + unit.Radius })
                .Select(v => v.V.ToLength(v.R - v.V.Length))
                .Sum();
        }

        public static Line GetLinePath(this Unit unit, Unit enemy)
        {
            return new Line(unit.Position, enemy.Position);
        }

        public static Vector MoveToEnemy(this Unit unit, Unit enemy)
        {
            var obstacles = unit.GetActives().Where(u => u != enemy)
                .Select(u => new Circle(u.Position, u.Radius + unit.Radius)).ToArray();

            var pointToMove = new Line(unit.Position, unit.Position + (enemy.Position - unit.Position).ToLength(2*unit.Radius), unit.Id).FirstMoveToPassCircleObstacleGroup(unit.Speed, obstacles);

            return pointToMove - unit.Position;
        }
    }
}
