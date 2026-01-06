namespace Variable.RPG;

/// <summary>
///     Defines the type of operation to perform on a stat field.
///     Used in RPG systems for equipment, buffs, leveling, and passive abilities.
/// </summary>
public enum RpgStatOperation
{
    /// <summary>Set to absolute value (e.g., "Set Health to 100").</summary>
    Set = 0,
    
    /// <summary>Add flat value (e.g., "+5 Strength from Ring").</summary>
    Add = 1,
    
    /// <summary>Subtract flat value (e.g., "-10 Max Health debuff").</summary>
    Subtract = 2,
    
    /// <summary>Multiply by value (e.g., "×1.5 damage from buff").</summary>
    Multiply = 3,
    
    /// <summary>Divide by value (e.g., "÷2 speed from slow debuff").</summary>
    Divide = 4,
    
    /// <summary>Add percentage of base (e.g., "+20% Base Health").</summary>
    AddPercent = 5,
    
    /// <summary>Subtract percentage of base (e.g., "-30% Armor debuff").</summary>
    SubtractPercent = 6,
    
    /// <summary>Add percentage of current value (e.g., "Heal 50% of current health").</summary>
    AddPercentOfCurrent = 7,
    
    /// <summary>Subtract percentage of current value (e.g., "-25% current mana cost").</summary>
    SubtractPercentOfCurrent = 8,
    
    /// <summary>Set to minimum of current and value (e.g., "Cap at 100").</summary>
    Min = 9,
    
    /// <summary>Set to maximum of current and value (e.g., "At least 10").</summary>
    Max = 10
}

/// <summary>
///     Extension methods for RpgStatOperation.
/// </summary>
public static class RpgStatOperationExtensions
{
    /// <summary>
    ///     Gets the inverse operation for reverting a modifier.
    ///     Example: Add becomes Subtract, Multiply becomes Divide.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RpgStatOperation GetInverse(this RpgStatOperation operation)
    {
        return operation switch
        {
            RpgStatOperation.Add => RpgStatOperation.Subtract,
            RpgStatOperation.Subtract => RpgStatOperation.Add,
            RpgStatOperation.Multiply => RpgStatOperation.Divide,
            RpgStatOperation.Divide => RpgStatOperation.Multiply,
            RpgStatOperation.AddPercent => RpgStatOperation.SubtractPercent,
            RpgStatOperation.SubtractPercent => RpgStatOperation.AddPercent,
            RpgStatOperation.AddPercentOfCurrent => RpgStatOperation.SubtractPercentOfCurrent,
            RpgStatOperation.SubtractPercentOfCurrent => RpgStatOperation.AddPercentOfCurrent,
            
            // Non-invertible operations return themselves
            RpgStatOperation.Set => RpgStatOperation.Set,
            RpgStatOperation.Min => RpgStatOperation.Min,
            RpgStatOperation.Max => RpgStatOperation.Max,
            
            _ => operation
        };
    }

    /// <summary>
    ///     Checks if an operation can be safely inverted.
    ///     Set, Min, and Max operations cannot be inverted.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsInvertible(this RpgStatOperation operation)
    {
        return operation switch
        {
            RpgStatOperation.Set => false,
            RpgStatOperation.Min => false,
            RpgStatOperation.Max => false,
            _ => true
        };
    }
}
