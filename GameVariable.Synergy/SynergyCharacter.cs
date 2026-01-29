namespace GameVariable.Synergy;

/// <summary>
/// A high-level structure that combines Health, Mana, Cooldown, and Experience.
/// Demonstrates the synergy between Variable.Bounded, Variable.Timer, Variable.Regen, and Variable.Experience.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct SynergyCharacter
{
    /// <summary>The character's health points (0 to Max).</summary>
    public BoundedFloat Health;

    /// <summary>The character's mana points (regenerates over time).</summary>
    public RegenFloat Mana;

    /// <summary>The cooldown timer for the primary ability.</summary>
    public Cooldown AttackCooldown;

    /// <summary>The character's experience and level progression.</summary>
    public ExperienceInt Experience;

    /// <summary>
    /// Creates a new SynergyCharacter with specified parameters.
    /// </summary>
    /// <param name="maxHealth">Maximum health value.</param>
    /// <param name="maxMana">Maximum mana value.</param>
    /// <param name="manaRegen">Mana regeneration per second.</param>
    /// <param name="cooldown">Ability cooldown in seconds.</param>
    public SynergyCharacter(float maxHealth, float maxMana, float manaRegen, float cooldown)
    {
        Health = new BoundedFloat(maxHealth, maxHealth);
        Mana = new RegenFloat(maxMana, maxMana, manaRegen);
        AttackCooldown = new Cooldown(cooldown);
        Experience = new ExperienceInt(100);
    }
}
