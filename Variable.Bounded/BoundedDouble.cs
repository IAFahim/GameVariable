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
    public struct BoundedDouble :
        IVariable,
        IEquatable<BoundedDouble>,
        IComparable<BoundedDouble>,
        IComparable,
        IFormattable,
        IConvertible
    {
        public double Current;
        public double Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedDouble(double max, double current)
        {
            Max = max;
            Current = current > max ? max : current < 0.0 ? 0.0 : current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedDouble(double max) : this(max, max)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            Current = Current > Max ? Max : Current < 0.0 ? 0.0 : Current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out double current, out double max)
        {
            current = Current;
            max = Max;
        }

        private BoundedDouble(SerializationInfo info, StreamingContext context)
        {
            Max = info.GetDouble(nameof(Max));
            var raw = info.GetDouble(nameof(Current));
            Current = raw > Max ? Max : raw < 0.0 ? 0.0 : raw;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetRatio()
        {
            return Math.Abs(Max) < double.Epsilon ? 0.0 : Current / Max;
        }

        public bool IsFull
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Math.Abs(Current - Max) < double.Epsilon;
        }

        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Math.Abs(Current) < double.Epsilon;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator double(BoundedDouble value)
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
                default: return $"{Current.ToString(format, formatProvider)}/{Max.ToString(format, formatProvider)}";
            }
        }

        public override bool Equals(object obj)
        {
            return obj is BoundedDouble other && Equals(other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BoundedDouble other)
        {
            return Current.Equals(other.Current) && Max.Equals(other.Max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(BoundedDouble other)
        {
            var cmp = Current.CompareTo(other.Current);
            return cmp != 0 ? cmp : Max.CompareTo(other.Max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj is BoundedDouble other) return CompareTo(other);
            throw new ArgumentException($"Object must be of type {nameof(BoundedDouble)}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return HashCode.Combine(Current, Max);
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.Double;
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
            return (decimal)Current;
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
            return (int)Current;
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return (long)Current;
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return (sbyte)Current;
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return (float)Current;
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

        public static bool operator ==(BoundedDouble left, BoundedDouble right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BoundedDouble left, BoundedDouble right)
        {
            return !left.Equals(right);
        }

        public static bool operator <(BoundedDouble left, BoundedDouble right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(BoundedDouble left, BoundedDouble right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(BoundedDouble left, BoundedDouble right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(BoundedDouble left, BoundedDouble right)
        {
            return left.CompareTo(right) >= 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedDouble operator ++(BoundedDouble a)
        {
            return a + 1.0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedDouble operator --(BoundedDouble a)
        {
            return a - 1.0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedDouble operator +(BoundedDouble a, double b)
        {
            return new BoundedDouble(a.Max, a.Current + b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedDouble operator +(double b, BoundedDouble a)
        {
            return a + b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedDouble operator -(BoundedDouble a, double b)
        {
            return new BoundedDouble(a.Max, a.Current - b);
        }
    }
}