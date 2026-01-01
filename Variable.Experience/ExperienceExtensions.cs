namespace Variable.Experience;

/// <summary>
///     Extension methods for ExperienceInt and ExperienceLong.
/// </summary>
public static class ExperienceExtensions
{
    /// <summary>
    ///     Adds experience and returns the number of levels gained.
    /// </summary>
    /// <param name="xp">The experience struct to modify.</param>
    /// <param name="amount">The amount of experience to add.</param>
    /// <param name="nextMaxFormula">A function that takes the NEW level and returns the Max XP required for that level.</param>
    /// <returns>The number of levels gained (0 if no level up occurred).</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Add(ref this ExperienceInt xp, int amount, Func<int, int> nextMaxFormula)
    {
        return ExperienceLogic.AddExperience(
            ref xp.Current,
            ref xp.Max,
            ref xp.Level,
            amount,
            nextMaxFormula
        );
    }

    /// <summary>
    ///     Adds experience and returns the number of levels gained.
    /// </summary>
    /// <param name="xp">The experience struct to modify.</param>
    /// <param name="amount">The amount of experience to add.</param>
    /// <param name="nextMaxFormula">A function that takes the NEW level and returns the Max XP required for that level.</param>
    /// <returns>The number of levels gained (0 if no level up occurred).</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Add(ref this ExperienceLong xp, long amount, Func<int, long> nextMaxFormula)
    {
        return ExperienceLogic.AddExperience(
            ref xp.Current,
            ref xp.Max,
            ref xp.Level,
            amount,
            nextMaxFormula
        );
    }

    /// <summary>
    ///     Returns true when current XP meets or exceeds the required max (ready to level up).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(this ExperienceInt xp)
    {
        return xp.Current >= xp.Max;
    }

    /// <summary>
    ///     Returns true when current XP is zero.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(this ExperienceInt xp)
    {
        return xp.Current == 0;
    }

    /// <summary>
    ///     Gets the normalized ratio of current XP to max.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetRatio(this ExperienceInt xp)
    {
        return xp.Max == 0 ? 0.0 : (double)xp.Current / xp.Max;
    }

    /// <summary>
    ///     Returns true when current XP meets or exceeds the required max.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(this ExperienceLong xp)
    {
        return xp.Current >= xp.Max;
    }

    /// <summary>
    ///     Returns true when current XP is zero.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(this ExperienceLong xp)
    {
        return xp.Current == 0;
    }

    /// <summary>
    ///     Gets the normalized ratio of current XP to max.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetRatio(this ExperienceLong xp)
    {
        return xp.Max == 0 ? 0.0 : (double)xp.Current / xp.Max;
    }
}
