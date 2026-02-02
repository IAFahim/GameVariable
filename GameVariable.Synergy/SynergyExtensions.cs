namespace GameVariable.Synergy;

/// <summary>
/// Layer C: The Adapter.
/// Extensions to manipulate SynergyCharacter using pure logic.
/// </summary>
public static class SynergyExtensions
{
    /// <summary>
    /// Updates the character's state over time (Regen, Cooldowns).
    /// </summary>
    /// <param name="character">The character to update.</param>
    /// <param name="deltaTime">Time elapsed since last frame.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Tick(ref this SynergyCharacter character, float deltaTime)
    {
        character.Mana.Tick(deltaTime);
        character.AbilityCooldown.Tick(deltaTime);
    }

    /// <summary>
    /// Attempts to cast an ability, consuming mana and setting cooldown if successful.
    /// </summary>
    /// <param name="character">The character casting the ability.</param>
    /// <param name="cost">The mana cost of the ability.</param>
    /// <returns>True if the ability was cast successfully; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryCastAbility(ref this SynergyCharacter character, float cost)
    {
        SynergyLogic.CanCastAbility(
            in character.Mana.Value.Current,
            in character.AbilityCooldown.Current,
            in cost,
            out bool canCast
        );

        if (canCast)
        {
            SynergyLogic.CalculatePostCastState(
                in character.Mana.Value.Current,
                in cost,
                in character.AbilityCooldown.Duration,
                out float newMana,
                out float newCooldown
            );

            // Apply new state
            character.Mana.Value.Current = newMana;
            character.AbilityCooldown.Current = newCooldown;
        }

        return canCast;
    }
}
