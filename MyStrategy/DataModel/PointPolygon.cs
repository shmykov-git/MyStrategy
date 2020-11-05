namespace MyStrategy.DataModel
{
    public class PointPolygon : PathPoligon
    {
        public PointPolygon(Index index) 
            : base(new []{ index })
        {
        }

        public static implicit operator PointPolygon((int, int) a)
        {
            return new PointPolygon(a);
        }
    }
}