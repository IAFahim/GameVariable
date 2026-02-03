namespace GameVariable.Synergy;

/// <summary>
///     Extension methods for <see cref="SynergyCharacter" />.
///     Acts as the Adapter Layer (Layer C), bridging the struct to the pure logic.
/// </summary>
public static class SynergyExtensions
{
    /// <summary>
    ///     Updates the character's state (Mana regen, Cooldowns) by the specified time delta.
    /// </summary>
    /// <param name="character">The character to update.</param>
    /// <param name="deltaTime">Time elapsed since last frame.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Tick(ref this SynergyCharacter character, float deltaTime)
    {
        SynergyLogic.Tick(
            in deltaTime,
            in character.Mana.Rate,
            in character.Mana.Value.Max,
            ref character.Mana.Value.Current,
            ref character.AbilityCooldown.Current
        );
    }

    /// <summary>
    ///     Attempts to cast the ability, checking and consuming resources.
    /// </summary>
    /// <param name="character">The character attempting to cast.</param>
    /// <param name="manaCost">The mana cost of the ability.</param>
    /// <returns>True if the cast was successful (resources consumed), false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryCastAbility(ref this SynergyCharacter character, float manaCost)
    {
        SynergyLogic.TryCast(
            in manaCost,
            ref character.Mana.Value.Current,
            ref character.AbilityCooldown.Current,
            in character.AbilityCooldown.Duration,
            out var success
        );
        return success;
    }
}
