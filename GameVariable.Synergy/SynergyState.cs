namespace GameVariable.Synergy;

[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct SynergyState : IDisposable
{
    public RpgStatSheet Stats;
    public ExperienceInt Level;
    public RegenFloat Health;
    public RegenFloat Mana;

    public void Dispose()
    {
        Stats.Dispose();
    }
}
