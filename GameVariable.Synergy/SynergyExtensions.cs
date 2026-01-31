namespace GameVariable.Synergy;

/// <summary>
///     Extension methods that act as the conductor (Layer C) for synergy components.
/// </summary>
public static class SynergyExtensions
{
    /// <summary>
    ///     Updates the character state, applying synergistic effects (like Rage Mode).
    /// </summary>
    /// <param name="me">The character to update.</param>
    /// <param name="dt">The delta time in seconds.</param>
    public static void Update(ref this SynergyCharacter me, float dt)
    {
        // 1. Calculate Multiplier (Layer B)
        SynergyLogic.CalculateRageMultiplier(
            in me.Health.Current,
            in me.Health.Max,
            out float multiplier
        );

        // 2. Apply to Stamina (Integration)
        // Effectively multiplies the regen rate by passing scaled time.
        me.Stamina.Tick(dt * multiplier);
    }
}
