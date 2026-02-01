namespace GameVariable.Synergy;

/// <summary>
///     Extension methods for <see cref="SynergyCharacter" />.
///     Orchestrates updates and interactions between components.
/// </summary>
public static class SynergyExtensions
{
    /// <summary>
    ///     Updates the character's time-dependent state (Regen, Cooldowns).
    /// </summary>
    /// <param name="character">The character to update.</param>
    /// <param name="deltaTime">Time elapsed since last frame.</param>
    public static void Update(ref this SynergyCharacter character, float deltaTime)
    {
        // Tick Mana Regeneration
        character.Mana.Tick(deltaTime);

        // Tick Ability Cooldown
        character.AbilityCooldown.Tick(deltaTime);
    }

    /// <summary>
    ///     Attempts to cast an ability, consuming mana and triggering cooldown if successful.
    /// </summary>
    /// <param name="character">The character casting the ability.</param>
    /// <param name="manaCost">The amount of mana required.</param>
    /// <param name="success">True if the ability was cast successfully.</param>
    public static void TryCastAbility(ref this SynergyCharacter character, float manaCost, out bool success)
    {
        // 1. Validate using Logic Layer (Interoperability Check)
        SynergyLogic.CheckAbilityReadiness(
            in character.Mana.Value.Current,
            in manaCost,
            in character.AbilityCooldown.Current,
            out success
        );

        if (!success) return;

        // 2. Consume Resource
        character.Mana.Value -= manaCost;

        // 3. Reset Cooldown
        character.AbilityCooldown.Reset();
    }
}
