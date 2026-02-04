using Variable.Regen;
using Variable.Timer;

namespace GameVariable.Synergy;

/// <summary>
///     Pure logic layer for the Synergy system.
///     Orchestrates updates for multiple variable types efficiently.
/// </summary>
public static class SynergyLogic
{
    /// <summary>
    ///     Ticks all time-based components of a Synergy character.
    ///     This demonstrates how to update multiple independent systems in one optimized pass.
    /// </summary>
    /// <param name="deltaTime">Time elapsed since last frame.</param>
    /// <param name="manaCurrent">Current mana value.</param>
    /// <param name="manaMin">Minimum mana value.</param>
    /// <param name="manaMax">Maximum mana value.</param>
    /// <param name="manaRate">Mana regeneration rate per second.</param>
    /// <param name="cooldownCurrent">Current cooldown remaining time.</param>
    /// <param name="newManaCurrent">The updated mana value.</param>
    /// <param name="newCooldownCurrent">The updated cooldown time.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Tick(
        float deltaTime,
        // Mana
        float manaCurrent, float manaMin, float manaMax, float manaRate,
        // Cooldown
        float cooldownCurrent,
        // Outputs
        out float newManaCurrent,
        out float newCooldownCurrent
    )
    {
        // 1. Mana Regeneration
        newManaCurrent = manaCurrent;
        RegenLogic.Tick(ref newManaCurrent, manaMin, manaMax, manaRate, deltaTime);

        // 2. Cooldown Tick
        newCooldownCurrent = cooldownCurrent;
        TimerLogic.TickAndCheckReady(ref newCooldownCurrent, deltaTime);
    }
}
