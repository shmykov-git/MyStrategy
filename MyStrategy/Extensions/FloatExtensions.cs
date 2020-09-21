using System;

namespace MyStrategy.Extensions
{
    public static class FloatExtensions
    {
        public static float Pow2(this float x)
        {
            return x * x;
        }

        public static int ToInt(this float x)
        {
            return (int)Math.Round(x);
        }
    }
}