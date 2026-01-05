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
    /// <param name="formula">A struct implementing the formula that calculates Max XP for a level.</param>
    /// <typeparam name="TFormula">Struct type implementing INextMaxFormula.</typeparam>
    /// <returns>The number of levels gained (0 if no level up occurred).</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Add<TFormula>(ref this ExperienceInt xp, int amount, TFormula formula)
        where TFormula : struct, INextMaxFormula<int>
    {
        ExperienceLogic.AddExperience(ref xp.Current, amount);
        var levelsGained = 0;
        while (xp.Current >= xp.Max)
        {
            var newMax = Math.Max(1, formula.Calculate(xp.Level + 1));
            if (ExperienceLogic.TryApplyLevelUp(ref xp.Current, ref xp.Max, ref xp.Level, newMax))
                levelsGained++;
            else
                break;
        }

        return levelsGained;
    }

    /// <summary>
    ///     Adds experience and returns the number of levels gained.
    /// </summary>
    /// <param name="xp">The experience struct to modify.</param>
    /// <param name="amount">The amount of experience to add.</param>
    /// <param name="formula">A struct implementing the formula that calculates Max XP for a level.</param>
    /// <typeparam name="TFormula">Struct type implementing INextMaxFormula.</typeparam>
    /// <returns>The number of levels gained (0 if no level up occurred).</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Add<TFormula>(ref this ExperienceLong xp, long amount, TFormula formula)
        where TFormula : struct, INextMaxFormula<long>
    {
        ExperienceLogic.AddExperience(ref xp.Current, amount);
        var levelsGained = 0;
        while (xp.Current >= xp.Max)
        {
            var newMax = Math.Max(1, formula.Calculate(xp.Level + 1));
            if (ExperienceLogic.TryApplyLevelUp(ref xp.Current, ref xp.Max, ref xp.Level, newMax))
                levelsGained++;
            else
                break;
        }

        return levelsGained;
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