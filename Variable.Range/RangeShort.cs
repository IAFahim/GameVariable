using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Variable.Core;

namespace Variable.Range
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{Current} [{Min}, {Max}]")]
    public struct RangeShort :
        IVariable,
        IEquatable<RangeShort>,
        IComparable<RangeShort>,
        IComparable,
        IFormattable,
        IConvertible
    {
        public short Current;
        public short Min;
        public short Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RangeShort(short min, short max, short current)
        {
            Min = min;
            Max = max;
            Current = current > max ? max : current < min ? min : current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RangeShort(short min, short max) : this(min, max, min)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            Current = Current > Max ? Max : Current < Min ? Min : Current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out short current, out short min, out short max)
        {
            current = Current;
            min = Min;
            max = Max;
        }

        private RangeShort(SerializationInfo info, StreamingContext context)
        {
            Min = info.GetInt16(nameof(Min));
            Max = info.GetInt16(nameof(Max));
            var raw = info.GetInt16(nameof(Current));
            Current = raw > Max ? Max : raw < Min ? Min : raw;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetRatio()
        {
            return Max == Min ? 0.0 : (double)(Current - Min) / (Max - Min);
        }

        public bool IsFull
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Current == Max;
        }

        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Current == Min;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator short(RangeShort value)
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
            return obj is RangeShort other && Equals(other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(RangeShort other)
        {
            return Current == other.Current && Min == other.Min && Max == other.Max;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(RangeShort other)
        {
            var cmp = Current.CompareTo(other.Current);
            if (cmp != 0) return cmp;
            cmp = Min.CompareTo(other.Min);
            return cmp != 0 ? cmp : Max.CompareTo(other.Max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj is RangeShort other) return CompareTo(other);
            throw new ArgumentException($"Object must be of type {nameof(RangeShort)}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return HashCode.Combine(Current, Min, Max);
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.Int16;
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
            return Current;
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
        public static bool operator ==(RangeShort left, RangeShort right)
        {
            return left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(RangeShort left, RangeShort right)
        {
            return !left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(RangeShort left, RangeShort right)
        {
            return left.CompareTo(right) < 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(RangeShort left, RangeShort right)
        {
            return left.CompareTo(right) <= 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(RangeShort left, RangeShort right)
        {
            return left.CompareTo(right) > 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(RangeShort left, RangeShort right)
        {
            return left.CompareTo(right) >= 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeShort operator ++(RangeShort a)
        {
            return a + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeShort operator --(RangeShort a)
        {
            return a - 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeShort operator +(RangeShort a, int b)
        {
            var res = a.Current + b;
            if (res > a.Max) res = a.Max;
            else if (res < a.Min) res = a.Min;
            return new RangeShort(a.Min, a.Max, (short)res);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeShort operator +(int b, RangeShort a)
        {
            return a + b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeShort operator -(RangeShort a, int b)
        {
            return a + -b;
        }
    }
}