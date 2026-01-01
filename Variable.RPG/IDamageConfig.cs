namespace Variable.RPG;

/// <summary>
///     Configuration interface to map Elements to Stats.
///     Implement this struct to define your game's rules.
/// </summary>
public interface IDamageConfig
{
    /// <summary>
    ///     Maps an ElementID (Fire) to a StatID (FireResist) and mitigation type.
    /// </summary>
    /// <param name="elementId">The incoming element.</param>
    /// <param name="statId">The stat that resists this element.</param>
    /// <param name="isFlat">True for Armor (Flat), False for Resistance (Percent).</param>
    /// <returns>True if a mapping exists.</returns>
    bool TryGetMitigationStat(int elementId, out int statId, out bool isFlat);
}
