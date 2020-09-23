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
        private Index dim;

        private List<Unit>[,] net;

        public SceneNet(Vector size, Vector frameSize, int frameCapacity = 16)
        {
            this.size = size;
            this.frameSize = frameSize;

            dim = GetIndex(size);

            net = new List<Unit>[dim.I, dim.J];
            for (var i = 0; i < dim.I; i++)
            for (var j = 0; j < dim.J; j++)
                net[i, j] = new List<Unit>(frameCapacity);
        }

        private Index GetIndex(Vector position) => new Index((int)(position.Y / frameSize.Y) + 1, (int)(position.X / frameSize.X) + 1);

        public void NotifyPosition(Unit unit)
        {
            var prev = unit.PositionIndex;
            var cur = GetIndex(unit.Position);
            if (prev == cur)
                return;

            net[prev.I, prev.J].Remove(unit);
            net[cur.I, cur.J].Add(unit);

            unit.PositionIndex = cur;
        }

        public void AddPosition(Unit unit)
        {
            var index = GetIndex(unit.Position);
            unit.PositionIndex = index;
            net[index.I, index.J].Add(unit);
        }

        public void RemovePosition(Unit unit)
        {
            var index = unit.PositionIndex;
            unit.PositionIndex = Index.Zero;
            net[index.I, index.J].Remove(unit);
        }

        public override string ToString()
        {
            var netInfo = SelectNodes().Where(info => info.List.Count > 0).Select(info =>
                    $"[{info.Index.I}, {info.Index.J}]: {info.List.Select(u => u.Id.ToString()).SJoin(", ")}")
                .SJoin("   ");

            return netInfo;
        }

        private struct NodeInfo
        {
            public Index Index;
            public List<Unit> List;
        }

        private IEnumerable<NodeInfo> SelectNodes()
        {
            for (var i = 0; i < dim.I; i++)
            for (var j = 0; j < dim.J; j++)
                yield return new NodeInfo {Index = new Index(i, j), List = net[i, j]};
        }

        public IEnumerable<Unit> GetActives(Unit unit)
        {
            var offset = GetIndex((unit.ActiveDistance, unit.ActiveDistance));
            var from = new Index(unit.PositionIndex.I - offset.I, unit.PositionIndex.J - offset.J).Correct(dim);
            var to = new Index(unit.PositionIndex.I + offset.I, unit.PositionIndex.J + offset.J).Correct(dim);

            for (var i = from.I; i < to.I; i++)
            for (var j = from.J; j < to.J; j++)
                foreach (var activeUnit in net[i, j])
                    if (activeUnit != unit)
                        yield return activeUnit;
        }
    }
}