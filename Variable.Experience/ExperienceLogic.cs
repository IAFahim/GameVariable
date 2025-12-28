namespace Variable.Experience;

/// <summary>
///     Provides static logic for handling experience growth, leveling up, and curve calculations.
/// </summary>
public static class ExperienceLogic
{
    /// <summary>
    ///     Attempts to apply a level up if the experience is full.
    /// </summary>
    /// <param name="xp">The experience structure to modify.</param>
    /// <param name="nextMaxFormula">A function that takes the NEW level and returns the Max XP required for that level.</param>
    /// <returns>True if a level up occurred, false otherwise.</returns>
    public static bool TryLevelUp(ref ExperienceInt xp, Func<int, int> nextMaxFormula)
    {
        if (xp.Current < xp.Max) return false;

        while (xp.Current >= xp.Max)
        {
            xp.Current -= xp.Max;
            xp.Level++;
            xp.Max = Math.Max(1, nextMaxFormula(xp.Level));
        }

        return true;
    }

    /// <summary>
    ///     Attempts to apply a level up if the experience is full.
    /// </summary>
    /// <param name="xp">The experience structure to modify.</param>
    /// <param name="nextMaxFormula">A function that takes the NEW level and returns the Max XP required for that level.</param>
    /// <returns>True if a level up occurred, false otherwise.</returns>
    public static bool TryLevelUp(ref ExperienceLong xp, Func<int, long> nextMaxFormula)
    {
        if (xp.Current < xp.Max) return false;

        while (xp.Current >= xp.Max)
        {
            xp.Current -= xp.Max;
            xp.Level++;
            xp.Max = Math.Max(1, nextMaxFormula(xp.Level));
        }

        return true;
    }

    /// <summary>
    ///     Adds experience and automatically processes level ups based on the provided formula.
    /// </summary>
    public static int AddExperience(ref ExperienceInt xp, int amount, Func<int, int> nextMaxFormula)
    {
        var levelsGained = 0;
        xp.Current += amount;
        while (TryLevelUp(ref xp, nextMaxFormula)) levelsGained++;
        return levelsGained;
    }

    /// <summary>
    ///     Adds experience and automatically processes level ups based on the provided formula.
    /// </summary>
    public static int AddExperience(ref ExperienceLong xp, long amount, Func<int, long> nextMaxFormula)
    {
        var levelsGained = 0;
        xp.Current += amount;
        while (TryLevelUp(ref xp, nextMaxFormula)) levelsGained++;
        return levelsGained;
    }
}