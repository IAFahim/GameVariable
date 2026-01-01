namespace Variable.RPG;

/// <summary>
///     Represents a single instance of incoming damage.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct DamagePacket
{
    /// <summary>The type of damage (Physical, Fire, etc.). User defined ID.</summary>
    public int ElementId;

    /// <summary>The raw amount of damage.</summary>
    public float Amount;

    /// <summary>Generic flag for critical hits, etc.</summary>
    public int Flags;
}
