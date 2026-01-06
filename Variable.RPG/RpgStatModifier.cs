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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RpgStatModifier(RpgStatField field, RpgStatOperation operation, float value)
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
    public static RpgStatModifier AddFlat(RpgStatField field, float value)
    {
        return new RpgStatModifier(field, RpgStatOperation.Add, value);
    }

    /// <summary>
    ///     Creates a modifier that adds a multiplier.
    ///     Example: "+20% (pass 0.2f, not 20f)"
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RpgStatModifier AddPercent(RpgStatField field, float percent)
    {
        return new RpgStatModifier(field, RpgStatOperation.AddPercent, percent);
    }

    /// <summary>
    ///     Creates a modifier that sets an absolute value.
    ///     Example: "Set Max Health to 150"
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RpgStatModifier SetValue(RpgStatField field, float value)
    {
        return new RpgStatModifier(field, RpgStatOperation.Set, value);
    }

    /// <summary>
    ///     Returns the inverse of this modifier for easy removal.
    ///     Example: Add becomes Subtract, Multiply becomes Divide.
    ///     Non-invertible operations (Set, Min, Max) throw exception.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RpgStatModifier GetInverse()
    {
        if (!Operation.IsInvertible())
            throw new InvalidOperationException(
                $"Operation {Operation} cannot be inverted. Use manual removal instead.");

        return new RpgStatModifier(Field, Operation.GetInverse(), Value);
    }

    /// <summary>
    ///     Tries to get the inverse of this modifier.
    ///     Returns false for non-invertible operations (Set, Min, Max).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetInverse(out RpgStatModifier inverse)
    {
        if (!Operation.IsInvertible())
        {
            inverse = default;
            return false;
        }

        inverse = new RpgStatModifier(Field, Operation.GetInverse(), Value);
        return true;
    }
}