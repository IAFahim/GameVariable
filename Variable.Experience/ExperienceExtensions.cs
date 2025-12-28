namespace Variable.Experience;

public static class ExperienceExtensions
{
    /// <summary>
    ///     Adds experience and returns true if at least one level up occurred.
    /// </summary>
    public static bool Add(ref this ExperienceInt xp, int amount, Func<int, int> nextMaxFormula)
    {
        return ExperienceLogic.AddExperience(ref xp, amount, nextMaxFormula) > 0;
    }

    /// <summary>
    ///     Adds experience and returns true if at least one level up occurred.
    /// </summary>
    public static bool Add(ref this ExperienceLong xp, long amount, Func<int, long> nextMaxFormula)
    {
        return ExperienceLogic.AddExperience(ref xp, amount, nextMaxFormula) > 0;
    }
}