namespace MyStrategy.Extensions
{
    public static class IntExtentions
    {
        public static int Abs(this int x)
        {
            return x < 0 ? -x : x;
        }
    }
}