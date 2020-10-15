using System.Collections.Generic;
using System.Linq;

namespace MyStrategy.DataModel
{
    public class SquarePolygon : PathPoligon
    {
        private readonly Index center;
        private readonly int size;

        private static IEnumerable<Index> GetSquare(int size)
        {
            for (var k = -size; k < size; k++)
            {
                yield return (k, size);
                yield return (size, -k);
                yield return (-k, -size);
                yield return (-size, k);
            }
        }

        public override bool HasSubPoligon => size > 0;
        public override PathPoligon SubPoligon => size > 0 ? new SquarePolygon(center, size - 1) : null;

        public SquarePolygon(Index center, int size) 
            : base(GetSquare(size).Select(dir => center + dir).ToArray())
        {
            this.center = center;
            this.size = size;
        }

        public static implicit operator SquarePolygon(((int, int), int) a)
        {
            return new SquarePolygon(a.Item1, a.Item2);
        }
    }
}