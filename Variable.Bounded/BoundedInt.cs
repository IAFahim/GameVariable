using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Variable.Core;

namespace Variable.Bounded
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{Current}/{Max}")]
    public struct BoundedInt :
        IBoundedInfo,
        IEquatable<BoundedInt>,
        IComparable<BoundedInt>,
        IComparable,
        IFormattable,
        IConvertible
    {
        public int Current;
        public int Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedInt(int max, int current)
        {
            Max = max;
            Current = current > max ? max : current < 0 ? 0 : current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedInt(int max) : this(max, max)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            Current = Current > Max ? Max : Current < 0 ? 0 : Current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out int current, out int max)
        {
            current = Current;
            max = Max;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetRatio()
        {
            return Max == 0 ? 0.0 : (double)Current / Max;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsFull() => Current == Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsEmpty() => Current == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator int(BoundedInt value)
        {
            return value.Current;
        }

        public override string ToString()
        {
            return $"{Current}/{Max}";
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format)) format = "G";

            switch (format.ToUpperInvariant())
            {
                case "R": return GetRatio().ToString("P", formatProvider);
                case "C": return $"{Current}/{Max}";
                default: return ToString();
            }
        }

        public override bool Equals(object obj)
        {
            return obj is BoundedInt other && Equals(other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BoundedInt other)
        {
            return Current == other.Current && Max == other.Max;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(BoundedInt other)
        {
            var cmp = Current.CompareTo(other.Current);
            return cmp != 0 ? cmp : Max.CompareTo(other.Max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj is BoundedInt other) return CompareTo(other);
            throw new ArgumentException($"Object must be of type {nameof(BoundedInt)}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return HashCode.Combine(Current, Max);
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.Int32;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return Current != 0;
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return (byte)Current;
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return (char)Current;
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return Current;
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return Current;
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return (short)Current;
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return Current;
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Current;
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return (sbyte)Current;
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return Current;
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return ToString("G", provider);
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(Current, conversionType, provider);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return (ushort)Current;
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return (uint)Current;
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return (ulong)Current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(BoundedInt left, BoundedInt right)
        {
            return left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(BoundedInt left, BoundedInt right)
        {
            return !left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(BoundedInt left, BoundedInt right)
        {
            return left.CompareTo(right) < 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(BoundedInt left, BoundedInt right)
        {
            return left.CompareTo(right) <= 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(BoundedInt left, BoundedInt right)
        {
            return left.CompareTo(right) > 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(BoundedInt left, BoundedInt right)
        {
            return left.CompareTo(right) >= 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedInt operator ++(BoundedInt a)
        {
            return a + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedInt operator --(BoundedInt a)
        {
            return a - 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedInt operator +(BoundedInt a, int b)
        {
            var res = (long)a.Current + b;
            if (res > a.Max) res = a.Max;
            else if (res < 0) res = 0;
            return new BoundedInt(a.Max, (int)res);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedInt operator +(int b, BoundedInt a)
        {
            return a + b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedInt operator -(BoundedInt a, int b)
        {
            return a + -b;
        }
    }
}