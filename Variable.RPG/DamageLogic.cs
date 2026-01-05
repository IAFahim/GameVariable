namespace Variable.RPG;

/// <summary>
///     The Diamond Architecture Pipeline.
///     Aggregates incoming damage and applies mitigation based on defender stats.
/// </summary>
public static class DamageLogic
{
    /// <summary>
    ///     Calculates the mitigated damage for a single packet.
    ///     Pure primitive logic.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ApplyMitigation(float damage, float mitigationValue, bool isFlat)
    {
        if (isFlat)
        {
            // Armor style: Damage - Armor
            damage -= mitigationValue;
            // Clamp flat mitigation to prevent negative damage
            if (damage < 0f) damage = 0f;
        }
        else
        {
            // Resistance style: Damage * (1 - Resist%)
            // Allows amplified damage from negative resistance
            damage *= 1.0f - mitigationValue;
        }

        return damage;
    }
}