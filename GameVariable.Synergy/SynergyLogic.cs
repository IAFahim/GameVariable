namespace GameVariable.Synergy;

public static class SynergyLogic
{
    /// <summary>
    /// Calculates Max Health based on Vitality.
    /// Formula: MaxHealth = Vitality * 10.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CalculateMaxHealth(in float vitality, out float maxHealth)
    {
        maxHealth = vitality * 10f;
    }

    /// <summary>
    /// Calculates Max Mana based on Intellect.
    /// Formula: MaxMana = Intellect * 10.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CalculateMaxMana(in float intellect, out float maxMana)
    {
        maxMana = intellect * 10f;
    }

    /// <summary>
    /// Calculates Regen Rate based on Spirit.
    /// Formula: Regen = Spirit * 0.5.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CalculateRegen(in float spirit, out float regenRate)
    {
        regenRate = spirit * 0.5f;
    }

    /// <summary>
    /// Calculates a base stat value based on level.
    /// Formula: Start + (Level * Scale).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CalculateBaseStat(in int level, in float valPerLevel, in float startingVal, out float result)
    {
        result = startingVal + (level * valPerLevel);
    }
}
