namespace Variable.Experience;

/// <summary>
///     Tracks experience points (XP) using long integers.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("Lvl {Level} ({Current}/{Max})")]
public struct ExperienceLong :
    IBoundedInfo,
    IEquatable<ExperienceLong>,
    IComparable<ExperienceLong>,
    IComparable,
    IFormattable,
    IConvertible
{
    /// <summary>The current experience points.</summary>
    public long Current;

    /// <summary>The experience required for the next level.</summary>
    public long Max;

    /// <summary>The current level.</summary>
    public int Level;

    /// <summary>
    ///     Creates a new experience tracker.
    /// </summary>
    /// <param name="max">The XP required to level up.</param>
    /// <param name="current">The current XP. Defaults to 0.</param>
    /// <param name="level">The current level. Defaults to 1.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ExperienceLong(long max, long current = 0, int level = 1)
    {
        Max = max < 1 ? 1 : max;
        Current = current;
        Level = level;
    }

    /// <summary>
    ///     Deconstructs the experience into its components.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out long current, out long max, out int level)
    {
        current = Current;
        max = Max;
        level = Level;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double GetRatio()
    {
        return Max == 0 ? 0.0 : (double)Current / Max;
    }

    /// <summary>
    ///     Returns true when current XP meets or exceeds the required max.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsFull()
    {
        return Current >= Max;
    }

    /// <summary>
    ///     Returns true when current XP is zero.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsEmpty()
    {
        return Current == 0;
    }

    /// <summary>Implicitly converts the experience to its current XP value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator long(ExperienceLong value)
    {
        return value.Current;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Lvl {Level} ({Current}/{Max})";
    }

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (string.IsNullOrEmpty(format)) format = "G";

        switch (format.ToUpperInvariant())
        {
            case "R": return GetRatio().ToString("P", formatProvider);
            case "L": return Level.ToString(formatProvider);
            case "C": return $"{Current}/{Max}";
            default: return ToString();
        }
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is ExperienceLong other && Equals(other);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(ExperienceLong other)
    {
        return Current == other.Current && Max == other.Max && Level == other.Level;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(ExperienceLong other)
    {
        var cmp = Level.CompareTo(other.Level);
        if (cmp != 0) return cmp;
        var cmpMax = Max.CompareTo(other.Max);
        if (cmpMax != 0) return cmpMax;
        return Current.CompareTo(other.Current);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(object obj)
    {
        if (obj is ExperienceLong other) return CompareTo(other);
        throw new ArgumentException($"Object must be of type {nameof(ExperienceLong)}");
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return HashCode.Combine(Current, Max, Level);
    }

    /// <inheritdoc />
    public TypeCode GetTypeCode()
    {
        return TypeCode.Object;
    }

    bool IConvertible.ToBoolean(IFormatProvider provider)
    {
        return Current != 0;
    }

    byte IConvertible.ToByte(IFormatProvider provider)
    {
        return (byte)Current;
    }

    char IConvertible.ToChar(IFormatProvider provider)
    {
        return (char)Current;
    }

    DateTime IConvertible.ToDateTime(IFormatProvider provider)
    {
        throw new InvalidCastException();
    }

    decimal IConvertible.ToDecimal(IFormatProvider provider)
    {
        return Current;
    }

    double IConvertible.ToDouble(IFormatProvider provider)
    {
        return Current;
    }

    short IConvertible.ToInt16(IFormatProvider provider)
    {
        return (short)Current;
    }

    int IConvertible.ToInt32(IFormatProvider provider)
    {
        return (int)Current;
    }

    long IConvertible.ToInt64(IFormatProvider provider)
    {
        return Current;
    }

    sbyte IConvertible.ToSByte(IFormatProvider provider)
    {
        return (sbyte)Current;
    }

    float IConvertible.ToSingle(IFormatProvider provider)
    {
        return Current;
    }

    string IConvertible.ToString(IFormatProvider provider)
    {
        return ToString("G", provider);
    }

    object IConvertible.ToType(Type conversionType, IFormatProvider provider)
    {
        throw new InvalidCastException();
    }

    ushort IConvertible.ToUInt16(IFormatProvider provider)
    {
        return (ushort)Current;
    }

    uint IConvertible.ToUInt32(IFormatProvider provider)
    {
        return (uint)Current;
    }

    ulong IConvertible.ToUInt64(IFormatProvider provider)
    {
        return (ulong)Current;
    }

    /// <summary>Determines whether two experience values are equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(ExperienceLong left, ExperienceLong right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two experience values are not equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(ExperienceLong left, ExperienceLong right)
    {
        return !left.Equals(right);
    }

    /// <summary>Determines whether one experience is less than another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(ExperienceLong left, ExperienceLong right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>Determines whether one experience is less than or equal to another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(ExperienceLong left, ExperienceLong right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>Determines whether one experience is greater than another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(ExperienceLong left, ExperienceLong right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>Determines whether one experience is greater than or equal to another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(ExperienceLong left, ExperienceLong right)
    {
        return left.CompareTo(right) >= 0;
    }

    /// <summary>Adds XP to the experience. Does not auto-level; check IsFull() and handle leveling.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ExperienceLong operator +(ExperienceLong a, long amount)
    {
        return new ExperienceLong(a.Max, a.Current + amount, a.Level);
    }
}