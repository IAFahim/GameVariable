using System.Runtime.CompilerServices;
using Variable.Regen;
using Variable.Timer;

namespace GameVariable.Synergy;

/// <summary>
///     Extension methods for <see cref="SynergyCharacter"/>.
///     Serves as the Adapter Layer (Layer C) to orchestrate logic.
/// </summary>
public static class SynergyExtensions
{
    /// <summary>
    ///     Updates the character's state (Mana regen, Cooldowns).
    /// </summary>
    /// <param name="character">The character to update.</param>
    /// <param name="deltaTime">Time elapsed since last frame.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Update(ref this SynergyCharacter character, float deltaTime)
    {
        // Delegate to existing extensions
        character.Mana.Tick(deltaTime);
        character.FireballCd.Tick(deltaTime);
    }

    /// <summary>
    ///     Attempts to cast a fireball.
    ///     Checks if mana is sufficient and cooldown is ready.
    ///     If successful, consumes mana and resets cooldown.
    /// </summary>
    /// <param name="character">The character casting the spell.</param>
    /// <param name="manaCost">The mana cost of the fireball. Default is 20.</param>
    /// <returns>True if the spell was cast; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryCastFireball(ref this SynergyCharacter character, float manaCost = 20f)
    {
        // 1. Decompose struct to primitives (Layer A -> Layer B)
        SynergyLogic.CanCastFireball(
            in character.Mana.Value.Current,
            in manaCost,
            in character.FireballCd.Current,
            out var canCast
        );

        if (canCast)
        {
            // 2. Apply changes (Layer B modifies primitives -> Layer A)
            SynergyLogic.ApplyFireballCost(
                ref character.Mana.Value.Current,
                in manaCost,
                ref character.FireballCd.Current,
                character.FireballCd.Duration
            );
            return true;
        }

        return false;
    }
}
