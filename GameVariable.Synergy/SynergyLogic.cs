using System.Runtime.CompilerServices;
using Variable.Core;
using Variable.Timer;

namespace GameVariable.Synergy;

/// <summary>
///     Pure logic for the Synergy character.
///     All methods operate on primitives only - NO STRUCTS.
/// </summary>
public static class SynergyLogic
{
    /// <summary>
    ///     Checks if the fireball can be cast.
    /// </summary>
    /// <param name="manaCurrent">Current mana.</param>
    /// <param name="manaCost">Mana cost of the spell.</param>
    /// <param name="cdCurrent">Current cooldown remaining.</param>
    /// <param name="result">True if cast is possible.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CanCastFireball(in float manaCurrent, in float manaCost, in float cdCurrent, out bool result)
    {
        // Must have enough mana AND cooldown must be finished (<= 0/Tolerance)
        // TimerLogic.IsEmpty checks <= Tolerance, which is effectively "Ready" for a cooldown counting down to 0.
        result = manaCurrent >= manaCost && TimerLogic.IsEmpty(cdCurrent);
    }

    /// <summary>
    ///     Applies the cost of casting a fireball: consumes mana and resets cooldown.
    ///     Assumes checks have already passed.
    /// </summary>
    /// <param name="manaCurrent">Reference to current mana to modify.</param>
    /// <param name="manaCost">Mana cost to deduct.</param>
    /// <param name="cdCurrent">Reference to cooldown to reset.</param>
    /// <param name="cdDuration">Duration to reset cooldown to.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ApplyFireballCost(ref float manaCurrent, in float manaCost, ref float cdCurrent, in float cdDuration)
    {
        manaCurrent -= manaCost;
        cdCurrent = cdDuration;
    }
}
