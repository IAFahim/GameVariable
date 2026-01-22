using Variable.Bounded;
using Variable.Experience;
using Variable.Reservoir;
using Variable.RPG;
using Variable.Timer;

namespace GameVariable.Synergy;

/// <summary>
///     Layer B: Core Logic for Character Synergy.
///     Stateless, static methods operating on primitive data from the composite struct.
/// </summary>
public static class CharacterLogic
{
    // Stat IDs (Example mapping)
    /// <summary>Vitality stat ID (Increases Health).</summary>
    public const int STAT_VITALITY = 0;
    /// <summary>Intelligence stat ID (Increases Mana).</summary>
    public const int STAT_INTELLIGENCE = 1;
    /// <summary>Health Regen stat ID.</summary>
    public const int STAT_REGEN_HEALTH = 2;
    /// <summary>Mana Regen stat ID.</summary>
    public const int STAT_REGEN_MANA = 3;

    /// <summary>
    ///     Updates max health and mana based on stats.
    ///     Vitality -> Max Health (1 Vit = 10 HP)
    ///     Intelligence -> Max Mana (1 Int = 5 Mana)
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SynchronizeStats(ref RpgStatSheet stats, ref BoundedFloat health, ref ReservoirFloat mana)
    {
        // Calculate Max HP from Vitality
        float vitality = stats.Get(STAT_VITALITY);
        float newMaxHealth = 100f + (vitality * 10f);

        // Calculate Max Mana from Intelligence
        float intelligence = stats.Get(STAT_INTELLIGENCE);
        float newMaxMana = 50f + (intelligence * 5f);

        // Update Health Max
        health.Max = newMaxHealth;
        if (health.Current > health.Max) health.Current = health.Max;

        // Update Mana Max
        mana.Volume.Max = newMaxMana;
        if (mana.Volume.Current > mana.Volume.Max) mana.Volume.Current = mana.Volume.Max;
    }

    /// <summary>
    ///     Regenerates health and mana based on stats and delta time.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Regenerate(ref RpgStatSheet stats, ref BoundedFloat health, ref ReservoirFloat mana, in float deltaTime)
    {
        float healthRegen = stats.Get(STAT_REGEN_HEALTH) * deltaTime;
        float manaRegen = stats.Get(STAT_REGEN_MANA) * deltaTime;

        // Apply Health Regen
        if (healthRegen > 0)
        {
             health.Current += healthRegen;
             if (health.Current > health.Max) health.Current = health.Max;
        }

        // Apply Mana Regen
        if (manaRegen > 0)
        {
             mana.Volume.Current += manaRegen;
             if (mana.Volume.Current > mana.Volume.Max) mana.Volume.Current = mana.Volume.Max;
        }
    }

    /// <summary>
    ///     Attempts to cast a spell.
    ///     Checks cooldown and mana.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void TryCastSpell(
        ref ReservoirFloat mana,
        ref Cooldown cooldown,
        in float manaCost,
        in float cooldownDuration,
        out bool success)
    {
        // Check Cooldown (counts down to 0)
        // If Current > 0, it's not ready.
        if (cooldown.Current > 0f)
        {
            success = false;
            return;
        }

        if (mana.Volume.Current < manaCost)
        {
            success = false;
            return;
        }

        // Consume Mana
        mana.Volume.Current -= manaCost;

        // Start Cooldown
        // We set duration and current
        cooldown.Duration = cooldownDuration;
        cooldown.Current = cooldownDuration;

        success = true;
    }

    /// <summary>
    ///     Gain experience and level up if needed.
    ///     On level up, increase base stats.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddExperience(
        ref ExperienceInt experience,
        ref RpgStatSheet stats,
        in int amount,
        out bool leveledUp)
    {
        leveledUp = false;
        experience.Current += amount;

        // Ensure Max is set (if 0, init it)
        if (experience.Max == 0) experience.Max = experience.Level * 1000;

        // Check for level up
        while (experience.Current >= experience.Max)
        {
            experience.Current -= experience.Max;
            experience.Level++;

            // Increase requirement for next level
            experience.Max = experience.Level * 1000;

            leveledUp = true;

            // Synergy: Increase stats on level up
            // Increase Vit and Int
            stats.GetRef(STAT_VITALITY).Base += 1f;
            stats.GetRef(STAT_INTELLIGENCE).Base += 1f;
        }
    }
}
