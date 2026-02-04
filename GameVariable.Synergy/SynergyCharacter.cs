using Variable.Bounded;
using Variable.Experience;
using Variable.Regen;
using Variable.Timer;

namespace GameVariable.Synergy;

/// <summary>
///     A composite character struct demonstrating the synergy between different Variable components.
///     Acts as "The Orchestra" combining Health, Mana, Cooldowns, and XP.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct SynergyCharacter
{
    /// <summary>Character's Health (BoundedFloat).</summary>
    public BoundedFloat Health;

    /// <summary>Character's Mana with Regeneration (RegenFloat).</summary>
    public RegenFloat Mana;

    /// <summary>Character's Ability Cooldown (Cooldown).</summary>
    public Cooldown AbilityCooldown;

    /// <summary>Character's Experience (ExperienceInt).</summary>
    public ExperienceInt XP;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SynergyCharacter" /> struct.
    /// </summary>
    public SynergyCharacter(BoundedFloat health, RegenFloat mana, Cooldown abilityCooldown, ExperienceInt xp)
    {
        Health = health;
        Mana = mana;
        AbilityCooldown = abilityCooldown;
        XP = xp;
    }
}
