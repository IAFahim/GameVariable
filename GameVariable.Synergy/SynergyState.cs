using System;
using Variable.Experience;
using Variable.Regen;
using Variable.RPG;

namespace GameVariable.Synergy;

/// <summary>
///     The "Conductor" state container. Orchestrates Stats, XP, Health, and Mana.
///     Acts as the central integration point ("Sergey") for the GameVariable ecosystem.
/// </summary>
public unsafe struct SynergyState : IDisposable
{
    /// <summary>
    ///     The unmanaged stat sheet.
    ///     Must be disposed.
    /// </summary>
    public RpgStatSheet Stats;

    /// <summary>
    ///     Experience and Level tracker.
    /// </summary>
    public ExperienceInt XP;

    /// <summary>
    ///     Health with regeneration.
    /// </summary>
    public RegenFloat Health;

    /// <summary>
    ///     Mana with regeneration.
    /// </summary>
    public RegenFloat Mana;

    /// <summary>
    ///     Initializes a new SynergyState.
    /// </summary>
    /// <param name="statCount">Number of stats to allocate.</param>
    /// <param name="xpMax">Initial max XP.</param>
    /// <param name="healthMax">Initial max health.</param>
    /// <param name="manaMax">Initial max mana.</param>
    public SynergyState(int statCount, int xpMax, float healthMax, float manaMax)
    {
        Stats = new RpgStatSheet(statCount);
        XP = new ExperienceInt(xpMax);
        // Initial rates are 0, they should be set via Sync or logic later
        Health = new RegenFloat(healthMax, healthMax, 0f);
        Mana = new RegenFloat(manaMax, manaMax, 0f);
    }

    /// <summary>
    ///     Disposes the unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Stats.Dispose();
    }
}
