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
    public static void ApplyMitigation(in float damage, in float mitigationValue, in bool isFlat, out float result)
    {
        // Use a local copy since 'damage' is now read-only 'in' parameter
        var finalDamage = damage;

        if (isFlat)
        {
            // Armor style: Damage - Armor
            finalDamage -= mitigationValue;
            // Clamp flat mitigation to prevent negative damage
            if (finalDamage < 0f) finalDamage = 0f;
        }
        else
        {
            // Resistance style: Damage * (1 - Resist%)
            // Allows amplified damage from negative resistance
            finalDamage *= 1.0f - mitigationValue;
        }

        result = finalDamage;
    }
}