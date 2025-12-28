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
    public struct BoundedInt :
        IVariable,
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
            Current = current > max ? max : (current < 0 ? 0 : current);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedInt(int max) : this(max, max)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize() => Current = Current > Max ? Max : (Current < 0 ? 0 : Current);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out int current, out int max)
        {
            current = Current;
            max = Max;
        }

        private BoundedInt(SerializationInfo info, StreamingContext context)
        {
            Max = info.GetInt32(nameof(Max));
            int raw = info.GetInt32(nameof(Current));
            Current = raw > Max ? Max : (raw < 0 ? 0 : raw);
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
        public static implicit operator int(BoundedInt value) => value.Current;

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

        public override bool Equals(object obj) => obj is BoundedInt other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BoundedInt other) => Current == other.Current && Max == other.Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(BoundedInt other)
        {
            int cmp = Current.CompareTo(other.Current);
            return cmp != 0 ? cmp : Max.CompareTo(other.Max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj is BoundedInt other) return CompareTo(other);
            throw new ArgumentException($"Object must be of type {nameof(BoundedInt)}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(Current, Max);

        public TypeCode GetTypeCode() => TypeCode.Int32;
        bool IConvertible.ToBoolean(IFormatProvider provider) => Current != 0;
        byte IConvertible.ToByte(IFormatProvider provider) => (byte)Current;
        char IConvertible.ToChar(IFormatProvider provider) => (char)Current;
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException();
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Current;
        double IConvertible.ToDouble(IFormatProvider provider) => Current;
        short IConvertible.ToInt16(IFormatProvider provider) => (short)Current;
        int IConvertible.ToInt32(IFormatProvider provider) => Current;
        long IConvertible.ToInt64(IFormatProvider provider) => Current;
        sbyte IConvertible.ToSByte(IFormatProvider provider) => (sbyte)Current;
        float IConvertible.ToSingle(IFormatProvider provider) => Current;
        string IConvertible.ToString(IFormatProvider provider) => ToString("G", provider);

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) =>
            Convert.ChangeType(Current, conversionType, provider);

        ushort IConvertible.ToUInt16(IFormatProvider provider) => (ushort)Current;
        uint IConvertible.ToUInt32(IFormatProvider provider) => (uint)Current;
        ulong IConvertible.ToUInt64(IFormatProvider provider) => (ulong)Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(BoundedInt left, BoundedInt right) => left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(BoundedInt left, BoundedInt right) => !left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(BoundedInt left, BoundedInt right) => left.CompareTo(right) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(BoundedInt left, BoundedInt right) => left.CompareTo(right) <= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(BoundedInt left, BoundedInt right) => left.CompareTo(right) > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(BoundedInt left, BoundedInt right) => left.CompareTo(right) >= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedInt operator ++(BoundedInt a) => a + 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedInt operator --(BoundedInt a) => a - 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedInt operator +(BoundedInt a, int b)
        {
            long res = (long)a.Current + b;
            if (res > a.Max) res = a.Max;
            else if (res < 0) res = 0;
            return new BoundedInt(a.Max, (int)res);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedInt operator +(int b, BoundedInt a) => a + b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedInt operator -(BoundedInt a, int b) => a + (-b);
    }
}