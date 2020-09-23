using System;

namespace MyStrategy.DataModel
{
    public delegate Vector VectorFn();

    public struct Vector
    {
        public static Vector Zero = (0, 0);

        public float X;
        public float Y;

        public Vector(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float Length => (float)Math.Sqrt(Length2);
        public float Length2 => X * X + Y * Y;
        public Vector ToLength(float len) => (len / Length) * this;

        public static Vector operator /(Vector a, float k)
        {
            return new Vector(a.X / k, a.Y / k);
        }

        public static Vector operator *(Vector a, float k)
        {
            return new Vector(a.X * k, a.Y * k);
        }

        public static Vector operator *(float k, Vector a)
        {
            return new Vector(a.X * k, a.Y * k);
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y);
        }

        public static implicit operator Vector((int, int) a)
        {
            return new Vector(a.Item1, a.Item2);
        }

        public static implicit operator Vector((float, float) a)
        {
            return new Vector(a.Item1, a.Item2);
        }

        public static implicit operator Vector((double, double) a)
        {
            return new Vector((float)a.Item1, (float)a.Item2);
        }

        public override string ToString()
        {
            return $"({X:F1}, {Y:F1})";
        }
    }
}