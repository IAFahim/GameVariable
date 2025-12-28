namespace Variable.Range;

/// <summary>
///     A float value constrained within a configurable minimum and maximum range.
///     Ideal for temperature, percentages, volume sliders, and other ranged values.
/// </summary>
/// <remarks>
///     <para>Unlike <c>BoundedFloat</c>, this type supports arbitrary min/max ranges.</para>
///     <para>The value is automatically clamped on construction and arithmetic operations.</para>
///     <para>This struct is blittable and can be used in Unity ECS and Burst jobs.</para>
/// </remarks>
/// <example>
///     <code>
/// // Temperature: -50°C to 50°C
/// var temp = new RangeFloat(-50f, 50f, 22f);
/// 
/// // Volume slider: 0.0 to 1.0
/// var volume = new RangeFloat(0f, 1f, 0.75f);
/// </code>
/// </example>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{Current} [{Min}, {Max}]")]
public struct RangeFloat :
    IBoundedInfo,
    IEquatable<RangeFloat>,
    IComparable<RangeFloat>,
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

    /// <summary>
    ///     Creates a new range float with specified bounds and current value.
    /// </summary>
    /// <param name="min">The minimum allowed value.</param>
    /// <param name="max">The maximum allowed value.</param>
    /// <param name="current">The initial current value. Will be clamped to [min, max].</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RangeFloat(float min, float max, float current)
    {
        Min = min;
        Max = max;
        Current = current > max ? max : current < min ? min : current;
    }

    /// <summary>
    ///     Creates a new range float with current set to min.
    /// </summary>
    /// <param name="min">The minimum allowed value.</param>
    /// <param name="max">The maximum allowed value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RangeFloat(float min, float max) : this(min, max, min)
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
    public void Deconstruct(out float current, out float min, out float max)
    {
        current = Current;
        min = Min;
        max = Max;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double GetRatio()
    {
        return Math.Abs(Max - Min) < float.Epsilon ? 0.0 : (Current - Min) / (Max - Min);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsFull()
    {
        return Math.Abs(Current - Max) < float.Epsilon;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsEmpty()
    {
        return Math.Abs(Current - Min) < float.Epsilon;
    }

    /// <summary>Implicitly converts the range float to its current value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator float(RangeFloat value)
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
            default:
                return
                    $"{Current.ToString(format, formatProvider)} [{Min.ToString(format, formatProvider)}, {Max.ToString(format, formatProvider)}]";
        }
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is RangeFloat other && Equals(other);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(RangeFloat other)
    {
        return Current.Equals(other.Current) && Min.Equals(other.Min) && Max.Equals(other.Max);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(RangeFloat other)
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
        if (obj is RangeFloat other) return CompareTo(other);
        throw new ArgumentException($"Object must be of type {nameof(RangeFloat)}");
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
        return TypeCode.Single;
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
        return (decimal)Current;
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
        return (long)Current;
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

    /// <summary>Determines whether two range floats are equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(RangeFloat left, RangeFloat right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two range floats are not equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(RangeFloat left, RangeFloat right)
    {
        return !left.Equals(right);
    }

    /// <summary>Determines whether one range float is less than another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(RangeFloat left, RangeFloat right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>Determines whether one range float is less than or equal to another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(RangeFloat left, RangeFloat right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>Determines whether one range float is greater than another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(RangeFloat left, RangeFloat right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>Determines whether one range float is greater than or equal to another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(RangeFloat left, RangeFloat right)
    {
        return left.CompareTo(right) >= 0;
    }

    /// <summary>Increments the current value by 1, clamped to max.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeFloat operator ++(RangeFloat a)
    {
        return a + 1f;
    }

    /// <summary>Decrements the current value by 1, clamped to min.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeFloat operator --(RangeFloat a)
    {
        return a - 1f;
    }

    /// <summary>Adds a value to the range float, clamping the result.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeFloat operator +(RangeFloat a, float b)
    {
        return new RangeFloat(a.Min, a.Max, a.Current + b);
    }

    /// <summary>Adds a value to the range float, clamping the result.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeFloat operator +(float b, RangeFloat a)
    {
        return a + b;
    }

    /// <summary>Subtracts a value from the range float, clamping the result.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeFloat operator -(RangeFloat a, float b)
    {
        return new RangeFloat(a.Min, a.Max, a.Current - b);
    }
}