namespace Variable.RPG;

/// <summary>
///     Enum representing individual fields of an RpgStat.
///     Used for selective modification of specific stat properties.
/// </summary>
[Flags]
public enum RpgStatField
{
    /// <summary>No field selected.</summary>
    None = 0,
    
    /// <summary>The base value field.</summary>
    Base = 1 << 0,      // 1
    
    /// <summary>The flat modifier field.</summary>
    ModAdd = 1 << 1,    // 2
    
    /// <summary>The multiplier modifier field.</summary>
    ModMult = 1 << 2,   // 4
    
    /// <summary>The minimum bound field.</summary>
    Min = 1 << 3,       // 8
    
    /// <summary>The maximum bound field.</summary>
    Max = 1 << 4,       // 16
    
    /// <summary>The cached calculated value field.</summary>
    Value = 1 << 5      // 32
}
