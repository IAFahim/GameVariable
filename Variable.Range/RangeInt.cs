namespace Variable.Range;

/// <summary>
///     An integer value constrained within a configurable minimum and maximum range.
///     Ideal for levels, ranks, dice rolls, and other discrete ranged values.
/// </summary>
/// <remarks>
///     <para>Unlike <c>BoundedInt</c>, this type supports arbitrary min/max ranges.</para>
///     <para>The value is automatically clamped on construction and arithmetic operations.</para>
///     <para>This struct is blittable and can be used in Unity ECS and Burst jobs.</para>
/// </remarks>
/// <example>
///     <code>
/// // Dice roll: 1 to 6
/// var die = new RangeInt(1, 6, 3);
/// 
/// // Character level: 1 to 99
/// var level = new RangeInt(1, 99, 1);
/// </code>
/// </example>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{Current} [{Min}, {Max}]")]
public struct RangeInt :
    IBoundedInfo,
    IEquatable<RangeInt>,
    IComparable<RangeInt>,
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

    /// <summary>
    ///     Creates a new range int with specified bounds and current value.
    /// </summary>
    /// <param name="min">The minimum allowed value.</param>
    /// <param name="max">The maximum allowed value.</param>
    /// <param name="current">The initial current value. Will be clamped to [min, max].</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RangeInt(int min, int max, int current)
    {
        Min = min;
        Max = max;
        Current = current > max ? max : current < min ? min : current;
    }

    /// <summary>
    ///     Creates a new range int with current set to min.
    /// </summary>
    /// <param name="min">The minimum allowed value.</param>
    /// <param name="max">The maximum allowed value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RangeInt(int min, int max) : this(min, max, min)
    {
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
    ///     Deconstructs the range value into its components.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out int current, out int min, out int max)
    {
        current = Current;
        min = Min;
        max = Max;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double GetRatio()
    {
        return Max == Min ? 0.0 : (double)(Current - Min) / (Max - Min);
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
        return Current == Min;
    }

    /// <summary>Implicitly converts the range int to its current value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator int(RangeInt value)
    {
        return value.Current;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Current} [{Min}, {Max}]";
    }

    /// <inheritdoc />
    public string ToString(string format, IFormatProvider formatProvider)
    {
        if (string.IsNullOrEmpty(format)) format = "G";

        switch (format.ToUpperInvariant())
        {
            case "R": return GetRatio().ToString("P", formatProvider);
            case "C": return $"{Current} [{Min}, {Max}]";
            default: return ToString();
        }
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is RangeInt other && Equals(other);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(RangeInt other)
    {
        return Current == other.Current && Min == other.Min && Max == other.Max;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(RangeInt other)
    {
        var cmp = Current.CompareTo(other.Current);
        if (cmp != 0) return cmp;
        cmp = Min.CompareTo(other.Min);
        return cmp != 0 ? cmp : Max.CompareTo(other.Max);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(object obj)
    {
        if (obj is RangeInt other) return CompareTo(other);
        throw new ArgumentException($"Object must be of type {nameof(RangeInt)}");
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return HashCode.Combine(Current, Min, Max);
    }

    /// <inheritdoc />
    public TypeCode GetTypeCode()
    {
        return TypeCode.Int32;
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
        return Current;
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

    /// <summary>Determines whether two range ints are equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(RangeInt left, RangeInt right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two range ints are not equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(RangeInt left, RangeInt right)
    {
        return !left.Equals(right);
    }

    /// <summary>Determines whether one range int is less than another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(RangeInt left, RangeInt right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>Determines whether one range int is less than or equal to another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(RangeInt left, RangeInt right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>Determines whether one range int is greater than another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(RangeInt left, RangeInt right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>Determines whether one range int is greater than or equal to another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(RangeInt left, RangeInt right)
    {
        return left.CompareTo(right) >= 0;
    }

    /// <summary>Increments the current value by 1, clamped to max.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeInt operator ++(RangeInt a)
    {
        return a + 1;
    }

    /// <summary>Decrements the current value by 1, clamped to min.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeInt operator --(RangeInt a)
    {
        return a - 1;
    }

    /// <summary>Adds a value to the range int, clamping the result. Uses long internally to prevent overflow.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeInt operator +(RangeInt a, int b)
    {
        var res = (long)a.Current + b;
        if (res > a.Max) res = a.Max;
        else if (res < a.Min) res = a.Min;
        return new RangeInt(a.Min, a.Max, (int)res);
    }

    /// <summary>Adds a value to the range int, clamping the result.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeInt operator +(int b, RangeInt a)
    {
        return a + b;
    }

    /// <summary>Subtracts a value from the range int, clamping the result.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeInt operator -(RangeInt a, int b)
    {
        return a + -b;
    }
}