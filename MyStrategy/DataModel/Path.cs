using System.Collections.Generic;
using System.Data;

namespace MyStrategy.DataModel
{
    public class Path
    {
        public List<Vector> Points = new List<Vector>();

        public Path(Line line)
        {
            Points.Add(line.A);
            Points.Add(line.B);
        }

        public void Split(int index, Vector c)
        {
            Points.Insert(index, c);
        }

        public void Replace(int index, Vector c)
        {
            Points[index] = c;
        }
    }
}