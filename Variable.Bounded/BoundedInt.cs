using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Variable.Core;

namespace Variable.Bounded
{
    /// <summary>
    /// A bounded integer value that is clamped between a configurable minimum and maximum.
    /// Ideal for health, ammo, level, score, and similar discrete game mechanics.
    /// </summary>
    /// <remarks>
    /// <para>The value is automatically clamped on construction and arithmetic operations.</para>
    /// <para>This struct is blittable and can be used in Unity ECS, Burst jobs, and network serialization.</para>
    /// <para>Arithmetic operations use <c>long</c> internally to prevent integer overflow.</para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Ammo clip: 0 to 30
    /// var ammo = new BoundedInt(30, 0, 30);
    /// ammo--; // Fire shot
    /// 
    /// // Karma system: -100 to 100
    /// var karma = new BoundedInt(100, -100, 0);
    /// karma += 10; // Good deed
    /// </code>
    /// </example>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{Current} [{Min}, {Max}]")]
    public struct BoundedInt :
        IBoundedInfo,
        IEquatable<BoundedInt>,
        IComparable<BoundedInt>,
        IComparable,
        IFormattable,
        IConvertible
    {
        /// <summary>The current value, always clamped between <see cref="Min"/> and <see cref="Max"/>.</summary>
        public int Current;

        /// <summary>The minimum allowed value (floor).</summary>
        public int Min;

        /// <summary>The maximum allowed value (ceiling).</summary>
        public int Max;

        /// <summary>
        /// Creates a new bounded integer with min = 0 and current = max.
        /// </summary>
        /// <param name="max">The maximum allowed value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedInt(int max)
        {
            Max = max;
            Min = 0;
            Current = max;
        }

        /// <summary>
        /// Creates a new bounded integer with min = 0.
        /// </summary>
        /// <param name="max">The maximum allowed value.</param>
        /// <param name="current">The initial current value. Will be clamped to [0, max].</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedInt(int max, int current)
        {
            Max = max;
            Min = 0;
            Current = current > max ? max : current < 0 ? 0 : current;
        }

        /// <summary>
        /// Creates a new bounded integer with specified bounds and current value.
        /// </summary>
        /// <param name="max">The maximum allowed value.</param>
        /// <param name="min">The minimum allowed value.</param>
        /// <param name="current">The initial current value. Will be clamped to [min, max].</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedInt(int max, int min, int current)
        {
            Min = min;
            Max = max;
            Current = current > max ? max : current < min ? min : current;
        }

        /// <summary>
        /// Clamps the current value to the valid range [Min, Max].
        /// Call this after directly modifying <see cref="Current"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            Current = Current > Max ? Max : Current < Min ? Min : Current;
        }

        /// <summary>
        /// Deconstructs the bounded value into its components.
        /// </summary>
        /// <param name="current">The current value.</param>
        /// <param name="min">The minimum bound.</param>
        /// <param name="max">The maximum bound.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out int current, out int min, out int max)
        {
            current = Current;
            min = Min;
            max = Max;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetRatio()
        {
            var range = Max - Min;
            return range == 0 ? 0.0 : (double)(Current - Min) / range;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsFull() => Current == Max;

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsEmpty() => Current == Min;

        /// <summary>
        /// Implicitly converts the bounded integer to its current value.
        /// </summary>
        /// <param name="value">The bounded integer.</param>
        /// <returns>The current value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator int(BoundedInt value)
        {
            return value.Current;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Current}/{Max}";
        }

        /// <summary>
        /// Formats the bounded value as a string.
        /// </summary>
        /// <param name="format">
        /// Format string: "R" for ratio as percentage, "C" for current/max, 
        /// "F" for full format with min, or standard numeric formats.
        /// </param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>The formatted string.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format)) format = "G";

            switch (format.ToUpperInvariant())
            {
                case "R": return GetRatio().ToString("P", formatProvider);
                case "C": return $"{Current}/{Max}";
                case "F": return $"{Current} [{Min}, {Max}]";
                default: return ToString();
            }
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is BoundedInt other && Equals(other);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BoundedInt other)
        {
            return Current == other.Current && Min == other.Min && Max == other.Max;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(BoundedInt other)
        {
            var cmp = Current.CompareTo(other.Current);
            if (cmp != 0) return cmp;
            cmp = Min.CompareTo(other.Min);
            return cmp != 0 ? cmp : Max.CompareTo(other.Max);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj is BoundedInt other) return CompareTo(other);
            throw new ArgumentException($"Object must be of type {nameof(BoundedInt)}");
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return HashCode.Combine(Current, Min, Max);
        }

        /// <inheritdoc/>
        public TypeCode GetTypeCode()
        {
            return TypeCode.Int32;
        }

        /// <inheritdoc/>
        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return Current != 0;
        }

        /// <inheritdoc/>
        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return (byte)Current;
        }

        /// <inheritdoc/>
        char IConvertible.ToChar(IFormatProvider provider)
        {
            return (char)Current;
        }

        /// <inheritdoc/>
        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        /// <inheritdoc/>
        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return Current;
        }

        /// <inheritdoc/>
        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return Current;
        }

        /// <inheritdoc/>
        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return (short)Current;
        }

        /// <inheritdoc/>
        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return Current;
        }

        /// <inheritdoc/>
        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Current;
        }

        /// <inheritdoc/>
        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return (sbyte)Current;
        }

        /// <inheritdoc/>
        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return Current;
        }

        /// <inheritdoc/>
        string IConvertible.ToString(IFormatProvider provider)
        {
            return ToString("G", provider);
        }

        /// <inheritdoc/>
        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(Current, conversionType, provider);
        }

        /// <inheritdoc/>
        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return (ushort)Current;
        }

        /// <inheritdoc/>
        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return (uint)Current;
        }

        /// <inheritdoc/>
        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return (ulong)Current;
        }

        /// <summary>Determines whether two bounded integers are equal.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(BoundedInt left, BoundedInt right)
        {
            return left.Equals(right);
        }

        /// <summary>Determines whether two bounded integers are not equal.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(BoundedInt left, BoundedInt right)
        {
            return !left.Equals(right);
        }

        /// <summary>Determines whether one bounded integer is less than another.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(BoundedInt left, BoundedInt right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>Determines whether one bounded integer is less than or equal to another.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(BoundedInt left, BoundedInt right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>Determines whether one bounded integer is greater than another.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(BoundedInt left, BoundedInt right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>Determines whether one bounded integer is greater than or equal to another.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(BoundedInt left, BoundedInt right)
        {
            return left.CompareTo(right) >= 0;
        }

        /// <summary>Increments the current value by 1, clamped to max.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedInt operator ++(BoundedInt a)
        {
            return a + 1;
        }

        /// <summary>Decrements the current value by 1, clamped to min.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedInt operator --(BoundedInt a)
        {
            return a - 1;
        }

        /// <summary>Adds a value to the bounded integer, clamping the result. Uses long internally to prevent overflow.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedInt operator +(BoundedInt a, int b)
        {
            var res = (long)a.Current + b;
            if (res > a.Max) res = a.Max;
            else if (res < a.Min) res = a.Min;
            return new BoundedInt(a.Max, a.Min, (int)res);
        }

        /// <summary>Adds a value to the bounded integer, clamping the result.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedInt operator +(int b, BoundedInt a)
        {
            return a + b;
        }

        /// <summary>Subtracts a value from the bounded integer, clamping the result.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedInt operator -(BoundedInt a, int b)
        {
            return a + -b;
        }
    }
}