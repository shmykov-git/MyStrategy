using System;
using MyStrategy.Extensions;

namespace MyStrategy.DataModel
{
    public struct Line
    {
        public Vector A;
        public Vector B;

        public float GetSignedDistance(Vector c)
        {
            var x0 = c.X;
            var y0 = c.Y;
            var x1 = A.X;
            var y1 = A.Y;
            var x2 = B.X;
            var y2 = B.Y;

            var d = ((y2 - y1) * x0 - (x2 - x1) * y0 + x2 * y1 - y2 * x1) /
                    (float) Math.Sqrt((y2 - y1).Pow2() + (x2 - x1).Pow2());

            return d;
        }

        public Line(Vector a, Vector b)
        {
            A = a;
            B = b;
        }
    }
}