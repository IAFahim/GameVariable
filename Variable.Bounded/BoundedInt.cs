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
    IComparable
{
    /// <summary>The current value, always clamped between <see cref="Min" /> and <see cref="Max" />.</summary>
    public int Current;

    /// <summary>The minimum allowed value (floor).</summary>
    public int Min;

    /// <summary>The maximum allowed value (ceiling).</summary>
    public int Max;

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
        BoundedLogic.Clamp(current, 0, max, out Current);
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
        BoundedLogic.Clamp(current, min, max, out Current);
    }

    /// <summary>Implicitly converts the bounded int to its current value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator int(BoundedInt value)
    {
        return value.Current;
    }

    /// <inheritdoc />
    public readonly override string ToString()
    {
        Span<char> buffer = stackalloc char[32];
        if (TryFormat(buffer, out var charsWritten, default, CultureInfo.InvariantCulture))
        {
            return new string(buffer.Slice(0, charsWritten));
        }

        return string.Format(CultureInfo.InvariantCulture, "{0}/{1}", Current, Max);
    }

    /// <summary>
    ///     Formats the bounded int into a character span (zero allocation).
    /// </summary>
    /// <param name="destination">The span to write the formatted value into.</param>
    /// <param name="charsWritten">The number of characters written.</param>
    /// <param name="format">Optional format string. "R" for ratio percentage.</param>
    /// <param name="provider">Optional format provider.</param>
    /// <returns>True if formatting succeeded; false if destination was too small.</returns>
    /// <remarks>
    ///     This method is allocation-free and suitable for hot paths.
    ///     Example usage: Span&lt;char&gt; buffer = stackalloc char[32]; bounded.TryFormat(buffer, out var len);
    /// </remarks>
    public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default,
        IFormatProvider provider = null)
    {
        charsWritten = 0;

        // Handle ratio format
        if (format.Length > 0 && (format[0] == 'R' || format[0] == 'r'))
        {
            var ratio = this.GetRatio() * 100.0;
            if (!ratio.TryFormat(destination, out var written, "F1", provider))
                return false;
            charsWritten = written;

            if (charsWritten >= destination.Length)
                return false;
            destination[charsWritten++] = '%';
            return true;
        }

        // Default format: "Current/Max"
        if (!Current.TryFormat(destination, out var currentWritten, format, provider))
            return false;
        charsWritten = currentWritten;

        if (charsWritten >= destination.Length)
            return false;
        destination[charsWritten++] = '/';

        if (!Max.TryFormat(destination.Slice(charsWritten), out var maxWritten, format, provider))
            return false;
        charsWritten += maxWritten;

        return true;
    }

    /// <inheritdoc />
    public readonly override bool Equals(object obj)
    {
        return obj is BoundedInt other && Equals(other);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(BoundedInt other)
    {
        return Current == other.Current && Min == other.Min && Max == other.Max;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int CompareTo(BoundedInt other)
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
        if (obj is BoundedInt other) return CompareTo(other);
        throw new ArgumentException($"Object must be of type {nameof(BoundedInt)}");
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly override int GetHashCode()
    {
        return HashCode.Combine(Current, Min, Max);
    }

    /// <summary>Determines whether two bounded ints are equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(BoundedInt left, BoundedInt right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two bounded ints are not equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(BoundedInt left, BoundedInt right)
    {
        return !left.Equals(right);
    }

    /// <summary>Determines whether one bounded int is less than another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(BoundedInt left, BoundedInt right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>Determines whether one bounded int is less than or equal to another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(BoundedInt left, BoundedInt right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>Determines whether one bounded int is greater than another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(BoundedInt left, BoundedInt right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>Determines whether one bounded int is greater than or equal to another.</summary>
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

    /// <summary>Adds a value to the bounded int, clamping the result. Uses long to prevent overflow.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedInt operator +(BoundedInt a, int b)
    {
        BoundedLogic.Add(a.Current, a.Min, a.Max, b, out var newCurrent);
        return new BoundedInt(a.Max, a.Min, newCurrent);
    }

    /// <summary>Adds a value to the bounded int, clamping the result.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedInt operator +(int b, BoundedInt a)
    {
        return a + b;
    }

    /// <summary>Subtracts a value from the bounded int, clamping the result.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedInt operator -(BoundedInt a, int b)
    {
        return a + -b;
    }
}