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

    /// <summary>
    ///     Deconstructs the experience into its components.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Deconstruct(out int current, out int max, out int level)
    {
        current = Current;
        max = Max;
        level = Level;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        Span<char> destination = stackalloc char[48];
        return TryFormat(destination, out var charsWritten)
            ? new string(destination.Slice(0, charsWritten))
            : string.Format(CultureInfo.InvariantCulture, "Lvl {0} ({1}/{2})", Level, Current, Max);
    }

    /// <summary>
    ///     Formats the experience with custom format strings.
    /// </summary>
    /// <param name="format">
    ///     Format code: "L" = Level only, "C" = Current/Max only, null = default format.
    /// </param>
    /// <param name="formatProvider">Unused, for IFormattable compatibility.</param>
    /// <returns>Formatted string.</returns>
    public string ToString(string format, IFormatProvider formatProvider)
    {
        if (string.IsNullOrEmpty(format)) return ToString();

        Span<char> destination = stackalloc char[32];
        if (TryFormat(destination, out var charsWritten, format.AsSpan()))
        {
            return new string(destination.Slice(0, charsWritten));
        }

        return format.ToUpperInvariant() switch
        {
            "L" => Level.ToString(CultureInfo.InvariantCulture),
            "C" => string.Format(CultureInfo.InvariantCulture, "{0}/{1}", Current, Max),
            _ => ToString()
        };
    }

    /// <summary>
    ///     Formats the experience into a character span (zero allocation).
    /// </summary>
    /// <param name="destination">The span to write the formatted value into.</param>
    /// <param name="charsWritten">The number of characters written.</param>
    /// <param name="format">Optional format string. "L" = Level, "C" = Current/Max.</param>
    /// <returns>True if formatting succeeded; false if destination was too small.</returns>
    public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default)
    {
        charsWritten = 0;

        if (format.Length > 0 && (format[0] == 'L' || format[0] == 'l'))
        {
            return Level.TryFormat(destination, out charsWritten, default, CultureInfo.InvariantCulture);
        }

        if (format.Length > 0 && (format[0] == 'C' || format[0] == 'c'))
        {
            if (!Current.TryFormat(destination, out var currentWritten, default, CultureInfo.InvariantCulture))
                return false;
            charsWritten = currentWritten;

            if (charsWritten >= destination.Length) return false;
            destination[charsWritten++] = '/';

            if (!Max.TryFormat(destination.Slice(charsWritten), out var maxWritten, default, CultureInfo.InvariantCulture))
                return false;
            charsWritten += maxWritten;
            return true;
        }

        // Default format: "Lvl {Level} ({Current}/{Max})"
        const string lvl = "Lvl ";
        if (!lvl.AsSpan().TryCopyTo(destination)) return false;
        charsWritten = lvl.Length;

        if (!Level.TryFormat(destination.Slice(charsWritten), out var levelWritten, default, CultureInfo.InvariantCulture))
            return false;
        charsWritten += levelWritten;

        const string open = " (";
        if (charsWritten + open.Length > destination.Length) return false;
        open.AsSpan().CopyTo(destination.Slice(charsWritten));
        charsWritten += open.Length;

        if (!Current.TryFormat(destination.Slice(charsWritten), out var curWritten, default, CultureInfo.InvariantCulture))
            return false;
        charsWritten += curWritten;

        if (charsWritten >= destination.Length) return false;
        destination[charsWritten++] = '/';

        if (!Max.TryFormat(destination.Slice(charsWritten), out var mWritten, default, CultureInfo.InvariantCulture))
            return false;
        charsWritten += mWritten;

        if (charsWritten >= destination.Length) return false;
        destination[charsWritten++] = ')';

        return true;
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