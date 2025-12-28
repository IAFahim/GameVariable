using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Variable.Core;

namespace Variable.Bounded
{
    /// <summary>
    /// A bounded long value that is clamped between 0 and a maximum.
    /// Ideal for large counters, experience points, currencies, and high-value integers.
    /// </summary>
    /// <remarks>
    /// <para>The value is automatically clamped on construction and arithmetic operations.</para>
    /// <para>This struct is blittable and can be used in Unity ECS, Burst jobs, and network serialization.</para>
    /// </remarks>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{Current}/{Max}")]
    public struct BoundedLong :
        IBoundedInfo,
        IEquatable<BoundedLong>,
        IComparable<BoundedLong>,
        IComparable,
        IFormattable,
        IConvertible
    {
        /// <summary>The current value, always clamped between 0 and <see cref="Max"/>.</summary>
        public long Current;

        /// <summary>The maximum allowed value (ceiling).</summary>
        public long Max;

        /// <summary>
        /// Creates a new bounded long with the specified max and current value.
        /// </summary>
        /// <param name="max">The maximum allowed value.</param>
        /// <param name="current">The initial current value. Will be clamped to [0, max].</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedLong(long max, long current)
        {
            Max = max;
            Current = current > max ? max : current < 0 ? 0L : current;
        }

        /// <summary>
        /// Creates a new bounded long with current set to max.
        /// </summary>
        /// <param name="max">The maximum allowed value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedLong(long max) : this(max, max)
        {
        }

        /// <summary>
        /// Clamps the current value to the valid range [0, Max].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            Current = Current > Max ? Max : Current < 0 ? 0L : Current;
        }

        /// <summary>
        /// Deconstructs the bounded value into its components.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out long current, out long max)
        {
            current = Current;
            max = Max;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetRatio()
        {
            return Max == 0 ? 0.0 : (double)Current / Max;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsFull() => Current == Max;

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsEmpty() => Current == 0;

        /// <summary>Implicitly converts the bounded long to its current value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator long(BoundedLong value) => value.Current;

        /// <inheritdoc/>
        public override string ToString() => $"{Current}/{Max}";

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is BoundedLong other && Equals(other);

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BoundedLong other) => Current == other.Current && Max == other.Max;

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(BoundedLong other)
        {
            var cmp = Current.CompareTo(other.Current);
            return cmp != 0 ? cmp : Max.CompareTo(other.Max);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj is BoundedLong other) return CompareTo(other);
            throw new ArgumentException($"Object must be of type {nameof(BoundedLong)}");
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(Current, Max);

        /// <inheritdoc/>
        public TypeCode GetTypeCode() => TypeCode.Int64;

        /// <inheritdoc/>
        bool IConvertible.ToBoolean(IFormatProvider provider) => Current != 0;

        /// <inheritdoc/>
        byte IConvertible.ToByte(IFormatProvider provider) => (byte)Current;

        /// <inheritdoc/>
        char IConvertible.ToChar(IFormatProvider provider) => (char)Current;

        /// <inheritdoc/>
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException();

        /// <inheritdoc/>
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Current;

        /// <inheritdoc/>
        double IConvertible.ToDouble(IFormatProvider provider) => Current;

        /// <inheritdoc/>
        short IConvertible.ToInt16(IFormatProvider provider) => (short)Current;

        /// <inheritdoc/>
        int IConvertible.ToInt32(IFormatProvider provider) => (int)Current;

        /// <inheritdoc/>
        long IConvertible.ToInt64(IFormatProvider provider) => Current;

        /// <inheritdoc/>
        sbyte IConvertible.ToSByte(IFormatProvider provider) => (sbyte)Current;

        /// <inheritdoc/>
        float IConvertible.ToSingle(IFormatProvider provider) => Current;

        /// <inheritdoc/>
        string IConvertible.ToString(IFormatProvider provider) => ToString("G", provider);

        /// <inheritdoc/>
        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(Current, conversionType, provider);

        /// <inheritdoc/>
        ushort IConvertible.ToUInt16(IFormatProvider provider) => (ushort)Current;

        /// <inheritdoc/>
        uint IConvertible.ToUInt32(IFormatProvider provider) => (uint)Current;

        /// <inheritdoc/>
        ulong IConvertible.ToUInt64(IFormatProvider provider) => (ulong)Current;

        /// <summary>Determines whether two bounded longs are equal.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(BoundedLong left, BoundedLong right) => left.Equals(right);

        /// <summary>Determines whether two bounded longs are not equal.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(BoundedLong left, BoundedLong right) => !left.Equals(right);

        /// <summary>Determines whether one bounded long is less than another.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(BoundedLong left, BoundedLong right) => left.CompareTo(right) < 0;

        /// <summary>Determines whether one bounded long is less than or equal to another.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(BoundedLong left, BoundedLong right) => left.CompareTo(right) <= 0;

        /// <summary>Determines whether one bounded long is greater than another.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(BoundedLong left, BoundedLong right) => left.CompareTo(right) > 0;

        /// <summary>Determines whether one bounded long is greater than or equal to another.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(BoundedLong left, BoundedLong right) => left.CompareTo(right) >= 0;

        /// <summary>Increments the current value by 1, clamped to max.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedLong operator ++(BoundedLong a) => a + 1;

        /// <summary>Decrements the current value by 1, clamped to 0.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedLong operator --(BoundedLong a) => a - 1;

        /// <summary>Adds a value to the bounded long, clamping the result.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedLong operator +(BoundedLong a, long b) => new BoundedLong(a.Max, a.Current + b);

        /// <summary>Adds a value to the bounded long, clamping the result.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedLong operator +(long b, BoundedLong a) => a + b;

        /// <summary>Subtracts a value from the bounded long, clamping the result.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedLong operator -(BoundedLong a, long b) => new BoundedLong(a.Max, a.Current - b);
    }
}