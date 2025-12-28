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
    public struct RangeDouble :
        IVariable,
        IEquatable<RangeDouble>,
        IComparable<RangeDouble>,
        IComparable,
        IFormattable,
        IConvertible
    {
        public double Current;
        public double Min;
        public double Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RangeDouble(double min, double max, double current)
        {
            Min = min;
            Max = max;
            Current = current > max ? max : (current < min ? min : current);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RangeDouble(double min, double max) : this(min, max, min)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize() => Current = Current > Max ? Max : (Current < Min ? Min : Current);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out double current, out double min, out double max)
        {
            current = Current;
            min = Min;
            max = Max;
        }

        private RangeDouble(SerializationInfo info, StreamingContext context)
        {
            Min = info.GetDouble(nameof(Min));
            Max = info.GetDouble(nameof(Max));
            double raw = info.GetDouble(nameof(Current));
            Current = raw > Max ? Max : (raw < Min ? Min : raw);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetRatio() => Math.Abs(Max - Min) < double.Epsilon ? 0.0 : (Current - Min) / (Max - Min);

        public bool IsFull
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Math.Abs(Current - Max) < double.Epsilon;
        }

        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Math.Abs(Current - Min) < double.Epsilon;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator double(RangeDouble value) => value.Current;

        public override string ToString() => $"{Current} [{Min}, {Max}]";

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format)) format = "G";

            switch (format.ToUpperInvariant())
            {
                case "R": return GetRatio().ToString("P", formatProvider);
                case "C": return $"{Current} [{Min}, {Max}]";
                default: return $"{Current.ToString(format, formatProvider)} [{Min.ToString(format, formatProvider)}, {Max.ToString(format, formatProvider)}]";
            }
        }

        public override bool Equals(object obj) => obj is RangeDouble other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(RangeDouble other) => Current.Equals(other.Current) && Min.Equals(other.Min) && Max.Equals(other.Max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(RangeDouble other)
        {
            int cmp = Current.CompareTo(other.Current);
            if (cmp != 0) return cmp;
            cmp = Min.CompareTo(other.Min);
            return cmp != 0 ? cmp : Max.CompareTo(other.Max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj is RangeDouble other) return CompareTo(other);
            throw new ArgumentException($"Object must be of type {nameof(RangeDouble)}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(Current, Min, Max);

        public TypeCode GetTypeCode() => TypeCode.Double;
        bool IConvertible.ToBoolean(IFormatProvider provider) => Current != 0;
        byte IConvertible.ToByte(IFormatProvider provider) => (byte)Current;
        char IConvertible.ToChar(IFormatProvider provider) => (char)Current;
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException();
        decimal IConvertible.ToDecimal(IFormatProvider provider) => (decimal)Current;
        double IConvertible.ToDouble(IFormatProvider provider) => Current;
        short IConvertible.ToInt16(IFormatProvider provider) => (short)Current;
        int IConvertible.ToInt32(IFormatProvider provider) => (int)Current;
        long IConvertible.ToInt64(IFormatProvider provider) => (long)Current;
        sbyte IConvertible.ToSByte(IFormatProvider provider) => (sbyte)Current;
        float IConvertible.ToSingle(IFormatProvider provider) => (float)Current;
        string IConvertible.ToString(IFormatProvider provider) => ToString("G", provider);

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) =>
            Convert.ChangeType(Current, conversionType, provider);

        ushort IConvertible.ToUInt16(IFormatProvider provider) => (ushort)Current;
        uint IConvertible.ToUInt32(IFormatProvider provider) => (uint)Current;
        ulong IConvertible.ToUInt64(IFormatProvider provider) => (ulong)Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(RangeDouble left, RangeDouble right) => left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(RangeDouble left, RangeDouble right) => !left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(RangeDouble left, RangeDouble right) => left.CompareTo(right) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(RangeDouble left, RangeDouble right) => left.CompareTo(right) <= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(RangeDouble left, RangeDouble right) => left.CompareTo(right) > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(RangeDouble left, RangeDouble right) => left.CompareTo(right) >= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeDouble operator ++(RangeDouble a) => a + 1.0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeDouble operator --(RangeDouble a) => a - 1.0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeDouble operator +(RangeDouble a, double b) => new RangeDouble(a.Min, a.Max, a.Current + b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeDouble operator +(double b, RangeDouble a) => a + b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeDouble operator -(RangeDouble a, double b) => new RangeDouble(a.Min, a.Max, a.Current - b);
    }
}
