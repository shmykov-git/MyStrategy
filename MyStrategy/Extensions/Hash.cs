using System.Linq;

namespace MyStrategy.Extensions
{
    public static class Hash
    {
        private static int GetHashCodeInternal(int v1, int v2)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + v1;
                hash = hash * 31 + v2;
                return hash;
            }
        }

        public static int GetHashCode(params int[] values)
        {
            return values.Aggregate(GetHashCodeInternal);
        }
    }
}