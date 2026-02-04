namespace GameVariable.Synergy;

/// <summary>
///     Extension methods for the Synergy system.
///     Bridges the gap between the composite struct (Layer A) and the pure logic (Layer B).
/// </summary>
public static class SynergyExtensions
{
    /// <summary>
    ///     Updates the character's state based on time delta.
    ///     Applies regeneration to Mana and decrements AbilityCooldown.
    /// </summary>
    /// <param name="character">The character to update (passed by reference).</param>
    /// <param name="deltaTime">The time elapsed since last frame.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Tick(ref this SynergyCharacter character, float deltaTime)
    {
        SynergyLogic.Tick(
            deltaTime,
            character.Mana.Value.Current,
            character.Mana.Value.Min,
            character.Mana.Value.Max,
            character.Mana.Rate,
            character.AbilityCooldown.Current,
            out var newMana,
            out var newCooldown
        );

        // Apply updated values back to the struct
        // Since character is ref, and fields are structs, this modifies the memory directly.
        character.Mana.Value.Current = newMana;
        character.AbilityCooldown.Current = newCooldown;
    }
}
