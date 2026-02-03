namespace GameVariable.Synergy;

/// <summary>
///     Pure logic for synergy operations.
///     All methods operate on primitives only - NO STRUCTS.
/// </summary>
public static class SynergyLogic
{
    /// <summary>
    ///     Updates the character's dynamic values (Regen, Cooldowns) based on time delta.
    /// </summary>
    /// <param name="dt">Time delta in seconds.</param>
    /// <param name="manaRate">Mana regeneration rate per second.</param>
    /// <param name="manaMax">Maximum mana capacity.</param>
    /// <param name="manaCurrent">Current mana value (ref).</param>
    /// <param name="cdCurrent">Current cooldown remaining time (ref).</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Tick(
        in float dt,
        in float manaRate,
        in float manaMax,
        ref float manaCurrent,
        ref float cdCurrent)
    {
        // Update Mana (Regen)
        RegenLogic.Tick(ref manaCurrent, 0f, manaMax, manaRate, dt);

        // Update Cooldown (Timer) - Counts down to 0
        TimerLogic.TickAndCheckReady(ref cdCurrent, dt);
    }

    /// <summary>
    ///     Attempts to cast an ability, checking mana and cooldown requirements.
    ///     If successful, consumes mana and resets cooldown.
    /// </summary>
    /// <param name="manaCost">Mana cost of the ability.</param>
    /// <param name="manaCurrent">Current mana value (ref).</param>
    /// <param name="cdCurrent">Current cooldown remaining time (ref).</param>
    /// <param name="cdDuration">Total duration of the cooldown to apply on success.</param>
    /// <param name="success">Output: true if cast succeeded, false otherwise.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void TryCast(
        in float manaCost,
        ref float manaCurrent,
        ref float cdCurrent,
        in float cdDuration,
        out bool success)
    {
        // 1. Check Cooldown (must be 0 or effectively 0)
        if (cdCurrent > MathConstants.Tolerance)
        {
            success = false;
            return;
        }

        // 2. Check Mana
        if (manaCurrent < manaCost)
        {
            success = false;
            return;
        }

        // 3. Consume Mana
        manaCurrent -= manaCost;

        // 4. Start Cooldown
        cdCurrent = cdDuration;

        success = true;
    }
}
