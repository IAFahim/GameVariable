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
    IComparable
{
    /// <summary>The current value, always clamped between <see cref="Min" /> and <see cref="Max" />.</summary>
    public float Current;

    /// <summary>The minimum allowed value (floor).</summary>
    public float Min;

    /// <summary>The maximum allowed value (ceiling).</summary>
    public float Max;
    
    /// <inheritdoc />
    float IBoundedInfo.Min
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Min;
    }

    /// <inheritdoc />
    float IBoundedInfo.Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Current;
    }

    /// <inheritdoc />
    float IBoundedInfo.Max
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Max;
    }

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

    /// <summary>
    ///     Deconstructs the bounded float into its components.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Deconstruct(out float current, out float min, out float max)
    {
        current = Current;
        min = Min;
        max = Max;
    }

    /// <inheritdoc />
    public readonly override string ToString()
    {
        return $"{Current}/{Max}";
    }

    /// <summary>
    ///     Formats the bounded float with custom format strings.
    /// </summary>
    /// <param name="format">
    ///     Format code: "R" = Ratio (percentage), null = default format.
    /// </param>
    /// <param name="formatProvider">Unused, for IFormattable compatibility.</param>
    /// <returns>Formatted string.</returns>
    public readonly string ToString(string format, IFormatProvider formatProvider)
    {
        if (string.IsNullOrEmpty(format)) return ToString();
        
        if (format.ToUpperInvariant() == "R")
        {
            var ratio = this.GetRatio();
            return $"{ratio * 100:F1}%";
        }
        
        return ToString();
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