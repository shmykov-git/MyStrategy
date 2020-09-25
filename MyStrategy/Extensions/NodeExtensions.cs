using System.Collections.Generic;
using MyStrategy.DataModel;

namespace MyStrategy.Extensions
{
    public static class NodeExtensions
    {
        public static void RefreshSimplyCrossable(this Node node)
        {
            node.IsSimplyCrossable = true;
            var subNet = node.SubNet;

            for (var i = 0; i < node.SubNetSize; i++)
            {
                if (subNet[i, 0] || subNet[0, i] || subNet[i, node.SubNetSize - 1] || subNet[node.SubNetSize - 1, i])
                {
                    node.IsSimplyCrossable = false;
                    break;
                }
            }
        }

        private static Index[] moves = {(0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1)};

        //public static IEnumerable<Index> GetPath(Node node, Index from, Index to)
        //{

        //}



        //private static Index PathMove(Node node, Index from, Index to)
        //{
            
        //}
    }
}