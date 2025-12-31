namespace Variable.Bounded;

/// <summary>
///     A bounded float value that is clamped between a configurable minimum and maximum.
///     Ideal for health, mana, stamina, temperature, and similar game mechanics.
/// </summary>
/// <remarks>
///     <para>The value is automatically clamped on construction and arithmetic operations.</para>
///     <para>This struct is blittable and can be used in Unity ECS, Burst jobs, and network serialization.</para>
/// </remarks>
/// <example>
///     <code>
/// // Health bar: 0 to 100
/// var health = new BoundedFloat(100f, 0f, 100f);
/// health -= 25f; // Take damage
/// 
/// // Temperature: -50°C to 50°C  
/// var temp = new BoundedFloat(50f, -50f, 22f);
/// </code>
/// </example>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{Current} [{Min}, {Max}]")]
public struct BoundedFloat :
    IBoundedInfo,
    IEquatable<BoundedFloat>,
    IComparable<BoundedFloat>,
    IComparable,
    IFormattable,
    IConvertible
{
    /// <summary>The current value, always clamped between <see cref="Min" /> and <see cref="Max" />.</summary>
    public float Current;

    /// <summary>The minimum allowed value (floor).</summary>
    public float Min;

    /// <summary>The maximum allowed value (ceiling).</summary>
    public float Max;

    /// <summary>Cached inverse of the range (1 / (Max - Min)) for faster ratio calculations.</summary>
    private readonly float _inverseRange;
    
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
    ///     Creates a new bounded float with min = 0 and current = max.
    /// </summary>
    /// <param name="max">The maximum allowed value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BoundedFloat(float max)
    {
        Max = max;
        Min = 0f;
        Current = max;
        var range = Max - Min;
        _inverseRange = Math.Abs(range) < MathConstants.Tolerance ? 0f : 1f / range;
    }

    /// <summary>
    ///     Creates a new bounded float with min = 0.
    /// </summary>
    /// <param name="max">The maximum allowed value.</param>
    /// <param name="current">The initial current value. Will be clamped to [0, max].</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BoundedFloat(float max, float current)
    {
        Max = max;
        Min = 0f;
        Current = current > max ? max : current < 0f ? 0f : current;
        var range = Max - Min;
        _inverseRange = Math.Abs(range) < MathConstants.Tolerance ? 0f : 1f / range;
    }

    /// <summary>
    ///     Creates a new bounded float with specified bounds and current value.
    /// </summary>
    /// <param name="max">The maximum allowed value.</param>
    /// <param name="min">The minimum allowed value.</param>
    /// <param name="current">The initial current value. Will be clamped to [min, max].</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BoundedFloat(float max, float min, float current)
    {
        Min = min;
        Max = max;
        Current = current > max ? max : current < min ? min : current;
        var range = Max - Min;
        _inverseRange = Math.Abs(range) < MathConstants.Tolerance ? 0f : 1f / range;
    }

    /// <summary>
    ///     Clamps the current value to the valid range [Min, Max].
    ///     Call this after directly modifying <see cref="Current" />.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Normalize()
    {
        Current = Current > Max ? Max : Current < Min ? Min : Current;
    }

    /// <summary>
    ///     Deconstructs the bounded value into its components.
    /// </summary>
    /// <param name="current">The current value.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="max">The maximum bound.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Deconstruct(out float current, out float min, out float max)
    {
        current = Current;
        min = Min;
        max = Max;
    }

    /// <summary>
    ///     Gets the range between min and max.
    /// </summary>
    public readonly float Range
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Max - Min;
    }

    /// <summary>
    ///     Gets the amount remaining until max.
    /// </summary>
    public readonly float Remaining
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Max - Current;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly double GetRatio()
    {
        return (Current - Min) * _inverseRange;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsFull()
    {
        return Current >= Max - MathConstants.Tolerance;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsEmpty()
    {
        return Current <= Min + MathConstants.Tolerance;
    }

    /// <summary>
    ///     Implicitly converts the bounded float to its current value.
    /// </summary>
    /// <param name="value">The bounded float.</param>
    /// <returns>The current value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator float(in BoundedFloat value)
    {
        return value.Current;
    }

    /// <inheritdoc />
    public readonly override string ToString()
    {
        return $"{Current}/{Max}";
    }

    /// <summary>
    ///     Formats the bounded value as a string.
    /// </summary>
    /// <param name="format">
    ///     Format string: "R" for ratio as percentage, "C" for current/max,
    ///     "F" for full format with min, or standard numeric formats.
    /// </param>
    /// <param name="formatProvider">The format provider.</param>
    /// <returns>The formatted string.</returns>
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
        return obj is BoundedFloat other && Equals(in other);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(BoundedFloat other)
    {
        return Equals(in other);
    }

    /// <summary>Compares equality with another bounded float using ref parameter.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(in BoundedFloat other)
    {
        return Current.Equals(other.Current) && Min.Equals(other.Min) && Max.Equals(other.Max);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int CompareTo(BoundedFloat other)
    {
        return CompareTo(in other);
    }

    /// <summary>Compares with another bounded float using ref parameter.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int CompareTo(in BoundedFloat other)
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
        if (obj is BoundedFloat other) return CompareTo(in other);
        throw new ArgumentException($"Object must be of type {nameof(BoundedFloat)}");
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
        return TypeCode.Single;
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
        return (decimal)Current;
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
        return (int)Current;
    }

    /// <inheritdoc />
    readonly long IConvertible.ToInt64(IFormatProvider provider)
    {
        return (long)Current;
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

    /// <summary>Determines whether two bounded floats are equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(in BoundedFloat left, in BoundedFloat right)
    {
        return left.Equals(in right);
    }

    /// <summary>Determines whether two bounded floats are not equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(in BoundedFloat left, in BoundedFloat right)
    {
        return !left.Equals(in right);
    }

    /// <summary>Determines whether one bounded float is less than another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(in BoundedFloat left, in BoundedFloat right)
    {
        return left.CompareTo(in right) < 0;
    }

    /// <summary>Determines whether one bounded float is less than or equal to another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(in BoundedFloat left, in BoundedFloat right)
    {
        return left.CompareTo(in right) <= 0;
    }

    /// <summary>Determines whether one bounded float is greater than another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(in BoundedFloat left, in BoundedFloat right)
    {
        return left.CompareTo(in right) > 0;
    }

    /// <summary>Determines whether one bounded float is greater than or equal to another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(in BoundedFloat left, in BoundedFloat right)
    {
        return left.CompareTo(in right) >= 0;
    }

    /// <summary>Increments the current value by 1, clamped to max.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedFloat operator ++(in BoundedFloat a)
    {
        return a + 1f;
    }

    /// <summary>Decrements the current value by 1, clamped to min.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedFloat operator --(in BoundedFloat a)
    {
        return a - 1f;
    }

    /// <summary>Adds a value to the bounded float, clamping the result.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedFloat operator +(in BoundedFloat a, float b)
    {
        return new BoundedFloat(a.Max, a.Min, a.Current + b);
    }

    /// <summary>Adds a value to the bounded float, clamping the result.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedFloat operator +(float b, in BoundedFloat a)
    {
        return a + b;
    }

    /// <summary>Subtracts a value from the bounded float, clamping the result.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedFloat operator -(in BoundedFloat a, float b)
    {
        return new BoundedFloat(a.Max, a.Min, a.Current - b);
    }
}