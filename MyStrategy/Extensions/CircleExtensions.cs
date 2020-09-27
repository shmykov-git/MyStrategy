using System.Collections.Generic;
using System.Linq;
using MyStrategy.DataModel;

namespace MyStrategy.Extensions
{
    public static class CircleExtensions
    {
        public static bool IsInside(this Circle circle, Vector v)
        {
            return circle.Center.IsAtDistance(v, circle.Radius - FloatExtensions.Epsilon);
        }

        public static bool IsCrossing(this Circle circle, Circle other)
        {
            return circle.Center.IsAtDistance(other.Center, circle.Radius + other.Radius);
        }

        public static Circle Join(this IEnumerable<Circle> circles)
        {
            return circles.Aggregate((a, b) => new Circle(0.5f * (a.Center + b.Center), 0.5f * (a.Radius + b.Radius + (b.Center - a.Center).Length)));
        }
    }
}