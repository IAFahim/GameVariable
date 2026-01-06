namespace Variable.RPG;

/// <summary>
///     Pure logic for recalculating attributes.
///     All methods operate on primitives only - NO STRUCTS in parameters.
/// </summary>
public static class RpgStatLogic
{
    /// <summary>
    ///     Formula: (Base + Add) * Mult, clamped to [Min, Max].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Recalculate(float baseVal, float modAdd, float modMult, float min, float max, out float value)
    {
        var val = (baseVal + modAdd) * modMult;

        if (val > max) val = max;
        else if (val < min) val = min;

        value = val;
    }

    /// <summary>
    ///     Adds a temporary modifier to the attribute.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddModifier(ref float modAdd, ref float modMult, float flat, float percent)
    {
        modAdd += flat;
        modMult += percent;
    }

    /// <summary>
    ///     Resets modifiers to default (Add=0, Mult=1).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ClearModifiers(out float modAdd, out float modMult)
    {
        modAdd = 0f;
        modMult = 1f;
    }

    /// <summary>
    ///     Gets the value, recalculating immediately if dirty.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetValueRecalculated(ref float value, float baseVal, float modAdd, float modMult, float min,
        float max)
    {
        Recalculate(baseVal, modAdd, modMult, min, max, out value);
        return value;
    }

    /// <summary>
    ///     Sets a specific field value by field selector.
    ///     Returns true if the field was valid, false otherwise.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TrySetField(RpgStatField field, float newValue, 
        ref float baseVal, ref float modAdd, ref float modMult, 
        ref float min, ref float max, ref float value)
    {
        switch (field)
        {
            case RpgStatField.Base:
                baseVal = newValue;
                return true;
            case RpgStatField.ModAdd:
                modAdd = newValue;
                return true;
            case RpgStatField.ModMult:
                modMult = newValue;
                return true;
            case RpgStatField.Min:
                min = newValue;
                return true;
            case RpgStatField.Max:
                max = newValue;
                return true;
            case RpgStatField.Value:
                value = newValue;
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    ///     Gets a specific field value by field selector.
    ///     Returns true if the field was valid, false otherwise.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGetField(RpgStatField field, 
        float baseVal, float modAdd, float modMult, 
        float min, float max, float value, 
        out float result)
    {
        switch (field)
        {
            case RpgStatField.Base:
                result = baseVal;
                return true;
            case RpgStatField.ModAdd:
                result = modAdd;
                return true;
            case RpgStatField.ModMult:
                result = modMult;
                return true;
            case RpgStatField.Min:
                result = min;
                return true;
            case RpgStatField.Max:
                result = max;
                return true;
            case RpgStatField.Value:
                result = value;
                return true;
            default:
                result = 0f;
                return false;
        }
    }

    /// <summary>
    ///     Applies an operation to a field value.
    ///     Pure function - returns the new value without mutation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ApplyOperation(RpgStatOperation operation, float currentValue, float operandValue, float baseValue = 0f)
    {
        switch (operation)
        {
            case RpgStatOperation.Set:
                return operandValue;
            
            case RpgStatOperation.Add:
                return currentValue + operandValue;
            
            case RpgStatOperation.Subtract:
                return currentValue - operandValue;
            
            case RpgStatOperation.Multiply:
                return currentValue * operandValue;
            
            case RpgStatOperation.Divide:
                // Early exit: prevent division by zero
                if (operandValue == 0f) return currentValue;
                return currentValue / operandValue;
            
            case RpgStatOperation.AddPercent:
                return currentValue + (baseValue * operandValue);
            
            case RpgStatOperation.SubtractPercent:
                return currentValue - (baseValue * operandValue);
            
            case RpgStatOperation.AddPercentOfCurrent:
                return currentValue + (currentValue * operandValue);
            
            case RpgStatOperation.SubtractPercentOfCurrent:
                return currentValue - (currentValue * operandValue);
            
            case RpgStatOperation.Min:
                return currentValue < operandValue ? currentValue : operandValue;
            
            case RpgStatOperation.Max:
                return currentValue > operandValue ? currentValue : operandValue;
            
            default:
                return currentValue;
        }
    }
}