using System;
using System.Runtime.CompilerServices;
using MyStrategy.Extensions;

namespace MyStrategy.DataModel
{
    public struct Index : IEquatable<Index>
    {
        public static Index Zero => new Index(0, 0);

        public readonly int I;
        public readonly int J;

        public Index(int i, int j)
        {
            I = i;
            J = j;
        }

        public static bool operator ==(Index a, Index b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Index a, Index b)
        {
            return !a.Equals(b);
        }

        public bool Equals(Index a)
        {
            return I == a.I && J == a.J;
        }

        public override bool Equals(object obj)
        {
            return Equals((Index) obj);
        }

        public override int GetHashCode()
        {
            return Hash.GetHashCode(I, J);
        }

        public override string ToString()
        {
            return $"{I}, {J}";
        }

        public Index Correct(Index cor)
        {
            return new Index(I < 0 ? 0 : (I > cor.I ? cor.I : I), J < 0 ? 0 : (J > cor.J ? cor.J : J));
        }
    }
}