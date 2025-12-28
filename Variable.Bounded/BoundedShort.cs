namespace Variable.Bounded;

/// <summary>
///     A bounded short value that is clamped between 0 and a maximum.
///     Ideal for compact storage when value ranges are small (up to 32,767).
/// </summary>
/// <remarks>
///     <para>The value is automatically clamped on construction and arithmetic operations.</para>
///     <para>This struct is blittable and can be used in Unity ECS, Burst jobs, and network serialization.</para>
/// </remarks>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{Current}/{Max}")]
public struct BoundedShort :
    IBoundedInfo,
    IEquatable<BoundedShort>,
    IComparable<BoundedShort>,
    IComparable,
    IFormattable,
    IConvertible
{
    /// <summary>The current value, always clamped between 0 and <see cref="Max" />.</summary>
    public short Current;

    /// <summary>The maximum allowed value (ceiling).</summary>
    public short Max;

    /// <summary>
    ///     Creates a new bounded short with the specified max and current value.
    /// </summary>
    /// <param name="max">The maximum allowed value.</param>
    /// <param name="current">The initial current value. Will be clamped to [0, max].</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BoundedShort(short max, short current)
    {
        Max = max;
        Current = current > max ? max : current < 0 ? (short)0 : current;
    }

    /// <summary>
    ///     Creates a new bounded short with current set to max.
    /// </summary>
    /// <param name="max">The maximum allowed value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BoundedShort(short max) : this(max, max)
    {
    }

    /// <summary>
    ///     Clamps the current value to the valid range [0, Max].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Normalize()
    {
        Current = Current > Max ? Max : Current < 0 ? (short)0 : Current;
    }

    /// <summary>
    ///     Deconstructs the bounded value into its components.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out short current, out short max)
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

    /// <summary>Implicitly converts the bounded short to its current value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator short(BoundedShort value)
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
        return obj is BoundedShort other && Equals(other);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(BoundedShort other)
    {
        return Current == other.Current && Max == other.Max;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(BoundedShort other)
    {
        var cmp = Current.CompareTo(other.Current);
        return cmp != 0 ? cmp : Max.CompareTo(other.Max);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(object obj)
    {
        if (obj is BoundedShort other) return CompareTo(other);
        throw new ArgumentException($"Object must be of type {nameof(BoundedShort)}");
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
        return TypeCode.Int16;
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
        return Current;
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

    /// <summary>Determines whether two bounded shorts are equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(BoundedShort left, BoundedShort right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two bounded shorts are not equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(BoundedShort left, BoundedShort right)
    {
        return !left.Equals(right);
    }

    /// <summary>Determines whether one bounded short is less than another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(BoundedShort left, BoundedShort right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>Determines whether one bounded short is less than or equal to another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(BoundedShort left, BoundedShort right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>Determines whether one bounded short is greater than another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(BoundedShort left, BoundedShort right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>Determines whether one bounded short is greater than or equal to another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(BoundedShort left, BoundedShort right)
    {
        return left.CompareTo(right) >= 0;
    }

    /// <summary>Increments the current value by 1, clamped to max.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedShort operator ++(BoundedShort a)
    {
        return a + 1;
    }

    /// <summary>Decrements the current value by 1, clamped to 0.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedShort operator --(BoundedShort a)
    {
        return a - 1;
    }

    /// <summary>Adds a value to the bounded short, clamping the result.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedShort operator +(BoundedShort a, int b)
    {
        var res = a.Current + b;
        if (res > a.Max) res = a.Max;
        else if (res < 0) res = 0;
        return new BoundedShort(a.Max, (short)res);
    }

    /// <summary>Adds a value to the bounded short, clamping the result.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedShort operator +(int b, BoundedShort a)
    {
        return a + b;
    }

    /// <summary>Subtracts a value from the bounded short, clamping the result.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundedShort operator -(BoundedShort a, int b)
    {
        return a + -b;
    }
}