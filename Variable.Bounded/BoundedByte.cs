namespace Variable.Bounded;

/// <summary>
///     A bounded byte value that is clamped between 0 and a maximum.
///     Ideal for compact storage when value ranges are very small (0-255).
/// </summary>
/// <remarks>
///     <para>The value is automatically clamped on construction and arithmetic operations.</para>
///     <para>This struct is blittable and can be used in Unity ECS, Burst jobs, and network serialization.</para>
///     <para>Bytes are unsigned, so the minimum is always 0.</para>
/// </remarks>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{Current}/{Max}")]
public struct BoundedByte :
    IBoundedInfo,
    IEquatable<BoundedByte>,
    IComparable<BoundedByte>,
    IComparable,
    IFormattable
{
    /// <summary>The current value, always clamped between 0 and <see cref="Max" />.</summary>
    public byte Current;

    /// <summary>The maximum allowed value (ceiling).</summary>
    public byte Max;

    /// <inheritdoc />
    float IBoundedInfo.Min
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => 0;
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
    ///     Creates a new bounded byte with the specified max and current value.
    /// </summary>
    /// <param name="max">The maximum allowed value.</param>
    /// <param name="current">The initial current value. Will be clamped to [0, max].</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BoundedByte(byte max, byte current)
    {
        Max = max;
        BoundedLogic.Clamp(current, max, out Current);
    }

    /// <summary>
    ///     Creates a new bounded byte with current set to max.
    /// </summary>
    /// <param name="max">The maximum allowed value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BoundedByte(byte max) : this(max, max)
    {
    }

    /// <summary>
    ///     Creates a new bounded byte with int values, clamping automatically.
    /// </summary>
    /// <param name="max">The maximum allowed value.</param>
    /// <param name="current">The initial current value. Will be clamped to [0, max].</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BoundedByte(byte max, int current)
    {
        Max = max;
        BoundedLogic.Clamp(current, max, out Current);
    }

    /// <summary>Implicitly converts the bounded byte to its current value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator byte(BoundedByte value)
    {
        return value.Current;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return ToString(null, null);
    }

    /// <summary>
    ///     Formats the bounded byte with custom format strings.
    /// </summary>
    /// <param name="format">
    ///     Format code: "R" = Ratio (percentage), null = default format.
    /// </param>
    /// <param name="formatProvider">The provider to use for formatting.</param>
    /// <returns>Formatted string.</returns>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        Span<char> buffer = stackalloc char[128];
        if (TryFormat(buffer, out var charsWritten, format, formatProvider))
            return new string(buffer.Slice(0, charsWritten));

        return string.Format(formatProvider ?? CultureInfo.InvariantCulture, "{0}/{1}", Current, Max);
    }

    /// <summary>
    ///     Formats the bounded byte into a character span (zero allocation).
    /// </summary>
    /// <param name="destination">The span to write the formatted value into.</param>
    /// <param name="charsWritten">The number of characters written.</param>
    /// <param name="format">Optional format string. "R" for ratio percentage.</param>
    /// <param name="provider">Optional format provider. Defaults to InvariantCulture.</param>
    /// <returns>True if formatting succeeded; false if destination was too small.</returns>
    /// <remarks>
    ///     This method is allocation-free and suitable for hot paths.
    ///     Example usage: Span&lt;char&gt; buffer = stackalloc char[128]; bounded.TryFormat(buffer, out var len);
    /// </remarks>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default,
        IFormatProvider? provider = default)
    {
        charsWritten = 0;
        provider ??= CultureInfo.InvariantCulture;

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
        if (!Current.TryFormat(destination, out var currentWritten, default, provider))
            return false;
        charsWritten = currentWritten;

        if (charsWritten >= destination.Length)
            return false;
        destination[charsWritten++] = '/';

        if (!Max.TryFormat(destination.Slice(charsWritten), out var maxWritten, default, provider))
            return false;
        charsWritten += maxWritten;

        return true;
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is BoundedByte other && Equals(other);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(BoundedByte other)
    {
        return Current == other.Current && Max == other.Max;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(BoundedByte other)
    {
        var cmp = Current.CompareTo(other.Current);
        return cmp != 0 ? cmp : Max.CompareTo(other.Max);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(object obj)
    {
        if (obj is BoundedByte other) return CompareTo(other);
        throw new ArgumentException($"Object must be of type {nameof(BoundedByte)}");
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return HashCode.Combine(Current, Max);
    }

    /// <summary>Determines whether two bounded bytes are equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(BoundedByte left, BoundedByte right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two bounded bytes are not equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(BoundedByte left, BoundedByte right)
    {
        return !left.Equals(right);
    }

    /// <summary>Determines whether one bounded byte is less than another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(BoundedByte left, BoundedByte right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>Determines whether one bounded byte is less than or equal to another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(BoundedByte left, BoundedByte right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>Determines whether one bounded byte is greater than another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(BoundedByte left, BoundedByte right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>Determines whether one bounded byte is greater than or equal to another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(BoundedByte left, BoundedByte right)
    {
        return left.CompareTo(right) >= 0;
    }

    /// <summary>Increments the current value by 1, clamped to max.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedByte operator ++(BoundedByte a)
    {
        return a + 1;
    }

    /// <summary>Decrements the current value by 1, clamped to 0.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedByte operator --(BoundedByte a)
    {
        return a - 1;
    }

    /// <summary>Adds a value to the bounded byte, clamping the result.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedByte operator +(BoundedByte a, int b)
    {
        return new BoundedByte(a.Max, a.Current + b);
    }

    /// <summary>Adds a value to the bounded byte, clamping the result.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedByte operator +(int b, BoundedByte a)
    {
        return a + b;
    }

    /// <summary>Subtracts a value from the bounded byte, clamping the result.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedByte operator -(BoundedByte a, int b)
    {
        return a + -b;
    }
}