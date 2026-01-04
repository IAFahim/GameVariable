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
        Current = BoundedLogic.Clamp(current, 0, max);
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
        Current = BoundedLogic.Clamp(current, min, max);
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
        return $"{Current}/{Max}";
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
        var result = (long)a.Current + b;
        if (result > a.Max) result = a.Max;
        else if (result < a.Min) result = a.Min;
        return new BoundedInt(a.Max, a.Min, (int)result);
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