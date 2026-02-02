namespace GameVariable.Synergy;

/// <summary>
/// Represents a complete character state, integrating Health, Mana, Cooldowns, and XP.
/// Layer A: Pure Data.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct SynergyCharacter
{
    /// <summary>
    /// The character's health points.
    /// </summary>
    public BoundedFloat Health;

    /// <summary>
    /// The character's mana points, which regenerates over time.
    /// </summary>
    public RegenFloat Mana;

    /// <summary>
    /// The cooldown for the character's primary ability.
    /// </summary>
    public Cooldown AbilityCooldown;

    /// <summary>
    /// The character's experience and level.
    /// </summary>
    public ExperienceInt XP;
}
