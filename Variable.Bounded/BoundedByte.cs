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
    IComparable
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
        return string.Format(CultureInfo.InvariantCulture, "{0}/{1}", Current, Max);
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