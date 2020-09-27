using System;
using MyStrategy.Extensions;
using Newtonsoft.Json;

namespace MyStrategy.DataModel
{
    public delegate Vector VectorFn();

    public struct Vector : IEquatable<Vector>
    {
        public static Vector Zero = (0, 0);

        public float X;
        public float Y;

        public Vector(float x, float y)
        {
            X = x;
            Y = y;
        }

        [JsonIgnore]
        public float Length => Length2.Sqrt();
        [JsonIgnore]
        public float Length2 => X * X + Y * Y;
        
        public Vector ToLength(float len) => (len / Length) * this;

        [JsonIgnore]
        public bool IsZero => X.Abs() <= FloatExtensions.Epsilon && Y.Abs() <= FloatExtensions.Epsilon;
        [JsonIgnore]
        public bool IsDefault => Equals(default(Vector));
        [JsonIgnore]
        public Vector Unit => this / Length;
        [JsonIgnore]
        public Vector Ort => (Y, -X);
        [JsonIgnore]
        public Vector OrtUnit => Ort.Unit;

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

        public static Vector operator -(Vector a)
        {
            return new Vector(-a.X, -a.Y);
        }

        public static Vector operator ~(Vector a)
        {
            return new Vector(a.Y, -a.X);
        }

        public static float operator *(Vector a, Vector b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static bool operator ==(Vector a, Vector b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Vector a, Vector b)
        {
            return !a.Equals(b);
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

        public bool Equals(Vector other)
        {
            return (this - other).IsZero;
        }

        public override bool Equals(object obj)
        {
            return Equals((Vector) obj);
        }

        public override int GetHashCode()
        {
            return Hash.GetHashCode(X.ToEpsilonHash(), Y.ToEpsilonHash());
        }

        public override string ToString()
        {
            return $"({X:F1}, {Y:F1})";
        }
    }
}