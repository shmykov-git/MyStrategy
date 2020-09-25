using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Suit.Extensions;

namespace MyStrategy.DataModel
{
    public class SceneNet
    {
        private readonly Vector size;
        private readonly Vector frameSize;
        private readonly int subNetSize;
        private Index dim;

        private Node[,] net;

        public SceneNet(Vector size, Vector frameSize, int subNetSize, int frameCapacity = 16)
        {
            this.size = size;
            this.frameSize = frameSize;
            this.subNetSize = subNetSize;

            dim = GetIndex(size);

            net = new Node[dim.I, dim.J];
            for (var i = 0; i < dim.I; i++)
            for (var j = 0; j < dim.J; j++)
                net[i, j] = new Node()
                {
                    List = new List<Unit>(frameCapacity),
                    SubNet = new bool[subNetSize, subNetSize],
                    SubNetSize = subNetSize,
                    NeedRefresh = true
                };
        }

        private Index GetIndex(Vector position) => new Index((int)(position.Y / frameSize.Y), (int)(position.X / frameSize.X));
        private Vector GetSubPosition(Vector position, Index index) => new Vector(position.X - index.J * frameSize.X, position.Y - index.I * frameSize.Y);
        private Index GetSubIndex(Vector position) => new Index((int)(position.Y / subNetSize), (int)(position.X / subNetSize));

        private void FillSubNet(Node node)
        {
            if (!node.NeedRefresh)
                return;

            var subNet = node.SubNet;
            for (var i = 0; i < subNetSize; i++)
            for (var j = 0; j < subNetSize; j++)
                subNet[i, j] = false;

            foreach (var unit in node.List)
            {
                var subIndex = GetSubIndex(GetSubPosition(unit.Position, unit.PositionIndex));
                subNet[subIndex.I, subIndex.J] = true;
            }
        }


        public void NotifyPosition(Unit unit)
        {
            var prev = unit.PositionIndex;
            var cur = GetIndex(unit.Position);

            net[prev.I, prev.J].NeedRefresh = true;
            net[cur.I, cur.J].NeedRefresh = true;

            if (prev == cur)
                return;

            net[prev.I, prev.J].List.Remove(unit);
            net[cur.I, cur.J].List.Add(unit);

            unit.PositionIndex = cur;
        }

        public void AddPosition(Unit unit)
        {
            var index = GetIndex(unit.Position);
            unit.PositionIndex = index;
            net[index.I, index.J].List.Add(unit);
            net[index.I, index.J].NeedRefresh = true;
        }

        public void RemovePosition(Unit unit)
        {
            var index = unit.PositionIndex;
            unit.PositionIndex = Index.Zero;
            net[index.I, index.J].List.Remove(unit);
            net[index.I, index.J].NeedRefresh = true;
        }

        private struct NodeInfo
        {
            public Index Index;
            public List<Unit> List;
        }

        public override string ToString()
        {
            var netInfo = SelectNodes().Where(info => info.List.Count > 0).Select(info =>
                    $"[{info.Index.I}, {info.Index.J}]: {info.List.Select(u => u.Id.ToString()).SJoin(", ")}")
                .SJoin("   ");

            return netInfo;
        }

        private IEnumerable<NodeInfo> SelectNodes()
        {
            for (var i = 0; i < dim.I; i++)
            for (var j = 0; j < dim.J; j++)
                yield return new NodeInfo {Index = new Index(i, j), List = net[i, j].List };
        }

        public IEnumerable<Unit> GetActives(Unit unit)
        {
            var offset = GetIndex((unit.ActiveDistance, unit.ActiveDistance));
            var from = new Index(unit.PositionIndex.I - offset.I, unit.PositionIndex.J - offset.J).Correct(dim);
            var to = new Index(unit.PositionIndex.I + offset.I, unit.PositionIndex.J + offset.J).Correct(dim);

            for (var i = from.I; i < to.I; i++)
            for (var j = from.J; j < to.J; j++)
                foreach (var activeUnit in net[i, j].List)
                    if (activeUnit != unit)
                        yield return activeUnit;
        }
    }
}