using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Priority_Queue;
using Suit.Aspects;
using Suit.Extensions;

namespace MyStrategy.DataModel
{
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

        public bool this[Index ind]
        {
            get => net[ind.I, ind.J].Value;
            set => net[ind.I, ind.J].Value = value;
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

        private void AddTempWall(PathPoligon poligon)
        {
            poligon.ForEach(ind=> net[ind.I, ind.J].IsVisited = true);
        }

        public void AddWalls(params PathPoligon[] walls)
        {
            walls.ForEach(wall => wall.ForEach(ind => this[ind] = true));
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

        private static readonly Index[] Dirs = { (0, 1), (1, 0), (0, -1), (-1, 0) };
        private bool IsCorrect(Index ind) => ind.I >= 0 && ind.I < Dim.I && ind.J >= 0 && ind.J < Dim.J;
        private bool CanSet(Index ind) => IsCorrect(ind) && !net[ind.I, ind.J].Value && !net[ind.I, ind.J].IsVisited;

        #region FindNums

        [LoggingAspect(LoggingRule.Performance)]
        public void FindNums(Index findPos)
        {
            Clean();

            FindNumsInternal(findPos);

            isCleaned = false;
        }

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

        #endregion

        #region FindPath

        [LoggingAspect(LoggingRule.Performance)]
        public void FindPath(PathPoligon poligon)
        {
            Clean();

            FindPathInternal(poligon);

            isCleaned = false;
        }

        private void FindPathInternal(PathPoligon poligon)
        {
            if (poligon.HasSubPoligon)
                AddTempWall(poligon.SubPoligon);

            var queue = new Queue<(Index, Index)>(8 * Math.Min(Dim.I, Dim.J));
            poligon.ForEach(ind => queue.Enqueue((ind, Index.B)));

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

        #endregion

        #region FindPathFirst

        [LoggingAspect(LoggingRule.Performance)]
        public void FindPathFirst(PathPoligon poligon, Index a)
        {
            Clean();

            FindPathFirstInternal(poligon, a);

            isCleaned = false;
        }

        private void FindPathFirstInternal(PathPoligon poligon, Index aInd)
        {
            net[aInd.I, aInd.J].Dir = Index.A;

            if (poligon.HasSubPoligon)
                AddTempWall(poligon.SubPoligon);

            var stack = new Stack<(Index, Index)>(8 * Math.Min(Dim.I, Dim.J));
            poligon.OrderByDescending(ind => (aInd - ind).AbsSum)
                .ForEach(ind =>
                {
                    stack.Push((ind, Index.B));
                    net[ind.I, ind.J].Dir = Index.B;
                });

            while (stack.Count > 0)
            {
                var (ind, prevDir) = stack.Pop();

                if (ind == aInd)
                    return;

                if (CanSet(ind))
                {
                    net[ind.I, ind.J].Dir = prevDir;
                    net[ind.I, ind.J].IsVisited = true;
                    Dirs.OrderByDescending(dir => (aInd - dir - ind).AbsSum)
                        .ForEach(dir => stack.Push((ind + dir, dir)));
                }
            }
        }

        #endregion

        #region FindPathFirst

        private struct PathAItem
        {
            public int SortValue;
            public Index Ind;
            public Index Dir;

            public PathAItem(int sortValue, Index ind, Index dir)
            {
                SortValue = sortValue;
                Ind = ind;
                Dir = dir;
            }
        }

        [LoggingAspect(LoggingRule.Performance)]
        public void FindPathA(PathPoligon poligon, Index a)
        {
            Clean();

            FindPathAInternal(poligon, a);

            isCleaned = false;
        }

        private void FindPathAInternal(PathPoligon poligon, Index a)
        {
            if (poligon.HasSubPoligon)
                AddTempWall(poligon.SubPoligon);

            var queue = new Queue<(Index, Index)>(8 * Math.Min(Dim.I, Dim.J));
            poligon.ForEach(ind => queue.Enqueue((ind, Index.B)));

            while (queue.Count > 0)
            {
                var (ind, prevDir) = queue.Dequeue();
                if (CanSet(ind))
                {
                    net[ind.I, ind.J].Dir = prevDir;
                    net[ind.I, ind.J].IsVisited = true;
                    Dirs.ForEach(dir => queue.Enqueue((ind + dir, dir)));
                }

                //while (expression)
                //{
                    
                //}
            }
        }

        private void FindPathA1Internal(PathPoligon poligon, Index aInd)
        {
            net[aInd.I, aInd.J].Dir = Index.A;

            if (poligon.HasSubPoligon)
                AddTempWall(poligon.SubPoligon);

            var queue = new Queue<PathAItem>();

            var stack = new Stack<PathAItem>();
            poligon.OrderByDescending(ind => (aInd - ind).AbsSum)
                .ForEach(ind =>
                {
                    stack.Push(new PathAItem((aInd - ind).AbsSum, ind, Index.B));
                    net[ind.I, ind.J].Dir = Index.B;
                });

            while (stack.Count > 0)
            {
                var v = stack.Pop();

                if (v.Ind == aInd)
                    return;

                if (CanSet(v.Ind))
                {
                    net[v.Ind.I, v.Ind.J].Dir = v.Dir;
                    net[v.Ind.I, v.Ind.J].IsVisited = true;

                    var items = Dirs.Select(dir => new PathAItem((aInd - dir - v.Ind).AbsSum, v.Ind + dir, dir)).ToList();
                    var maxSortedValue = items.Max(x=>x.SortValue);
                    while (stack.Count>0 && stack.Peek().SortValue < maxSortedValue)
                    {
                        items.Add(stack.Pop());
                    }

                    items.OrderByDescending(item => item.SortValue)
                        .ForEach(item => stack.Push(item));
                }
            }
        }

        #endregion


        #region FindPath

        public class FindItemA : FastPriorityQueueNode
        {
            public Index Ind;
            public Index Dir;

            public static implicit operator FindItemA((Index, Index) a)
            {
                return new FindItemA {Ind = a.Item1, Dir = a.Item2};
            }

            public void Deconstruct(out Index ind, out Index dir)
            {
                ind = Ind;
                dir = Dir;
            }
        }

        [LoggingAspect(LoggingRule.Performance)]
        public void FindPath1(PathPoligon poligon, Index aInd)
        {
            Clean();
            net[aInd.I, aInd.J].Dir = Index.A;

            FindPathInternal1(poligon, aInd);

            isCleaned = false;
        }

        private void FindPathInternal1(PathPoligon poligon, Index a)
        {
            if (poligon.HasSubPoligon)
                AddTempWall(poligon.SubPoligon);

            var queue = new FastPriorityQueue<FindItemA>(3 * (Dim.I + Dim.J));
            poligon.ForEach(ind => queue.Enqueue((ind, Index.B), (a-ind).AbsSum));

            while (queue.Count > 0)
            {
                var (ind, prevDir) = queue.Dequeue();
                if (ind == a)
                    return;

                if (CanSet(ind))
                {
                    net[ind.I, ind.J].Dir = prevDir;
                    net[ind.I, ind.J].IsVisited = true;
                    Dirs.ForEach(dir => queue.Enqueue((ind + dir, dir), (a-ind-dir).AbsSum));
                }
            }
        }

        #endregion

        #region ToString

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

        private static readonly char[] Arrows = { '⚑', '•', ' ', '←', '↑', '→', '↓' };
        private static readonly List<Index> ArrowDirs = new[] { Index.A, Index.B, (0, 0), (0, 1), (1, 0), (0, -1), (-1, 0) }.ToList();
        private static char DirToArrow(Index dir) => Arrows[ArrowDirs.IndexOf(dir)];

        public string ToPath()
        {
            var builder = new StringBuilder(5 * Dim.I * Dim.J);
            for (var i = 0; i < Dim.I; i++)
            {
                for (var j = 0; j < Dim.J; j++)
                {
                    if (net[i, j].Value)
                        builder.Append($" ▄ ");
                    else
                        builder.Append($" {DirToArrow(net[i, j].Dir)} ");
                }

                builder.Append("\r\n");
            }

            return builder.ToString();
        }

        #endregion
    }
}