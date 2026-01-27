using System;
using System.Runtime.InteropServices;
using GameVariable.Intent;
using Variable.Bounded;
using Variable.Experience;
using Variable.Regen;
using Variable.RPG;
using Variable.Timer;

namespace GameVariable.Synergy;

/// <summary>
///     The "Sergey" state: A composite struct demonstrating how all GameVariable systems
///     can coexist in a single, memory-efficient data structure.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public unsafe struct SynergyState : IDisposable
{
    /// <summary>
    ///     Health managed by Variable.Bounded (Layer A).
    /// </summary>
    public BoundedFloat Health;

    /// <summary>
    ///     Mana with regeneration managed by Variable.Regen (Layer A).
    /// </summary>
    public RegenFloat Mana;

    /// <summary>
    ///     Experience and leveling managed by Variable.Experience (Layer A).
    /// </summary>
    public ExperienceInt Experience;

    /// <summary>
    ///     Skill cooldown managed by Variable.Timer (Layer A).
    /// </summary>
    public Cooldown SkillCooldown;

    /// <summary>
    ///     Complex RPG stats (Armor, Strength, etc.) managed by Variable.RPG (Layer A).
    ///     REQUIRES DISPOSAL.
    /// </summary>
    public RpgStatSheet Stats;

    /// <summary>
    ///     Gold currency, managed by Variable.Inventory logic (Layer B).
    /// </summary>
    public float Gold;

    /// <summary>
    ///     Max gold capacity.
    /// </summary>
    public float MaxGold;

    /// <summary>
    ///     AI State Machine managed by GameVariable.Intent.
    /// </summary>
    public IntentState AiState;

    /// <summary>
    ///     Disposes the unmanaged resources (RpgStatSheet).
    /// </summary>
    public void Dispose()
    {
        Stats.Dispose();
    }
}
