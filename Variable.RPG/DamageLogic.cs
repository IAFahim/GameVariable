namespace Variable.RPG;

/// <summary>
///     The Diamond Architecture Pipeline.
///     Aggregates incoming damage and applies mitigation based on defender stats.
/// </summary>
public static class DamageLogic
{
    /// <summary>
    ///     Calculates the final damage from a list of incoming sources against a defender's attributes.
    ///     Does NOT mutate the defender's Health (Pure Function).
    ///     Uses Span for zero-copy access to attribute arrays.
    ///     Generic constraint prevents boxing of struct configs.
    /// </summary>
    /// <typeparam name="TConfig">Config type implementing IDamageConfig (should be struct for zero alloc)</typeparam>
    /// <param name="stats">
    ///     A Span of Attributes indexed by StatID.
    ///     Assumes dense array where index == StatId.
    /// </param>
    /// <param name="damages">The list of incoming damage packets.</param>
    /// <param name="config">Configuration for IDs (Armors, Resists).</param>
    /// <returns>The total damage to apply to Health.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ResolveDamage<TConfig>(
        Span<Attribute> stats,
        ReadOnlySpan<DamagePacket> damages,
        TConfig config) where TConfig : IDamageConfig
    {
        var totalDamage = 0f;

        for (var i = 0; i < damages.Length; i++)
        {
            var dmg = damages[i];
            var amount = dmg.Amount;

            // 1. Get Mitigation Stat ID for this element
            if (config.TryGetMitigationStat(dmg.ElementId, out var statId, out var isFlat))
            {
                // Safety check
                if (statId >= 0 && statId < stats.Length)
                {
                    var mitigationValue = AttributeLogic.GetValue(ref stats[statId]);

                    if (isFlat)
                    {
                        // Armor style: Damage - Armor
                        amount -= mitigationValue;
                        // Clamp flat mitigation to prevent negative damage
                        if (amount < 0f) amount = 0f;
                    }
                    else
                    {
                        // Resistance style: Damage * (1 - Resist%)
                        // Allows amplified damage from negative resistance
                        amount *= (1.0f - mitigationValue);
                    }
                }
            }

            // 3. Aggregate
            totalDamage += amount;
        }

        return totalDamage;
    }
}
