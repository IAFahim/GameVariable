using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Variable.Range
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{Current} [{Min}, {Max}]")]
    public struct RangeFloat :
        IEquatable<RangeFloat>,
        IComparable<RangeFloat>,
        IComparable,
        IFormattable,
        IConvertible
    {
        public float Current;
        public float Min;
        public float Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RangeFloat(float min, float max, float current)
        {
            Min = min;
            Max = max;
            Current = current > max ? max : (current < min ? min : current);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RangeFloat(float min, float max) : this(min, max, min)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize() => Current = Current > Max ? Max : (Current < Min ? Min : Current);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out float current, out float min, out float max)
        {
            current = Current;
            min = Min;
            max = Max;
        }

        private RangeFloat(SerializationInfo info, StreamingContext context)
        {
            Min = info.GetSingle(nameof(Min));
            Max = info.GetSingle(nameof(Max));
            float raw = info.GetSingle(nameof(Current));
            Current = raw > Max ? Max : (raw < Min ? Min : raw);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetRatio() => Math.Abs(Max - Min) < float.Epsilon ? 0.0 : (Current - Min) / (Max - Min);

        public bool IsFull
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Math.Abs(Current - Max) < float.Epsilon;
        }

        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Math.Abs(Current - Min) < float.Epsilon;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator float(RangeFloat value) => value.Current;

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

        public override bool Equals(object obj) => obj is RangeFloat other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(RangeFloat other) => Current.Equals(other.Current) && Min.Equals(other.Min) && Max.Equals(other.Max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(RangeFloat other)
        {
            int cmp = Current.CompareTo(other.Current);
            if (cmp != 0) return cmp;
            cmp = Min.CompareTo(other.Min);
            return cmp != 0 ? cmp : Max.CompareTo(other.Max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj is RangeFloat other) return CompareTo(other);
            throw new ArgumentException($"Object must be of type {nameof(RangeFloat)}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(Current, Min, Max);

        public TypeCode GetTypeCode() => TypeCode.Single;
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
        float IConvertible.ToSingle(IFormatProvider provider) => Current;
        string IConvertible.ToString(IFormatProvider provider) => ToString("G", provider);

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) =>
            Convert.ChangeType(Current, conversionType, provider);

        ushort IConvertible.ToUInt16(IFormatProvider provider) => (ushort)Current;
        uint IConvertible.ToUInt32(IFormatProvider provider) => (uint)Current;
        ulong IConvertible.ToUInt64(IFormatProvider provider) => (ulong)Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(RangeFloat left, RangeFloat right) => left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(RangeFloat left, RangeFloat right) => !left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(RangeFloat left, RangeFloat right) => left.CompareTo(right) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(RangeFloat left, RangeFloat right) => left.CompareTo(right) <= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(RangeFloat left, RangeFloat right) => left.CompareTo(right) > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(RangeFloat left, RangeFloat right) => left.CompareTo(right) >= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeFloat operator ++(RangeFloat a) => a + 1f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeFloat operator --(RangeFloat a) => a - 1f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeFloat operator +(RangeFloat a, float b) => new RangeFloat(a.Min, a.Max, a.Current + b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeFloat operator +(float b, RangeFloat a) => a + b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeFloat operator -(RangeFloat a, float b) => new RangeFloat(a.Min, a.Max, a.Current - b);
    }
}
