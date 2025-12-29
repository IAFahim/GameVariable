namespace Variable.Bounded;

/// <summary>
///     A bounded integer value that is clamped between a configurable minimum and maximum.
///     Ideal for discrete values like item counts, levels, skill points, and currencies.
/// </summary>
/// <remarks>
///     <para>The value is automatically clamped on construction and arithmetic operations.</para>
///     <para>Arithmetic uses <c>long</c> internally to prevent integer overflow.</para>
///     <para>This struct is blittable and can be used in Unity ECS, Burst jobs, and network serialization.</para>
/// </remarks>
/// <example>
///     <code>
/// // Inventory slot: 0 to 64 items
/// var stack = new BoundedInt(64, 0, 32);
/// stack += 50; // Clamped to 64
/// 
/// // Karma: -100 to +100
/// var karma = new BoundedInt(100, -100, 0);
/// karma -= 150; // Clamped to -100
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
    /// <summary>The current value, always clamped between <see cref="Min" /> and <see cref="Max" />.</summary>
    public int Current;

    /// <summary>The minimum allowed value (floor).</summary>
    public int Min;

    /// <summary>The maximum allowed value (ceiling).</summary>
    public int Max;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float GetMin() => Min;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float GetCurrent() => Current;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float GetMax() => Max;

    /// <summary>
    ///     Creates a new bounded int with min = 0 and current = max.
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
    ///     Creates a new bounded int with min = 0.
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
    ///     Creates a new bounded int with specified bounds and current value.
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
    ///     Clamps the current value to the valid range [Min, Max].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Normalize()
    {
        Current = Current > Max ? Max : Current < Min ? Min : Current;
    }

    /// <summary>
    ///     Deconstructs the bounded value into its components.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Deconstruct(out int current, out int min, out int max)
    {
        current = Current;
        min = Min;
        max = Max;
    }

    /// <summary>
    ///     Gets the range between min and max.
    /// </summary>
    public readonly int Range
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Max - Min;
    }

    /// <summary>
    ///     Gets the amount remaining until max.
    /// </summary>
    public readonly int Remaining
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Max - Current;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly double GetRatio()
    {
        var range = Max - Min;
        return range == 0 ? 0.0 : (double)(Current - Min) / range;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsFull()
    {
        return Current == Max;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsEmpty()
    {
        return Current == Min;
    }

    /// <summary>Implicitly converts the bounded int to its current value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator int(in BoundedInt value)
    {
        return value.Current;
    }

    /// <inheritdoc />
    public readonly override string ToString()
    {
        return $"{Current}/{Max}";
    }

    /// <inheritdoc />
    public readonly string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (string.IsNullOrEmpty(format)) format = "G";

        switch (format.ToUpperInvariant())
        {
            case "R": return GetRatio().ToString("P", formatProvider);
            case "C": return $"{Current}/{Max}";
            case "F": return $"{Current} [{Min}, {Max}]";
            default: return $"{Current.ToString(format, formatProvider)}/{Max.ToString(format, formatProvider)}";
        }
    }

    /// <inheritdoc />
    public readonly override bool Equals(object obj)
    {
        return obj is BoundedInt other && Equals(in other);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(BoundedInt other)
    {
        return Equals(in other);
    }

    /// <summary>Compares equality with another bounded int using ref parameter.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(in BoundedInt other)
    {
        return Current == other.Current && Min == other.Min && Max == other.Max;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int CompareTo(BoundedInt other)
    {
        return CompareTo(in other);
    }

    /// <summary>Compares with another bounded int using ref parameter.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int CompareTo(in BoundedInt other)
    {
        var cmp = Current.CompareTo(other.Current);
        if (cmp != 0) return cmp;
        cmp = Min.CompareTo(other.Min);
        return cmp != 0 ? cmp : Max.CompareTo(other.Max);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int CompareTo(object obj)
    {
        if (obj is BoundedInt other) return CompareTo(in other);
        throw new ArgumentException($"Object must be of type {nameof(BoundedInt)}");
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly override int GetHashCode()
    {
        return HashCode.Combine(Current, Min, Max);
    }

    /// <inheritdoc />
    public readonly TypeCode GetTypeCode()
    {
        return TypeCode.Int32;
    }

    /// <inheritdoc />
    readonly bool IConvertible.ToBoolean(IFormatProvider provider)
    {
        return Current != 0;
    }

    /// <inheritdoc />
    readonly byte IConvertible.ToByte(IFormatProvider provider)
    {
        return (byte)Current;
    }

    /// <inheritdoc />
    readonly char IConvertible.ToChar(IFormatProvider provider)
    {
        return (char)Current;
    }

    /// <inheritdoc />
    readonly DateTime IConvertible.ToDateTime(IFormatProvider provider)
    {
        throw new InvalidCastException();
    }

    /// <inheritdoc />
    readonly decimal IConvertible.ToDecimal(IFormatProvider provider)
    {
        return Current;
    }

    /// <inheritdoc />
    readonly double IConvertible.ToDouble(IFormatProvider provider)
    {
        return Current;
    }

    /// <inheritdoc />
    readonly short IConvertible.ToInt16(IFormatProvider provider)
    {
        return (short)Current;
    }

    /// <inheritdoc />
    readonly int IConvertible.ToInt32(IFormatProvider provider)
    {
        return Current;
    }

    /// <inheritdoc />
    readonly long IConvertible.ToInt64(IFormatProvider provider)
    {
        return Current;
    }

    /// <inheritdoc />
    readonly sbyte IConvertible.ToSByte(IFormatProvider provider)
    {
        return (sbyte)Current;
    }

    /// <inheritdoc />
    readonly float IConvertible.ToSingle(IFormatProvider provider)
    {
        return Current;
    }

    /// <inheritdoc />
    readonly string IConvertible.ToString(IFormatProvider provider)
    {
        return ToString("G", provider);
    }

    /// <inheritdoc />
    readonly object IConvertible.ToType(Type conversionType, IFormatProvider provider)
    {
        return Convert.ChangeType(Current, conversionType, provider);
    }

    /// <inheritdoc />
    readonly ushort IConvertible.ToUInt16(IFormatProvider provider)
    {
        return (ushort)Current;
    }

    /// <inheritdoc />
    readonly uint IConvertible.ToUInt32(IFormatProvider provider)
    {
        return (uint)Current;
    }

    /// <inheritdoc />
    readonly ulong IConvertible.ToUInt64(IFormatProvider provider)
    {
        return (ulong)Current;
    }

    /// <summary>Determines whether two bounded ints are equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(in BoundedInt left, in BoundedInt right)
    {
        return left.Equals(in right);
    }

    /// <summary>Determines whether two bounded ints are not equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(in BoundedInt left, in BoundedInt right)
    {
        return !left.Equals(in right);
    }

    /// <summary>Determines whether one bounded int is less than another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(in BoundedInt left, in BoundedInt right)
    {
        return left.CompareTo(in right) < 0;
    }

    /// <summary>Determines whether one bounded int is less than or equal to another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(in BoundedInt left, in BoundedInt right)
    {
        return left.CompareTo(in right) <= 0;
    }

    /// <summary>Determines whether one bounded int is greater than another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(in BoundedInt left, in BoundedInt right)
    {
        return left.CompareTo(in right) > 0;
    }

    /// <summary>Determines whether one bounded int is greater than or equal to another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(in BoundedInt left, in BoundedInt right)
    {
        return left.CompareTo(in right) >= 0;
    }

    /// <summary>Increments the current value by 1, clamped to max.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedInt operator ++(in BoundedInt a)
    {
        return a + 1;
    }

    /// <summary>Decrements the current value by 1, clamped to min.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedInt operator --(in BoundedInt a)
    {
        return a - 1;
    }

    /// <summary>Adds a value to the bounded int, clamping the result. Uses long to prevent overflow.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedInt operator +(in BoundedInt a, int b)
    {
        var result = (long)a.Current + b;
        if (result > a.Max) result = a.Max;
        else if (result < a.Min) result = a.Min;
        return new BoundedInt(a.Max, a.Min, (int)result);
    }

    /// <summary>Adds a value to the bounded int, clamping the result.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedInt operator +(int b, in BoundedInt a)
    {
        return a + b;
    }

    /// <summary>Subtracts a value from the bounded int, clamping the result.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedInt operator -(in BoundedInt a, int b)
    {
        return a + -b;
    }
}