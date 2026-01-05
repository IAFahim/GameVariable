namespace Variable.RPG;

/// <summary>
///     Extension methods for damage processing.
///     Orchestrates the interaction between Attributes and DamageLogic.
/// </summary>
public static class DamageExtensions
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
        this Span<Attribute> stats,
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
                // Safety check
                if (statId >= 0 && statId < stats.Length)
                {
                    // Use AttributeExtensions to get value (which calls Logic)
                    var mitigationValue = stats[statId].GetValue();
                    amount = DamageLogic.ApplyMitigation(amount, mitigationValue, isFlat);
                }

            // 3. Aggregate
            totalDamage += amount;
        }

        return totalDamage;
    }
}