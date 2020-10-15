using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Suit.Aspects;
using Suit.Extensions;

namespace MyStrategy.DataModel
{
    public class PathNode
    {
        public bool Value;
        public bool IsVisited;
        public int PathNum;
        public Index Dir;
    }

    public class PathGraph
    {
        public Index Dim { get; }

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

            for (var i = 0; i < Dim.I; i++)
            for (var j = 0; j < Dim.J; j++)
            {
                net[i, j].IsVisited = false;
            }

            isCleaned = true;
        }

        public PathGraph(int m, int n) : this((m, n)) { }

        public PathGraph(Index dim)
        {
            this.Dim = dim;

            net = new PathNode[dim.I, dim.J];

            for (var i = 0; i < dim.I; i++)
            for (var j = 0; j < dim.J; j++)
                net[i, j] = new PathNode()
                {
                    
                };
        }

        [LoggingAspect(LoggingRule.Performance)]
        public void FindNums(Index findPos)
        {
            Clean();

            FindNumsInternal(findPos);

            isCleaned = false;
        }

        [LoggingAspect(LoggingRule.Performance)]
        public void FindPath(Index findPos)
        {
            Clean();

            FindPathInternal(findPos);

            isCleaned = false;
        }

        private static readonly char[] Arrows = { '•', '←', '↑', '→', '↓' };
        private static readonly List<Index> ArrowDirs = new Index[] {(0, 0), (0, 1), (1, 0), (0, -1), (-1, 0)}.ToList();
        private static char DirToArrow(Index dir) => Arrows[ArrowDirs.IndexOf(dir)];

        private static readonly Index[] Dirs = { (0, 1), (1, 0), (0, -1), (-1, 0) };
        private bool IsCorrect(Index ind) => ind.I >= 0 && ind.I < Dim.I && ind.J >= 0 && ind.J < Dim.J;
        private bool CanSet(Index ind) => IsCorrect(ind) && !net[ind.I, ind.J].Value && !net[ind.I, ind.J].IsVisited;

        private void FindNumsInternal(Index findPos)
        {
            var queue = new Queue<(Index, int)>(8 * Math.Min(Dim.I, Dim.J));
            queue.Enqueue((findPos, 1));

            while (queue.Count > 0)
            {
                var (ind, num) = queue.Dequeue();
                if (CanSet(ind))
                {
                    net[ind.I, ind.J].PathNum = num;
                    net[ind.I, ind.J].IsVisited = true;
                    Dirs.ForEach(dir => queue.Enqueue((ind + dir, num + 1)));
                }
            }
        }

        private void FindPathInternal(Index findPos)
        {
            var queue = new Queue<(Index, Index)>(8 * Math.Min(Dim.I, Dim.J));
            queue.Enqueue((findPos, Index.Zero));

            while (queue.Count > 0)
            {
                var (ind, prevDir) = queue.Dequeue();
                if (CanSet(ind))
                {
                    net[ind.I, ind.J].Dir = prevDir;
                    net[ind.I, ind.J].IsVisited = true;
                    Dirs.ForEach(dir => queue.Enqueue((ind + dir, dir)));
                }
            }
        }

        public string ToNums()
        {
            var builder = new StringBuilder(5 * Dim.I * Dim.J);
            for (var i = 0; i < Dim.I; i++)
            {
                for (var j = 0; j < Dim.J; j++)
                {
                    builder.Append(net[i, j].PathNum.ToString().PadLeft(4, ' '));
                }

                builder.Append("\r\n");
            }

            return builder.ToString();
        }

        public string ToPath()
        {
            var builder = new StringBuilder(5 * Dim.I * Dim.J);
            for (var i = 0; i < Dim.I; i++)
            {
                for (var j = 0; j < Dim.J; j++)
                {
                    builder.Append($" {DirToArrow(net[i, j].Dir)} ");
                }

                builder.Append("\r\n");
            }

            return builder.ToString();
        }
    }
}