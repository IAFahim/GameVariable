namespace GameVariable.Synergy;

/// <summary>
///     A composite structure demonstrating the synergy between multiple Variable packages.
///     Represents a character with Health, Mana, Cooldowns, and Experience.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct SynergyCharacter
{
    /// <summary>Health managed by Variable.Bounded.</summary>
    public BoundedFloat Health;

    /// <summary>Mana managed by Variable.Regen (which wraps Variable.Bounded).</summary>
    public RegenFloat Mana;

    /// <summary>Primary ability cooldown managed by Variable.Timer.</summary>
    public Cooldown AbilityCooldown;

    /// <summary>Experience and Level managed by Variable.Experience.</summary>
    public ExperienceInt XP;
}
