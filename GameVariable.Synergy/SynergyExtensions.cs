using Variable.Experience;

namespace GameVariable.Synergy;

/// <summary>
///     Extensions for simpler usage of SynergyState.
/// </summary>
public static class SynergyExtensions
{
    /// <summary>
    ///     Updates the game state.
    /// </summary>
    public static void Update(ref this SynergyState state, float deltaTime)
    {
        SynergyLogic.Update(ref state, deltaTime);
    }

    /// <summary>
    ///     Adds experience points to the state's experience tracker.
    /// </summary>
    public static void AddExperience(ref this SynergyState state, int amount)
    {
        ExperienceLogic.AddExperience(ref state.Experience.Current, amount);
    }
}
