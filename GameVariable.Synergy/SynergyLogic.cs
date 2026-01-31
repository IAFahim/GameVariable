namespace GameVariable.Synergy;

/// <summary>
///     Pure logic for calculating synergistic interactions between different variables.
/// </summary>
public static class SynergyLogic
{
    /// <summary>
    ///     Calculates the rage multiplier based on health.
    ///     If health is below 30%, the multiplier is 2.0x. Otherwise, it is 1.0x.
    /// </summary>
    /// <param name="healthCurrent">The current health value.</param>
    /// <param name="healthMax">The maximum health value.</param>
    /// <param name="multiplier">The calculated multiplier.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CalculateRageMultiplier(in float healthCurrent, in float healthMax, out float multiplier)
    {
        if (healthMax > 0 && (healthCurrent / healthMax) < 0.3f)
        {
            multiplier = 2.0f;
            return;
        }
        multiplier = 1.0f;
    }
}
