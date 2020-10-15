using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Suit.Aspects;
using Suit.Extensions;

namespace MyStrategy.DataModel
{
    public class PathNode
    {
        public bool Value;
        public int PathNum;
    }

    public class PathGraph
    {
        private readonly Index dim;
        private PathNode[,] net;
        private bool isCleaned = true;

        public bool this[int i, int j]
        {
            get => net[i, j].Value;
            set => net[i, j].Value = value;
        }

        private void Clean()
        {
            if (isCleaned)
                return;

            for (var i = 0; i < dim.I; i++)
            for (var j = 0; j < dim.J; j++)
            {
                net[i, j].PathNum = 0;
            }

            isCleaned = true;
        }

        public PathGraph(int m, int n) : this((m, n)) { }

        public PathGraph(Index dim)
        {
            this.dim = dim;

            net = new PathNode[dim.I, dim.J];

            for (var i = 0; i < dim.I; i++)
            for (var j = 0; j < dim.J; j++)
                net[i, j] = new PathNode()
                {
                    
                };
        }

        [LoggingAspect(LoggingRule.Performance)]
        public void FindAll(Index findPos)
        {
            Clean();

            FindInternal(findPos);

            isCleaned = false;
        }

        private static readonly Index[] Dirs = { (0, 1), (1, 0), (0, -1), (-1, 0) };
        private bool IsCorrect(Index ind) => ind.I >= 0 && ind.I < dim.I && ind.J >= 0 && ind.J < dim.J;
        private bool CanSet(Index ind) => IsCorrect(ind) && !net[ind.I, ind.J].Value && net[ind.I, ind.J].PathNum == 0;

        public void FindInternal(Index findPos)
        {
            var queue = new Queue<(Index, int)>(8 * Math.Min(dim.I, dim.J));
            queue.Enqueue((findPos, 1));

            while (queue.Count > 0)
            {
                var (ind, num) = queue.Dequeue();
                if (CanSet(ind))
                {
                    net[ind.I, ind.J].PathNum = num;
                    Dirs.ForEach(dir => queue.Enqueue((ind + dir, num + 1)));
                }
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder(5 * dim.I * dim.J);
            for (var i = 0; i < dim.I; i++)
            {
                for (var j = 0; j < dim.J; j++)
                {
                    builder.Append(net[i, j].PathNum.ToString().PadLeft(4, ' '));
                }

                builder.Append("\r\n");
            }

            return builder.ToString();
        }
    }
}