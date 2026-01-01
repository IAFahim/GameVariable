namespace Variable.Experience;

/// <summary>
///     Tracks experience points (XP), current level, and XP required for next level.
///     Ideal for RPG progression, skill leveling, and achievement systems.
/// </summary>
/// <remarks>
///     <para>Level-up logic is intentionally not built-in. Use extension methods for custom curves.</para>
///     <para>This allows different XP formulas (linear, exponential, curve-based) per game.</para>
///     <para>This struct is blittable and can be used in Unity ECS and Burst jobs.</para>
/// </remarks>
/// <example>
///     <code>
/// // Start at level 1 with 0/1000 XP
/// var xp = new ExperienceInt(1000);
/// xp = xp + 500; // Gain XP
/// if (xp.IsFull()) LevelUp(ref xp);
/// </code>
/// </example>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("Lvl {Level} ({Current}/{Max})")]
public struct ExperienceInt :
    IBoundedInfo,
    IEquatable<ExperienceInt>,
    IComparable<ExperienceInt>,
    IComparable
{
    /// <summary>The current experience points within the current level.</summary>
    public int Current;

    /// <summary>The experience required to reach the next level.</summary>
    public int Max;

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
    public ExperienceInt(int max, int current = 0, int level = 1)
    {
        Max = max < 1 ? 1 : max;
        Current = current;
        Level = level;
    }

    /// <summary>Implicitly converts the experience to its current XP value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator int(ExperienceInt value)
    {
        return value.Current;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Lvl {Level} ({Current}/{Max})";
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is ExperienceInt other && Equals(other);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(ExperienceInt other)
    {
        return Current == other.Current && Max == other.Max && Level == other.Level;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(ExperienceInt other)
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
        if (obj is ExperienceInt other) return CompareTo(other);
        throw new ArgumentException($"Object must be of type {nameof(ExperienceInt)}");
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return HashCode.Combine(Current, Max, Level);
    }

    /// <summary>Determines whether two experience values are equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(ExperienceInt left, ExperienceInt right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two experience values are not equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(ExperienceInt left, ExperienceInt right)
    {
        return !left.Equals(right);
    }

    /// <summary>Determines whether one experience is less than another (compares level, then XP).</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(ExperienceInt left, ExperienceInt right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>Determines whether one experience is less than or equal to another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(ExperienceInt left, ExperienceInt right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>Determines whether one experience is greater than another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(ExperienceInt left, ExperienceInt right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>Determines whether one experience is greater than or equal to another.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(ExperienceInt left, ExperienceInt right)
    {
        return left.CompareTo(right) >= 0;
    }

    /// <summary>Adds XP to the experience. Does not auto-level; check IsFull() and handle leveling.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ExperienceInt operator +(ExperienceInt a, int amount)
    {
        return new ExperienceInt(a.Max, a.Current + amount, a.Level);
    }
}