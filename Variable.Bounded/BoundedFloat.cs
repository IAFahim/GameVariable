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
        BoundedLogic.Clamp(current, 0f, max, out Current);
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
        BoundedLogic.Clamp(current, min, max, out Current);
    }

    /// <summary>
    ///     Implicitly converts the bounded float to its current value.
    /// </summary>
    /// <param name="value">The bounded float.</param>
    /// <returns>The current value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator float(BoundedFloat value)
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
        Span<char> destination = stackalloc char[32];
        return TryFormat(destination, out var charsWritten)
            ? new string(destination.Slice(0, charsWritten))
            : string.Format(CultureInfo.InvariantCulture, "{0}/{1}", Current, Max);
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

    /// <summary>
    ///     Formats the bounded float into a character span (zero allocation).
    /// </summary>
    /// <param name="destination">The span to write the formatted value into.</param>
    /// <param name="charsWritten">The number of characters written.</param>
    /// <param name="format">Optional format string. "R" for ratio percentage.</param>
    /// <returns>True if formatting succeeded; false if destination was too small.</returns>
    /// <remarks>
    ///     This method is allocation-free and suitable for hot paths.
    ///     Example usage: Span&lt;char&gt; buffer = stackalloc char[32]; bounded.TryFormat(buffer, out var len);
    /// </remarks>
    public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default)
    {
        charsWritten = 0;

        // Handle ratio format
        if (format.Length > 0 && (format[0] == 'R' || format[0] == 'r'))
        {
            var ratio = this.GetRatio() * 100.0;
            if (!ratio.TryFormat(destination, out var written, "F1"))
                return false;
            charsWritten = written;

            if (charsWritten >= destination.Length)
                return false;
            destination[charsWritten++] = '%';
            return true;
        }

        // Default format: "Current/Max"
        if (!Current.TryFormat(destination, out var currentWritten))
            return false;
        charsWritten = currentWritten;

        if (charsWritten >= destination.Length)
            return false;
        destination[charsWritten++] = '/';

        if (!Max.TryFormat(destination.Slice(charsWritten), out var maxWritten))
            return false;
        charsWritten += maxWritten;

        return true;
    }

    /// <inheritdoc />
    public readonly override bool Equals(object obj)
    {
        return obj is BoundedFloat other && Equals(other);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(BoundedFloat other)
    {
        return Current.Equals(other.Current) && Min.Equals(other.Min) && Max.Equals(other.Max);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int CompareTo(BoundedFloat other)
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
        if (obj is BoundedFloat other) return CompareTo(other);
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
    public static bool operator ==(BoundedFloat left, BoundedFloat right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two bounded floats are not equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(BoundedFloat left, BoundedFloat right)
    {
        return !left.Equals(right);
    }

    /// <summary>Determines whether one bounded float is less than another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(BoundedFloat left, BoundedFloat right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>Determines whether one bounded float is less than or equal to another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(BoundedFloat left, BoundedFloat right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>Determines whether one bounded float is greater than another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(BoundedFloat left, BoundedFloat right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>Determines whether one bounded float is greater than or equal to another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(BoundedFloat left, BoundedFloat right)
    {
        return left.CompareTo(right) >= 0;
    }

    /// <summary>Increments the current value by 1, clamped to max.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedFloat operator ++(BoundedFloat a)
    {
        return a + 1f;
    }

    /// <summary>Decrements the current value by 1, clamped to min.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedFloat operator --(BoundedFloat a)
    {
        return a - 1f;
    }

    /// <summary>Adds a value to the bounded float, clamping the result.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedFloat operator +(BoundedFloat a, float b)
    {
        return new BoundedFloat(a.Max, a.Min, a.Current + b);
    }

    /// <summary>Adds a value to the bounded float, clamping the result.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedFloat operator +(float b, BoundedFloat a)
    {
        return a + b;
    }

    /// <summary>Subtracts a value from the bounded float, clamping the result.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedFloat operator -(BoundedFloat a, float b)
    {
        return new BoundedFloat(a.Max, a.Min, a.Current - b);
    }

    /// <summary>Multiplies the bounded float by a scalar value, clamping the result.</summary>
    /// <remarks>Useful for percentage-based calculations like "Take 50% damage".</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedFloat operator *(BoundedFloat a, float scalar)
    {
        return new BoundedFloat(a.Max, a.Min, a.Current * scalar);
    }

    /// <summary>Multiplies the bounded float by a scalar value, clamping the result.</summary>
    /// <remarks>Useful for percentage-based calculations like "Take 50% damage".</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedFloat operator *(float scalar, BoundedFloat a)
    {
        return a * scalar;
    }

    /// <summary>Divides the bounded float by a scalar value, clamping the result.</summary>
    /// <remarks>Useful for scaling calculations.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedFloat operator /(BoundedFloat a, float divisor)
    {
        return new BoundedFloat(a.Max, a.Min, a.Current / divisor);
    }
}