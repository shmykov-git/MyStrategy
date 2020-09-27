using System;
using MyStrategy.Extensions;

namespace MyStrategy.DataModel
{
    public struct Circle : IEquatable<Circle>
    {
        public readonly Vector Center;
        public readonly float Radius;

        public Circle(Vector center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public static implicit operator Circle((Vector, float) a)
        {
            return new Circle(a.Item1, a.Item2);
        }

        public bool Equals(Circle other)
        {
            return Center == other.Center && Radius.ToEpsilonHash() == other.Radius.ToEpsilonHash();
        }

        public override bool Equals(object obj)
        {
            return base.Equals((Circle)obj);
        }

        public override int GetHashCode()
        {
            return Hash.GetHashCode(Center.GetHashCode(), Radius.ToEpsilonHash());
        }
    }
}