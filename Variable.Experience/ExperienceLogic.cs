namespace Variable.Experience;

/// <summary>
///     Provides static logic for handling experience growth, leveling up, and curve calculations.
///     <para><b>Architecture:</b> Pure Primitives. Zero Allocations.</para>
/// </summary>
public static class ExperienceLogic
{
    /// <summary>
    ///     Attempts to apply a single level up if the experience is full.
    ///     Requires the caller to provide the new max experience for the next level.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryApplyLevelUp(ref int current, ref int max, ref int level, int newMax)
    {
        if (current < max) return false;

        current -= max;
        level++;
        max = newMax;
        return true;
    }

    /// <summary>
    ///     Attempts to apply a single level up if the experience is full (Long version).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryApplyLevelUp(ref long current, ref long max, ref int level, long newMax)
    {
        if (current < max) return false;

        current -= max;
        level++;
        max = newMax;
        return true;
    }

    /// <summary>
    ///     Adds experience. Does NOT handle leveling up.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddExperience(ref int current, int amount)
    {
        current += amount;
    }

    /// <summary>
    ///     Adds experience. Does NOT handle leveling up.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddExperience(ref long current, long amount)
    {
        current += amount;
    }
}