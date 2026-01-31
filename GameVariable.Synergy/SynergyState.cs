namespace GameVariable.Synergy;

/// <summary>
///     Composite state struct integrating Experience, Stats, and Regen components.
///     <para>Implements the "Diamond" integration pattern.</para>
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct SynergyState : IDisposable
{
    /// <summary>Experience and Level state.</summary>
    public ExperienceInt Experience;

    /// <summary>RPG Attributes (Stats). Manages unmanaged memory.</summary>
    public RpgStatSheet Stats;

    /// <summary>Health with regeneration capabilities.</summary>
    public RegenFloat Health;

    /// <summary>Mana with regeneration capabilities.</summary>
    public RegenFloat Mana;

    /// <summary>
    ///     Creates a new SynergyState with space for the specified number of stats.
    /// </summary>
    /// <param name="statCount">Number of stats to allocate in the RpgStatSheet.</param>
    public SynergyState(int statCount)
    {
        Experience = new ExperienceInt(1000);
        Stats = new RpgStatSheet(statCount);
        Health = new RegenFloat(100f, 100f, 1f);
        Mana = new RegenFloat(50f, 50f, 5f);
    }

    /// <summary>
    ///     Disposes the underlying unmanaged memory of the stat sheet.
    /// </summary>
    public void Dispose()
    {
        Stats.Dispose();
    }
}
