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
    IComparable
{
    /// <summary>The current experience points.</summary>
    public long Current;

    /// <summary>The experience required for the next level.</summary>
    public long Max;

    /// <summary>The current level.</summary>
    public int Level;

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

    /// <summary>Implicitly converts the experience to its current XP value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator long(ExperienceLong value)
    {
        return value.Current;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return string.Format(CultureInfo.InvariantCulture, "Lvl {0} ({1}/{2})", Level, Current, Max);
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