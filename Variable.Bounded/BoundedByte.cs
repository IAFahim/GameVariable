using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

using Variable.Core;

namespace Variable.Bounded
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{Current}/{Max}")]
    public struct BoundedByte :
        IVariable,
        IEquatable<BoundedByte>,
        IComparable<BoundedByte>,
        IComparable,
        IFormattable,
        IConvertible
    {
        public byte Current;
        public byte Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedByte(byte max, byte current)
        {
            Max = max;
            Current = current > max ? max : current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedByte(byte max) : this(max, max)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize() => Current = Current > Max ? Max : Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out byte current, out byte max)
        {
            current = Current;
            max = Max;
        }

        private BoundedByte(SerializationInfo info, StreamingContext context)
        {
            Max = info.GetByte(nameof(Max));
            byte raw = info.GetByte(nameof(Current));
            Current = raw > Max ? Max : raw;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetRatio() => Max == 0 ? 0.0 : (double)Current / Max;

        public bool IsFull
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Current == Max;
        }

        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Current == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator byte(BoundedByte value) => value.Current;

        public override string ToString() => $"{Current}/{Max}";

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

        public override bool Equals(object obj) => obj is BoundedByte other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BoundedByte other) => Current == other.Current && Max == other.Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(BoundedByte other)
        {
            int cmp = Current.CompareTo(other.Current);
            return cmp != 0 ? cmp : Max.CompareTo(other.Max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj is BoundedByte other) return CompareTo(other);
            throw new ArgumentException($"Object must be of type {nameof(BoundedByte)}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(Current, Max);

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
        public static bool operator ==(BoundedByte left, BoundedByte right) => left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(BoundedByte left, BoundedByte right) => !left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(BoundedByte left, BoundedByte right) => left.CompareTo(right) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(BoundedByte left, BoundedByte right) => left.CompareTo(right) <= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(BoundedByte left, BoundedByte right) => left.CompareTo(right) > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(BoundedByte left, BoundedByte right) => left.CompareTo(right) >= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedByte operator ++(BoundedByte a) => a + 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedByte operator --(BoundedByte a) => a - 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedByte operator +(BoundedByte a, int b)
        {
            int res = a.Current + b;
            if (res > a.Max) res = a.Max;
            else if (res < 0) res = 0;
            return new BoundedByte(a.Max, (byte)res);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedByte operator +(int b, BoundedByte a) => a + b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedByte operator -(BoundedByte a, int b) => a + (-b);
    }
}