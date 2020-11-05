using System;
using System.Collections.Generic;

namespace MyStrategy.Tools
{
    public class FuncComparer<T> : IComparer<T>
    {
        private readonly Func<T, T, int> compareFn;

        public FuncComparer(Func<T, T, int> compareFn)
        {
            this.compareFn = compareFn;
        }

        public int Compare(T x, T y)
        {
            return compareFn(x, y);
        }
    }
}