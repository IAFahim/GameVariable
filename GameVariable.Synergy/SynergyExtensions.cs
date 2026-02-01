using System.Runtime.CompilerServices;
using Variable.Regen;
using Variable.Timer;

namespace GameVariable.Synergy
{
    /// <summary>
    /// Extension methods for SynergyCharacter (Layer C).
    /// Bridges the pure data (Layer A) with the core logic (Layer B).
    /// </summary>
    public static class SynergyExtensions
    {
        /// <summary>
        /// Updates the character's state (Mana regen, Cooldowns).
        /// Should be called every frame.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Tick(ref this SynergyCharacter character, float deltaTime)
        {
            // Update Mana (Regen)
            character.Mana.Tick(deltaTime);

            // Update Cooldown (Timer)
            character.AbilityCooldown.Tick(deltaTime);
        }

        /// <summary>
        /// Attempts to cast an ability consuming Mana and triggering a Cooldown.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryCast(ref this SynergyCharacter character, float manaCost)
        {
            SynergyLogic.TryCastAbility(
                in character.Mana.Value.Current,
                in manaCost,
                in character.AbilityCooldown.Current,
                in character.AbilityCooldown.Duration,
                out bool success,
                out float newMana,
                out float newCooldown
            );

            if (success)
            {
                // Apply changes
                character.Mana.Value.Current = newMana;
                character.AbilityCooldown.Current = newCooldown;
            }

            return success;
        }

        /// <summary>
        /// Checks if the character is alive based on Health.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAlive(in this SynergyCharacter character)
        {
            SynergyLogic.CalculateStatus(
                in character.Health.Current,
                in character.Health.Max,
                out bool isAlive,
                out _ // We don't need ratio here
            );
            return isAlive;
        }

        /// <summary>
        /// Gets the health ratio (0.0 to 1.0).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetHealthRatio(in this SynergyCharacter character)
        {
            SynergyLogic.CalculateStatus(
                in character.Health.Current,
                in character.Health.Max,
                out _,
                out float ratio
            );
            return ratio;
        }
    }
}
