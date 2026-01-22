using Variable.Bounded;
using Variable.Experience;
using Variable.Reservoir;
using Variable.RPG;
using Variable.Timer;

namespace GameVariable.Synergy;

/// <summary>
///     Layer C: Extension methods for CharacterState.
///     Provides a clean API for the user.
/// </summary>
public static class CharacterExtensions
{
    /// <summary>
    ///     Updates the character (regenerates health/mana, ticks cooldowns).
    /// </summary>
    public static void Update(ref this CharacterState state, float deltaTime)
    {
        // Tick Cooldown (counts down)
        // Variable.Timer.TimerExtensions provides Tick
        state.Cooldown.Tick(deltaTime);

        // Regenerate
        CharacterLogic.Regenerate(ref state.Stats, ref state.Health, ref state.Mana, in deltaTime);

        // Sync stats (in case dynamic modifiers changed stats)
        CharacterLogic.SynchronizeStats(ref state.Stats, ref state.Health, ref state.Mana);
    }

    /// <summary>
    ///     Attempts to cast a spell.
    /// </summary>
    public static bool CastSpell(ref this CharacterState state, float manaCost, float cooldownDuration)
    {
        CharacterLogic.TryCastSpell(
            ref state.Mana,
            ref state.Cooldown,
            in manaCost,
            in cooldownDuration,
            out bool success
        );
        return success;
    }

    /// <summary>
    ///     Adds experience and handles level up synergy.
    /// </summary>
    public static void GainXp(ref this CharacterState state, int amount)
    {
        CharacterLogic.AddExperience(ref state.Experience, ref state.Stats, in amount, out bool leveledUp);

        if (leveledUp)
        {
            // Sync stats immediately to reflect new Max HP/Mana from increased stats
            CharacterLogic.SynchronizeStats(ref state.Stats, ref state.Health, ref state.Mana);

            // Full heal on level up? (Optional synergy)
            state.Health.Current = state.Health.Max;
            state.Mana.Volume.Current = state.Mana.Volume.Max;
        }
    }
}
