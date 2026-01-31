namespace GameVariable.Synergy;

/// <summary>
///     A composite character struct that bundles Health, Stamina, and Experience.
///     Demonstrates how GameVariable primitives interact.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct SynergyCharacter
{
    /// <summary>The character's health.</summary>
    public BoundedFloat Health;

    /// <summary>The character's stamina.</summary>
    public RegenFloat Stamina;

    /// <summary>The character's experience level.</summary>
    public ExperienceInt Level;
}
