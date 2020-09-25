using System.Collections.Generic;

namespace MyStrategy.DataModel
{
    public struct Node
    {
        // todo: bits
        public int SubNetSize;
        public bool[,] SubNet;
        public List<Unit> List;
        public bool NeedRefresh;
        public bool IsSimplyCrossable;
    }
}