using System.Runtime.InteropServices;
using Variable.Bounded;
using Variable.Experience;
using Variable.Regen;
using Variable.RPG;

namespace GameVariable.Synergy;

/// <summary>
///     Represents the combined state of a game entity, integrating multiple GameVariable modules.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct SynergyState
{
    /// <summary>
    ///     Experience and Level progress.
    /// </summary>
    public ExperienceInt Experience;

    /// <summary>
    ///     RPG Statistics (e.g. Strength, Intelligence).
    ///     Requires manual disposal via Dispose().
    /// </summary>
    public RpgStatSheet Stats;

    /// <summary>
    ///     Health with min/max bounds.
    /// </summary>
    public BoundedFloat Health;

    /// <summary>
    ///     Mana with regeneration logic.
    /// </summary>
    public RegenFloat Mana;

    /// <summary>
    ///     Initializes the state with default values.
    ///     Allocates unmanaged memory for Stats.
    /// </summary>
    public SynergyState(int statCount)
    {
        Experience = new ExperienceInt(); // Level 0
        Stats = new RpgStatSheet(statCount);
        Health = new BoundedFloat(100f, 100f);
        Mana = new RegenFloat(new BoundedFloat(50f, 50f), 1f); // 1 mana per sec
    }

    /// <summary>
    ///     Releases unmanaged resources held by the state.
    /// </summary>
    public void Dispose()
    {
        Stats.Dispose();
    }
}
