using System.Collections.Generic;
using System.Linq;
using MyStrategy.DataModel;

namespace MyStrategy.Extensions
{
    public static class VectorExtensions
    {
        public static Vector Sum(this IEnumerable<Vector> vectors)
        {
            return vectors.Aggregate(Vector.Zero, (a, b) => a + b);
        }
    }
}