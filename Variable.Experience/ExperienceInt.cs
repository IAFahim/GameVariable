using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Variable.Core;

namespace Variable.Experience
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("Lvl {Level} ({Current}/{Max})")]
    public struct ExperienceInt :
        IBoundedInfo,
        IEquatable<ExperienceInt>,
        IComparable<ExperienceInt>,
        IComparable,
        IFormattable,
        IConvertible
    {
        public int Current;
        public int Max;
        public int Level;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ExperienceInt(int max, int current = 0, int level = 1)
        {
            Max = max < 1 ? 1 : max;
            Current = current;
            Level = level;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out int current, out int max, out int level)
        {
            current = Current;
            max = Max;
            level = Level;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetRatio()
        {
            return Max == 0 ? 0.0 : (double)Current / Max;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsFull() => Current >= Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsEmpty() => Current == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator int(ExperienceInt value)
        {
            return value.Current;
        }

        public override string ToString()
        {
            return $"Lvl {Level} ({Current}/{Max})";
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format)) format = "G";

            switch (format.ToUpperInvariant())
            {
                case "R": return GetRatio().ToString("P", formatProvider);
                case "L": return Level.ToString(formatProvider);
                case "C": return $"{Current}/{Max}";
                default: return ToString();
            }
        }

        public override bool Equals(object obj)
        {
            return obj is ExperienceInt other && Equals(other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ExperienceInt other)
        {
            return Current == other.Current && Max == other.Max && Level == other.Level;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(ExperienceInt other)
        {
            var cmp = Level.CompareTo(other.Level);
            if (cmp != 0) return cmp;
            var cmpMax = Max.CompareTo(other.Max);
            if (cmpMax != 0) return cmpMax;
            return Current.CompareTo(other.Current);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj is ExperienceInt other) return CompareTo(other);
            throw new ArgumentException($"Object must be of type {nameof(ExperienceInt)}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return HashCode.Combine(Current, Max, Level);
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
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
            throw new InvalidCastException();
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
        public static bool operator ==(ExperienceInt left, ExperienceInt right)
        {
            return left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ExperienceInt left, ExperienceInt right)
        {
            return !left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(ExperienceInt left, ExperienceInt right)
        {
            return left.CompareTo(right) < 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(ExperienceInt left, ExperienceInt right)
        {
            return left.CompareTo(right) <= 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(ExperienceInt left, ExperienceInt right)
        {
            return left.CompareTo(right) > 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(ExperienceInt left, ExperienceInt right)
        {
            return left.CompareTo(right) >= 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ExperienceInt operator +(ExperienceInt a, int amount)
        {
            return new ExperienceInt(a.Max, a.Current + amount, a.Level);
        }
    }
}