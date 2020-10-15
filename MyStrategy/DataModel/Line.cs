using System;
using MyStrategy.Extensions;

namespace MyStrategy.DataModel
{
    public struct Line
    {
        public readonly int Id;
        public readonly Vector A;
        public readonly Vector B;

        public readonly Vector AB;
        public readonly Vector Unit;
        public readonly Vector OrtUnit;

        public float GetSignedDistanceToLine(Vector x) => OrtUnit * (x - A);
        public float GetDistanceToLine(Vector x) => GetSignedDistanceToLine(x).Abs();
        public float GetDistanceToA(Vector x) => Unit * (x - A);
        public Vector ProjectionPoint(Vector x) => A + Unit * GetDistanceToA(x);
        public bool IsInside(Vector x, float radius = 0) => Unit * (x - A) + radius > 0 && -Unit * (x - B) + radius > 0;
        public bool IsAInside(Vector x, float radius = 0) => Unit * (x - A) + radius > 0;

        public Line(Vector a, Vector b, int id = -1)
        {
            Id = id;
            A = a;
            B = b;

            AB = B - A;
            Unit = AB.Unit;
            OrtUnit = Unit.Ort;
        }

        public static implicit operator Line((Vector, Vector) a)
        {
            return new Line(a.Item1, a.Item2);
        }

        public override string ToString()
        {
            return $"({A}, {B})";
        }
    }
}