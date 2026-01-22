using Variable.Bounded;
using Variable.Experience;
using Variable.Reservoir;
using Variable.RPG;
using Variable.Timer;

namespace GameVariable.Synergy;

/// <summary>
///     A composite state struct demonstrating synergy between multiple GameVariable modules.
///     Adheres to Layer A (Pure Data) principles, but requires manual disposal due to RpgStatSheet.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct CharacterState : IDisposable
{
    /// <summary>
    ///     Health managed by BoundedFloat.
    ///     Max health can be derived from Stats.
    /// </summary>
    public BoundedFloat Health;

    /// <summary>
    ///     Mana managed by ReservoirFloat (Volume + Reserve).
    ///     Max mana can be derived from Stats.
    /// </summary>
    public ReservoirFloat Mana;

    /// <summary>
    ///     Experience points managed by ExperienceInt.
    /// </summary>
    public ExperienceInt Experience;

    /// <summary>
    ///     Global cooldown timer (counts down).
    /// </summary>
    public Cooldown Cooldown;

    /// <summary>
    ///     Stat sheet managing attributes (Str, Int, Vit, etc.) in unmanaged memory.
    /// </summary>
    public RpgStatSheet Stats;

    /// <summary>
    ///     Initializes a new CharacterState.
    /// </summary>
    public CharacterState(int statCount)
    {
        Health = new BoundedFloat(100f, 100f);
        Mana = new ReservoirFloat(50f, 50f, 0f);
        Experience = new ExperienceInt(1000); // Start at level 1, 1000 XP to next
        Cooldown = new Cooldown(0f, 0f); // Initially ready
        Stats = new RpgStatSheet(statCount);
    }

    /// <summary>
    ///     Disposes the underlying unmanaged memory in Stats.
    /// </summary>
    public void Dispose()
    {
        Stats.Dispose();
    }
}
