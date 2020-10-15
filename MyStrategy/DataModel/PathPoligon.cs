using System.Collections;
using System.Collections.Generic;

namespace MyStrategy.DataModel
{
    public class PathPoligon : IEnumerable<Index>
    {
        protected Index[] Indexes { get; }

        public PathPoligon(Index[] indexes)
        {
            Indexes = indexes;
        }

        public virtual bool HasSubPoligon => SubPoligon != null;
        public virtual PathPoligon SubPoligon => null;

        public IEnumerator<Index> GetEnumerator()
        {
            foreach (var index in Indexes)
                yield return index;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}