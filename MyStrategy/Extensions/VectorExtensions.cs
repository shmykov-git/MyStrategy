using System.Collections.Generic;
using System.Linq;
using MyStrategy.DataModel;

namespace MyStrategy.Extensions
{
    public static class VectorExtensions
    {
        public static bool IsAtDistance(this Vector a, Vector b, float distance)
        {
            return (b - a).Length2 <= distance * distance;
        }

        public static Vector Sum(this IEnumerable<Vector> vectors)
        {
            return vectors.Aggregate(Vector.Zero, (a, b) => a + b);
        }

        public static Line ToLine(this Vector a, Vector b)
        {
            return new Line(a, b);
        }
    }
}