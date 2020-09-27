using System;

namespace MyStrategy.Extensions
{
    public static class FloatExtensions
    {
        public static float Epsilon = 0.00001f;

        public static float Pow2(this float x)
        {
            return x * x;
        }

        public static int ToInt(this float x)
        {
            return (int)Math.Round(x);
        }

        public static float Abs(this float x)
        {
            return x < 0 ? -x : x;
        }

        public static float Sqrt(this float x)
        {
            return (float)Math.Sqrt(x);
        }

        public static int Sign(this float x)
        {
            return x > 0 ? 1 : -1;
        }

        public static decimal ToCheckValue(this float x)
        {
            return (decimal) (x.ToEpsilonHash() * Epsilon);
        }

        public static int ToEpsilonHash(this float x)
        {
            return (x / Epsilon).ToInt();
        }
    }
}