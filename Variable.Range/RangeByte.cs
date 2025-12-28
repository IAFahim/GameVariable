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
    public struct RangeByte :
        IVariable,
        IEquatable<RangeByte>,
        IComparable<RangeByte>,
        IComparable,
        IFormattable,
        IConvertible
    {
        public byte Current;
        public byte Min;
        public byte Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RangeByte(byte min, byte max, byte current)
        {
            Min = min;
            Max = max;
            Current = current > max ? max : (current < min ? min : current);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RangeByte(byte min, byte max) : this(min, max, min)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize() => Current = Current > Max ? Max : (Current < Min ? Min : Current);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out byte current, out byte min, out byte max)
        {
            current = Current;
            min = Min;
            max = Max;
        }

        private RangeByte(SerializationInfo info, StreamingContext context)
        {
            Min = info.GetByte(nameof(Min));
            Max = info.GetByte(nameof(Max));
            byte raw = info.GetByte(nameof(Current));
            Current = raw > Max ? Max : (raw < Min ? Min : raw);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetRatio() => Max == Min ? 0.0 : (double)(Current - Min) / (Max - Min);

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
        public static implicit operator byte(RangeByte value) => value.Current;

        public override string ToString() => $"{Current} [{Min}, {Max}]";

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

        public override bool Equals(object obj) => obj is RangeByte other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(RangeByte other) => Current == other.Current && Min == other.Min && Max == other.Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(RangeByte other)
        {
            int cmp = Current.CompareTo(other.Current);
            if (cmp != 0) return cmp;
            cmp = Min.CompareTo(other.Min);
            return cmp != 0 ? cmp : Max.CompareTo(other.Max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj is RangeByte other) return CompareTo(other);
            throw new ArgumentException($"Object must be of type {nameof(RangeByte)}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(Current, Min, Max);

        public TypeCode GetTypeCode() => TypeCode.Byte;
        bool IConvertible.ToBoolean(IFormatProvider provider) => Current != 0;
        byte IConvertible.ToByte(IFormatProvider provider) => Current;
        char IConvertible.ToChar(IFormatProvider provider) => (char)Current;
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException();
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Current;
        double IConvertible.ToDouble(IFormatProvider provider) => Current;
        short IConvertible.ToInt16(IFormatProvider provider) => Current;
        int IConvertible.ToInt32(IFormatProvider provider) => Current;
        long IConvertible.ToInt64(IFormatProvider provider) => Current;
        sbyte IConvertible.ToSByte(IFormatProvider provider) => (sbyte)Current;
        float IConvertible.ToSingle(IFormatProvider provider) => Current;
        string IConvertible.ToString(IFormatProvider provider) => ToString("G", provider);

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) =>
            Convert.ChangeType(Current, conversionType, provider);

        ushort IConvertible.ToUInt16(IFormatProvider provider) => Current;
        uint IConvertible.ToUInt32(IFormatProvider provider) => Current;
        ulong IConvertible.ToUInt64(IFormatProvider provider) => Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(RangeByte left, RangeByte right) => left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(RangeByte left, RangeByte right) => !left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(RangeByte left, RangeByte right) => left.CompareTo(right) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(RangeByte left, RangeByte right) => left.CompareTo(right) <= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(RangeByte left, RangeByte right) => left.CompareTo(right) > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(RangeByte left, RangeByte right) => left.CompareTo(right) >= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeByte operator ++(RangeByte a) => a + 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeByte operator --(RangeByte a) => a - 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeByte operator +(RangeByte a, int b)
        {
            int res = a.Current + b;
            if (res > a.Max) res = a.Max;
            else if (res < a.Min) res = a.Min;
            return new RangeByte(a.Min, a.Max, (byte)res);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeByte operator +(int b, RangeByte a) => a + b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeByte operator -(RangeByte a, int b) => a + (-b);
    }
}
