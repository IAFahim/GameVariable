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
    public struct BoundedByte :
        IBoundedInfo,
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
        public void Normalize()
        {
            Current = Current > Max ? Max : Current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out byte current, out byte max)
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
        public static implicit operator byte(BoundedByte value)
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
            return obj is BoundedByte other && Equals(other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BoundedByte other)
        {
            return Current == other.Current && Max == other.Max;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(BoundedByte other)
        {
            var cmp = Current.CompareTo(other.Current);
            return cmp != 0 ? cmp : Max.CompareTo(other.Max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj is BoundedByte other) return CompareTo(other);
            throw new ArgumentException($"Object must be of type {nameof(BoundedByte)}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return HashCode.Combine(Current, Max);
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.Byte;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return Current != 0;
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return Current;
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
            return Current;
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return Current;
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(BoundedByte left, BoundedByte right)
        {
            return left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(BoundedByte left, BoundedByte right)
        {
            return !left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(BoundedByte left, BoundedByte right)
        {
            return left.CompareTo(right) < 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(BoundedByte left, BoundedByte right)
        {
            return left.CompareTo(right) <= 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(BoundedByte left, BoundedByte right)
        {
            return left.CompareTo(right) > 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(BoundedByte left, BoundedByte right)
        {
            return left.CompareTo(right) >= 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedByte operator ++(BoundedByte a)
        {
            return a + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedByte operator --(BoundedByte a)
        {
            return a - 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedByte operator +(BoundedByte a, int b)
        {
            var res = a.Current + b;
            if (res > a.Max) res = a.Max;
            else if (res < 0) res = 0;
            return new BoundedByte(a.Max, (byte)res);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedByte operator +(int b, BoundedByte a)
        {
            return a + b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedByte operator -(BoundedByte a, int b)
        {
            return a + -b;
        }
    }
}