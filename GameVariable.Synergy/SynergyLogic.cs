using System.Runtime.CompilerServices;

namespace GameVariable.Synergy
{
    /// <summary>
    /// Pure logic layer for Synergy calculations.
    /// Operates only on primitives.
    /// </summary>
    public static class SynergyLogic
    {
        /// <summary>
        /// Attempts to cast an ability, checking mana and cooldown.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TryCastAbility(
            in float manaCurrent,
            in float manaCost,
            in float cooldownCurrent,
            in float cooldownDuration,
            out bool success,
            out float newMana,
            out float newCooldownCurrent)
        {
            // Check constraints
            // We assume 0 is the threshold for cooldown readiness
            bool isCooldownReady = cooldownCurrent <= 0f;
            bool hasEnoughMana = manaCurrent >= manaCost;

            if (isCooldownReady && hasEnoughMana)
            {
                success = true;
                newMana = manaCurrent - manaCost;
                newCooldownCurrent = cooldownDuration;
            }
            else
            {
                success = false;
                newMana = manaCurrent;
                newCooldownCurrent = cooldownCurrent;
            }
        }

        /// <summary>
        /// Calculates derived status from health.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CalculateStatus(
            in float healthCurrent,
            in float healthMax,
            out bool isAlive,
            out float healthRatio)
        {
            isAlive = healthCurrent > 0f;

            // Avoid division by zero
            if (healthMax > 0f)
            {
                 healthRatio = healthCurrent / healthMax;
            }
            else
            {
                 healthRatio = 0f;
            }
        }
    }
}
