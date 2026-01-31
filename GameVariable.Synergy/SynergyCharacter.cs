using System;
using System.Runtime.InteropServices;
using Variable.Bounded;
using Variable.Regen;
using Variable.Timer;
using Variable.Experience;

namespace GameVariable.Synergy;

/// <summary>
///     A composite structure representing a game character, demonstrating the synergy
///     between Health, Mana, Cooldowns, and Experience.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct SynergyCharacter
{
    /// <summary>The character's health points.</summary>
    public BoundedFloat Health;

    /// <summary>The character's mana points, which regenerate over time.</summary>
    public RegenFloat Mana;

    /// <summary>The cooldown timer for the Fireball ability.</summary>
    public Cooldown FireballCd;

    /// <summary>The character's experience and level progression.</summary>
    public ExperienceInt XP;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SynergyCharacter"/> struct.
    /// </summary>
    /// <param name="maxHealth">The maximum health.</param>
    /// <param name="maxMana">The maximum mana.</param>
    /// <param name="manaRegenRate">The mana regeneration rate per second.</param>
    /// <param name="fireballCooldown">The cooldown duration for the fireball.</param>
    /// <param name="xpForNextLevel">The XP required for the first level up.</param>
    public SynergyCharacter(float maxHealth, float maxMana, float manaRegenRate, float fireballCooldown, int xpForNextLevel)
    {
        Health = new BoundedFloat(maxHealth);
        // RegenFloat(max, current, regenRate) -> We start with full mana
        Mana = new RegenFloat(maxMana, maxMana, manaRegenRate);
        FireballCd = new Cooldown(fireballCooldown);
        XP = new ExperienceInt(xpForNextLevel);
    }
}
