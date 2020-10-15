using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using MyStrategy.DataModel;
using NLog;
using Suit;
using Suit.Extensions;
using Suit.Logs;

namespace MyStrategy.Extensions
{
    public static class LineExtensions
    {
        private static ILog log = IoC.Get<ILog>();

        public static Path ToPath(this Line line)
        {
            return new Path(line);
        }

        public static bool IsCrossing(this Line line, Circle obstacle)
        {
            return line.GetDistanceToLine(obstacle.Center) < obstacle.Radius
                   && line.IsInside(obstacle.Center);
        }

        private static Vector GetMoveIntersectObstaclesCorrection(Vector position, Vector move, Circle[] obstacles)
        {
            var line = new Line(position, position + move);

            return obstacles.Where(o => line.IsCrossing(o))
                .Select(o => new { V = line.B - o.Center, R = o.Radius })
                .Select(v => v.V.ToLength(v.R - v.V.Length))
                .Sum();
        }

        public static IEnumerable<Vector> PassCircleObstacle(this Line line, Circle obstacle, float move, Circle[] obstacleParts)
        {
            var distanceToCenter = (obstacle.Center - line.A).Length;

            var coef = 1 - (obstacle.Radius / distanceToCenter).Pow2();
            if (coef.Abs() < 0.01) // near circle bound
            {
                var projection = line.OrtUnit * (line.A - obstacle.Center);
                var alfa = projection > 0 ? 0.99f : 1.01f;

                var v1 = line.A + move * alfa * line.OrtUnit;
                yield return v1;

                var v2 = line.A - move / alfa * line.OrtUnit;
                yield return v2;
            }
            else // far from circle bound
            {
                var distance = obstacle.Radius / coef.Abs().Sqrt();

                if (coef > 0) // far outside circle
                {
                    var v1 = line.A + (obstacle.Center + distance * line.OrtUnit - line.A).ToLength(move);
                    yield return v1;

                    var v2 = line.A + (obstacle.Center - distance * line.OrtUnit - line.A).ToLength(move);
                    yield return v2;
                }
                else // far inside circle
                {

                    var projection = line.OrtUnit * (line.A - obstacle.Center);
                    //var alfa = projection > 0 ? 0.99f : 1.01f;

                    var move1 = move * line.OrtUnit;

                    var v1 = line.A + (move1 + GetMoveIntersectObstaclesCorrection(line.A, move1, obstacleParts)); // * alfa;
                    yield return v1;

                    //var move2 = -move * line.OrtUnit;

                    //var v2 = line.A + (move2 + GetMoveIntersectObstaclesCorrection(line.A, move2, obstacleParts)) / alfa;
                    //yield return v2;

                    //var d = line.GetDistanceToLine(obstacle.Center);
                    //var projDist = (obstacle.Radius.Pow2() - d.Pow2()).Sqrt();
                    //var correction = -2 * projDist * line.Unit;

                    //var v1 = line.A + (obstacle.Center + correction + distance * line.OrtUnit - line.A).ToLength(move);
                    //yield return v1;

                    //var v2 = line.A + (obstacle.Center + correction - distance * line.OrtUnit - line.A).ToLength(move);
                    //yield return v2;
                }
            }
        }

        public static Vector FirstMoveToPassCircleObstacleGroup(this Line line, float move, params Circle[] obstacles)
        {
            var spots = obstacles.Where(o => line.IsCrossing(o)).ToList();

            if (spots.Count == 0)
                return line.A + line.AB.ToLength(move);

            var others = obstacles.Where(o => !spots.Contains(o)).ToList();

            void FillSpot()
            {
                var newSpots = others.Where(o => spots.Any(s => s.IsCrossing(o))).ToList();
                if (newSpots.Count == 0)
                    return;

                spots.AddRange(newSpots);
                newSpots.ForEach(o => others.Remove(o));

                FillSpot();
            }

            FillSpot();

            var spot = spots.Join();

            var orderedPoints = line.PassCircleObstacle(spot, move, obstacles)
                .OrderBy(p => (p - line.A).Length + (line.B - p).Length);

            //if (line.Id == 2)
            //{
            //    var points = line.PassCircleObstacle(spot, move, obstacles).Select(p => (p - line.A).Length + (line.B - p).Length)
            //        .Select(l => l.ToString()).SJoin();
            //    log.Trace($"{line}: {points}");
            //}

            return orderedPoints.First();
        }
    }
}