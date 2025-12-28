using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Variable.Bounded
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{Current}/{Max}")]
    public struct BoundedFloat :
        IEquatable<BoundedFloat>,
        IComparable<BoundedFloat>,
        IComparable,
        IFormattable,
        IConvertible
    {
        public float Current;
        public float Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedFloat(float max, float current)
        {
            Max = max;
            Current = current > max ? max : (current < 0f ? 0f : current);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedFloat(float max) : this(max, max)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize() => Current = Current > Max ? Max : (Current < 0f ? 0f : Current);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out float current, out float max)
        {
            current = Current;
            max = Max;
        }

        private BoundedFloat(SerializationInfo info, StreamingContext context)
        {
            Max = info.GetSingle(nameof(Max));
            float raw = info.GetSingle(nameof(Current));
            Current = raw > Max ? Max : (raw < 0f ? 0f : raw);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetRatio() => Math.Abs(Max) < float.Epsilon ? 0.0 : Current / Max;

        public bool IsFull
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Math.Abs(Current - Max) < float.Epsilon;
        }

        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Math.Abs(Current) < float.Epsilon;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator float(BoundedFloat value) => value.Current;

        public override string ToString() => $"{Current}/{Max}";

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format)) format = "G";

            switch (format.ToUpperInvariant())
            {
                case "R": return GetRatio().ToString("P", formatProvider);
                case "C": return $"{Current}/{Max}";
                default: return $"{Current.ToString(format, formatProvider)}/{Max.ToString(format, formatProvider)}";
            }
        }

        public override bool Equals(object obj) => obj is BoundedFloat other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BoundedFloat other) => Current.Equals(other.Current) && Max.Equals(other.Max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(BoundedFloat other)
        {
            int cmp = Current.CompareTo(other.Current);
            return cmp != 0 ? cmp : Max.CompareTo(other.Max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj is BoundedFloat other) return CompareTo(other);
            throw new ArgumentException($"Object must be of type {nameof(BoundedFloat)}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(Current, Max);

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
        public static bool operator ==(BoundedFloat left, BoundedFloat right) => left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(BoundedFloat left, BoundedFloat right) => !left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(BoundedFloat left, BoundedFloat right) => left.CompareTo(right) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(BoundedFloat left, BoundedFloat right) => left.CompareTo(right) <= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(BoundedFloat left, BoundedFloat right) => left.CompareTo(right) > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(BoundedFloat left, BoundedFloat right) => left.CompareTo(right) >= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedFloat operator ++(BoundedFloat a) => a + 1f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedFloat operator --(BoundedFloat a) => a - 1f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedFloat operator +(BoundedFloat a, float b) => new BoundedFloat(a.Max, a.Current + b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedFloat operator +(float b, BoundedFloat a) => a + b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedFloat operator -(BoundedFloat a, float b) => new BoundedFloat(a.Max, a.Current - b);
    }
}