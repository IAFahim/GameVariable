using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Variable.Core;

namespace Variable.Bounded
{
    /// <summary>
    /// A bounded double value that is clamped between 0 and a maximum.
    /// Ideal for high-precision calculations requiring double floating-point accuracy.
    /// </summary>
    /// <remarks>
    /// <para>The value is automatically clamped on construction and arithmetic operations.</para>
    /// <para>This struct is blittable and can be used in Unity ECS, Burst jobs, and network serialization.</para>
    /// </remarks>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay("{Current}/{Max}")]
    public struct BoundedDouble :
        IBoundedInfo,
        IEquatable<BoundedDouble>,
        IComparable<BoundedDouble>,
        IComparable,
        IFormattable,
        IConvertible
    {
        /// <summary>The current value, always clamped between 0 and <see cref="Max"/>.</summary>
        public double Current;

        /// <summary>The maximum allowed value (ceiling).</summary>
        public double Max;

        /// <summary>
        /// Creates a new bounded double with the specified max and current value.
        /// </summary>
        /// <param name="max">The maximum allowed value.</param>
        /// <param name="current">The initial current value. Will be clamped to [0, max].</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedDouble(double max, double current)
        {
            Max = max;
            Current = current > max ? max : current < 0.0 ? 0.0 : current;
        }

        /// <summary>
        /// Creates a new bounded double with current set to max.
        /// </summary>
        /// <param name="max">The maximum allowed value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundedDouble(double max) : this(max, max)
        {
        }

        /// <summary>
        /// Clamps the current value to the valid range [0, Max].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            Current = Current > Max ? Max : Current < 0.0 ? 0.0 : Current;
        }

        /// <summary>
        /// Deconstructs the bounded value into its components.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out double current, out double max)
        {
            current = Current;
            max = Max;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetRatio()
        {
            return Math.Abs(Max) < double.Epsilon ? 0.0 : Current / Max;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsFull() => Math.Abs(Current - Max) < double.Epsilon;

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsEmpty() => Math.Abs(Current) < double.Epsilon;

        /// <summary>Implicitly converts the bounded double to its current value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator double(BoundedDouble value)
        {
            return value.Current;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Current}/{Max}";
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is BoundedDouble other && Equals(other);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BoundedDouble other)
        {
            return Current.Equals(other.Current) && Max.Equals(other.Max);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(BoundedDouble other)
        {
            var cmp = Current.CompareTo(other.Current);
            return cmp != 0 ? cmp : Max.CompareTo(other.Max);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object obj)
        {
            if (obj is BoundedDouble other) return CompareTo(other);
            throw new ArgumentException($"Object must be of type {nameof(BoundedDouble)}");
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return HashCode.Combine(Current, Max);
        }

        /// <inheritdoc/>
        public TypeCode GetTypeCode() => TypeCode.Double;

        /// <inheritdoc/>
        bool IConvertible.ToBoolean(IFormatProvider provider) => Current != 0;

        /// <inheritdoc/>
        byte IConvertible.ToByte(IFormatProvider provider) => (byte)Current;

        /// <inheritdoc/>
        char IConvertible.ToChar(IFormatProvider provider) => (char)Current;

        /// <inheritdoc/>
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException();

        /// <inheritdoc/>
        decimal IConvertible.ToDecimal(IFormatProvider provider) => (decimal)Current;

        /// <inheritdoc/>
        double IConvertible.ToDouble(IFormatProvider provider) => Current;

        /// <inheritdoc/>
        short IConvertible.ToInt16(IFormatProvider provider) => (short)Current;

        /// <inheritdoc/>
        int IConvertible.ToInt32(IFormatProvider provider) => (int)Current;

        /// <inheritdoc/>
        long IConvertible.ToInt64(IFormatProvider provider) => (long)Current;

        /// <inheritdoc/>
        sbyte IConvertible.ToSByte(IFormatProvider provider) => (sbyte)Current;

        /// <inheritdoc/>
        float IConvertible.ToSingle(IFormatProvider provider) => (float)Current;

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

        /// <summary>Determines whether two bounded doubles are equal.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(BoundedDouble left, BoundedDouble right) => left.Equals(right);

        /// <summary>Determines whether two bounded doubles are not equal.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(BoundedDouble left, BoundedDouble right) => !left.Equals(right);

        /// <summary>Determines whether one bounded double is less than another.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(BoundedDouble left, BoundedDouble right) => left.CompareTo(right) < 0;

        /// <summary>Determines whether one bounded double is less than or equal to another.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(BoundedDouble left, BoundedDouble right) => left.CompareTo(right) <= 0;

        /// <summary>Determines whether one bounded double is greater than another.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(BoundedDouble left, BoundedDouble right) => left.CompareTo(right) > 0;

        /// <summary>Determines whether one bounded double is greater than or equal to another.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(BoundedDouble left, BoundedDouble right) => left.CompareTo(right) >= 0;

        /// <summary>Increments the current value by 1, clamped to max.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedDouble operator ++(BoundedDouble a) => a + 1.0;

        /// <summary>Decrements the current value by 1, clamped to 0.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedDouble operator --(BoundedDouble a) => a - 1.0;

        /// <summary>Adds a value to the bounded double, clamping the result.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedDouble operator +(BoundedDouble a, double b) => new BoundedDouble(a.Max, a.Current + b);

        /// <summary>Adds a value to the bounded double, clamping the result.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedDouble operator +(double b, BoundedDouble a) => a + b;

        /// <summary>Subtracts a value from the bounded double, clamping the result.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundedDouble operator -(BoundedDouble a, double b) => new BoundedDouble(a.Max, a.Current - b);
    }
}