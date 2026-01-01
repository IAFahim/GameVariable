namespace Variable.Experience;

/// <summary>
///     Provides static logic for handling experience growth, leveling up, and curve calculations.
///     <para><b>Architecture:</b> Pure Primitives. Zero Allocations.</para>
/// </summary>
public static class ExperienceLogic
{
    /// <summary>
    ///     Attempts to apply a level up if the experience is full.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryLevelUp(ref int current, ref int max, ref int level, Func<int, int> nextMaxFormula)
    {
        if (current < max) return false;

        while (current >= max)
        {
            current -= max;
            level++;
            // Calculate new max based on the NEW level
            max = Math.Max(1, nextMaxFormula(level));
        }

        return true;
    }

    /// <summary>
    ///     Attempts to apply a level up if the experience is full (Long version).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryLevelUp(ref long current, ref long max, ref int level, Func<int, long> nextMaxFormula)
    {
        if (current < max) return false;

        while (current >= max)
        {
            current -= max;
            level++;
            max = Math.Max(1, nextMaxFormula(level));
        }

        return true;
    }

    /// <summary>
    ///     Adds experience and processes level ups.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AddExperience(ref int current, ref int max, ref int level, int amount,
        Func<int, int> nextMaxFormula)
    {
        var levelsGained = 0;
        current += amount;
        while (TryLevelUp(ref current, ref max, ref level, nextMaxFormula)) levelsGained++;
        return levelsGained;
    }

    /// <summary>
    ///     Adds experience and processes level ups (Long version).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AddExperience(ref long current, ref long max, ref int level, long amount,
        Func<int, long> nextMaxFormula)
    {
        var levelsGained = 0;
        current += amount;
        while (TryLevelUp(ref current, ref max, ref level, nextMaxFormula)) levelsGained++;
        return levelsGained;
    }
}