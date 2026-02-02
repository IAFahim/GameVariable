namespace GameVariable.Synergy;

/// <summary>
/// Layer B: Pure Logic.
/// Performs checks and calculations for the Synergy system.
/// </summary>
public static class SynergyLogic
{
    /// <summary>
    /// Checks if an ability can be cast based on mana and cooldown state.
    /// </summary>
    /// <param name="manaCurrent">Current mana available.</param>
    /// <param name="cooldownCurrent">Current cooldown remaining.</param>
    /// <param name="cost">Mana cost of the ability.</param>
    /// <param name="canCast">True if cast is possible.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CanCastAbility(in float manaCurrent, in float cooldownCurrent, in float cost, out bool canCast)
    {
        // Must have enough mana AND cooldown must be finished (<= 0)
        canCast = manaCurrent >= cost && cooldownCurrent <= MathConstants.Tolerance;
    }

    /// <summary>
    /// Calculates the new state after casting an ability.
    /// </summary>
    /// <param name="manaCurrent">Current mana before cast.</param>
    /// <param name="cost">Mana cost to consume.</param>
    /// <param name="cooldownDuration">Duration to reset cooldown to.</param>
    /// <param name="newMana">New mana value.</param>
    /// <param name="newCooldown">New cooldown value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CalculatePostCastState(in float manaCurrent, in float cost, in float cooldownDuration, out float newMana, out float newCooldown)
    {
        newMana = manaCurrent - cost;
        // Clamp to 0 just in case, though CanCast should guard it.
        if (newMana < 0f) newMana = 0f;

        newCooldown = cooldownDuration;
    }
}
