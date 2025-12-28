using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Variable.Core;

namespace Variable.Range
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{Current} [{Min}, {Max}]")]
    public struct RangeInt :
        IBoundedInfo,
        IEquatable<RangeInt>,
        IComparable<RangeInt>,
        IComparable,
        IFormattable,
        IConvertible
    {
        public int Current;
        public int Min;
        public int Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RangeInt(int min, int max, int current)
        {
            Min = min;
            Max = max;
            Current = current > max ? max : current < min ? min : current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RangeInt(int min, int max) : this(min, max, min)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            Current = Current > Max ? Max : Current < Min ? Min : Current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out int current, out int min, out int max)
        {
            current = Current;
            min = Min;
            max = Max;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetRatio()
        {
            return Max == Min ? 0.0 : (double)(Current - Min) / (Max - Min);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsFull() => Current == Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsEmpty() => Current == Min;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator int(RangeInt value)
        {
            return value.Current;
        }

        public override string ToString()
        {
            return $"{Current} [{Min}, {Max}]";
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format)) format = "G";

            switch (format.ToUpperInvariant())
            {
                case "R": return GetRatio().ToString("P", formatProvider);
                case "C": return $"{Current} [{Min}, {Max}]";
                default: return ToString();
            }
        }

        public override bool Equals(object obj)
        {
            return obj is RangeInt other && Equals(other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(RangeInt other)
        {
            return Current == other.Current && Min == other.Min && Max == other.Max;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(RangeInt other)
        {
            var cmp = Current.CompareTo(other.Current);
            if (cmp != 0) return cmp;
            cmp = Min.CompareTo(other.Min);
            return cmp != 0 ? cmp : Max.CompareTo(other.Max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj is RangeInt other) return CompareTo(other);
            throw new ArgumentException($"Object must be of type {nameof(RangeInt)}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return HashCode.Combine(Current, Min, Max);
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
        public static bool operator ==(RangeInt left, RangeInt right)
        {
            return left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(RangeInt left, RangeInt right)
        {
            return !left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(RangeInt left, RangeInt right)
        {
            return left.CompareTo(right) < 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(RangeInt left, RangeInt right)
        {
            return left.CompareTo(right) <= 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(RangeInt left, RangeInt right)
        {
            return left.CompareTo(right) > 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(RangeInt left, RangeInt right)
        {
            return left.CompareTo(right) >= 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeInt operator ++(RangeInt a)
        {
            return a + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeInt operator --(RangeInt a)
        {
            return a - 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeInt operator +(RangeInt a, int b)
        {
            var res = (long)a.Current + b;
            if (res > a.Max) res = a.Max;
            else if (res < a.Min) res = a.Min;
            return new RangeInt(a.Min, a.Max, (int)res);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeInt operator +(int b, RangeInt a)
        {
            return a + b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeInt operator -(RangeInt a, int b)
        {
            return a + -b;
        }
    }
}