namespace Variable.Bounded;

/// <summary>
///     A bounded long value that is clamped between 0 and a maximum.
///     Ideal for large counters, experience points, currencies, and high-value integers.
/// </summary>
/// <remarks>
///     <para>The value is automatically clamped on construction and arithmetic operations.</para>
///     <para>This struct is blittable and can be used in Unity ECS, Burst jobs, and network serialization.</para>
/// </remarks>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{Current}/{Max}")]
public struct BoundedLong :
    IBoundedInfo,
    IEquatable<BoundedLong>,
    IComparable<BoundedLong>,
    IComparable,
    IFormattable,
    IConvertible
{
    /// <summary>The current value, always clamped between 0 and <see cref="Max" />.</summary>
    public long Current;

    /// <summary>The maximum allowed value (ceiling).</summary>
    public long Max;

    /// <summary>
    ///     Creates a new bounded long with the specified max and current value.
    /// </summary>
    /// <param name="max">The maximum allowed value.</param>
    /// <param name="current">The initial current value. Will be clamped to [0, max].</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BoundedLong(long max, long current)
    {
        Max = max;
        Current = current > max ? max : current < 0 ? 0L : current;
    }

    /// <summary>
    ///     Creates a new bounded long with current set to max.
    /// </summary>
    /// <param name="max">The maximum allowed value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BoundedLong(long max) : this(max, max)
    {
    }

    /// <summary>
    ///     Clamps the current value to the valid range [0, Max].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Normalize()
    {
        Current = Current > Max ? Max : Current < 0 ? 0L : Current;
    }

    /// <summary>
    ///     Deconstructs the bounded value into its components.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out long current, out long max)
    {
        current = Current;
        max = Max;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double GetRatio()
    {
        return Max == 0 ? 0.0 : (double)Current / Max;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsFull()
    {
        return Current == Max;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsEmpty()
    {
        return Current == 0;
    }

    /// <summary>Implicitly converts the bounded long to its current value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator long(BoundedLong value)
    {
        return value.Current;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Current}/{Max}";
    }

    /// <inheritdoc />
    public string ToString(string format, IFormatProvider formatProvider)
    {
        if (string.IsNullOrEmpty(format)) format = "G";

        switch (format.ToUpperInvariant())
        {
            case "R": return GetRatio().ToString("P", formatProvider);
            case "C": return $"{Current}/{Max}";
            default: return ToString();
        }
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is BoundedLong other && Equals(other);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(BoundedLong other)
    {
        return Current == other.Current && Max == other.Max;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(BoundedLong other)
    {
        var cmp = Current.CompareTo(other.Current);
        return cmp != 0 ? cmp : Max.CompareTo(other.Max);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(object obj)
    {
        if (obj is BoundedLong other) return CompareTo(other);
        throw new ArgumentException($"Object must be of type {nameof(BoundedLong)}");
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return HashCode.Combine(Current, Max);
    }

    /// <inheritdoc />
    public TypeCode GetTypeCode()
    {
        return TypeCode.Int64;
    }

    /// <inheritdoc />
    bool IConvertible.ToBoolean(IFormatProvider provider)
    {
        return Current != 0;
    }

    /// <inheritdoc />
    byte IConvertible.ToByte(IFormatProvider provider)
    {
        return (byte)Current;
    }

    /// <inheritdoc />
    char IConvertible.ToChar(IFormatProvider provider)
    {
        return (char)Current;
    }

    /// <inheritdoc />
    DateTime IConvertible.ToDateTime(IFormatProvider provider)
    {
        throw new InvalidCastException();
    }

    /// <inheritdoc />
    decimal IConvertible.ToDecimal(IFormatProvider provider)
    {
        return Current;
    }

    /// <inheritdoc />
    double IConvertible.ToDouble(IFormatProvider provider)
    {
        return Current;
    }

    /// <inheritdoc />
    short IConvertible.ToInt16(IFormatProvider provider)
    {
        return (short)Current;
    }

    /// <inheritdoc />
    int IConvertible.ToInt32(IFormatProvider provider)
    {
        return (int)Current;
    }

    /// <inheritdoc />
    long IConvertible.ToInt64(IFormatProvider provider)
    {
        return Current;
    }

    /// <inheritdoc />
    sbyte IConvertible.ToSByte(IFormatProvider provider)
    {
        return (sbyte)Current;
    }

    /// <inheritdoc />
    float IConvertible.ToSingle(IFormatProvider provider)
    {
        return Current;
    }

    /// <inheritdoc />
    string IConvertible.ToString(IFormatProvider provider)
    {
        return ToString("G", provider);
    }

    /// <inheritdoc />
    object IConvertible.ToType(Type conversionType, IFormatProvider provider)
    {
        return Convert.ChangeType(Current, conversionType, provider);
    }

    /// <inheritdoc />
    ushort IConvertible.ToUInt16(IFormatProvider provider)
    {
        return (ushort)Current;
    }

    /// <inheritdoc />
    uint IConvertible.ToUInt32(IFormatProvider provider)
    {
        return (uint)Current;
    }

    /// <inheritdoc />
    ulong IConvertible.ToUInt64(IFormatProvider provider)
    {
        return (ulong)Current;
    }

    /// <summary>Determines whether two bounded longs are equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(BoundedLong left, BoundedLong right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two bounded longs are not equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(BoundedLong left, BoundedLong right)
    {
        return !left.Equals(right);
    }

    /// <summary>Determines whether one bounded long is less than another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(BoundedLong left, BoundedLong right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>Determines whether one bounded long is less than or equal to another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(BoundedLong left, BoundedLong right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>Determines whether one bounded long is greater than another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(BoundedLong left, BoundedLong right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>Determines whether one bounded long is greater than or equal to another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(BoundedLong left, BoundedLong right)
    {
        return left.CompareTo(right) >= 0;
    }

    /// <summary>Increments the current value by 1, clamped to max.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedLong operator ++(BoundedLong a)
    {
        return a + 1;
    }

    /// <summary>Decrements the current value by 1, clamped to 0.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedLong operator --(BoundedLong a)
    {
        return a - 1;
    }

    /// <summary>Adds a value to the bounded long, clamping the result.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedLong operator +(BoundedLong a, long b)
    {
        return new BoundedLong(a.Max, a.Current + b);
    }

    /// <summary>Adds a value to the bounded long, clamping the result.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedLong operator +(long b, BoundedLong a)
    {
        return a + b;
    }

    /// <summary>Subtracts a value from the bounded long, clamping the result.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedLong operator -(BoundedLong a, long b)
    {
        return new BoundedLong(a.Max, a.Current - b);
    }
}