namespace Variable.RPG;

/// <summary>
///     Represents a single stat modification with field, operation, and value.
///     Immutable by design for cache-friendly buff/equipment systems.
///     Can be used in arrays for zero-allocation buff processing.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct RpgStatModifier
{
    /// <summary>Which field to modify (Base, ModAdd, Max, etc.).</summary>
    public RpgStatField Field;
    
    /// <summary>What operation to perform (Add, Multiply, etc.).</summary>
    public RpgStatOperation Operation;
    
    /// <summary>The value for the operation.</summary>
    public float Value;
    

    /// <summary>
    ///     Creates a new stat modifier.
    /// </summary>
    /// <param name="field">Which field to modify.</param>
    /// <param name="operation">What operation to perform.</param>
    /// <param name="value">The value for the operation.</param>
    /// <param name="sourceId">Optional source identifier for tracking.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RpgStatModifier(RpgStatField field, RpgStatOperation operation, float value, int sourceId = 0)
    {
        Field = field;
        Operation = operation;
        Value = value;
    }

    /// <summary>
    ///     Creates a modifier that adds a flat value.
    ///     Example: "+5 Strength"
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RpgStatModifier AddFlat(RpgStatField field, float value, int sourceId = 0)
        => new(field, RpgStatOperation.Add, value, sourceId);

    /// <summary>
    ///     Creates a modifier that adds a multiplier.
    ///     Example: "+20% (pass 0.2f, not 20f)"
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RpgStatModifier AddPercent(RpgStatField field, float percent, int sourceId = 0)
        => new(field, RpgStatOperation.AddPercent, percent, sourceId);

    /// <summary>
    ///     Creates a modifier that sets an absolute value.
    ///     Example: "Set Max Health to 150"
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RpgStatModifier SetValue(RpgStatField field, float value, int sourceId = 0)
        => new(field, RpgStatOperation.Set, value, sourceId);
}
