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
    public static void GetValueRecalculated(ref float value, float baseVal, float modAdd, float modMult, float min,
        float max, out float result)
    {
        Recalculate(baseVal, modAdd, modMult, min, max, out value);
        result = value;
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
    ///     Pure function - calculates the new value without mutation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ApplyOperation(RpgStatOperation operation, float currentValue, float operandValue,
        out float result, float baseValue = 0f)
    {
        switch (operation)
        {
            case RpgStatOperation.Set:
                result = operandValue;
                return;

            case RpgStatOperation.Add:
                result = currentValue + operandValue;
                return;

            case RpgStatOperation.Subtract:
                result = currentValue - operandValue;
                return;

            case RpgStatOperation.Multiply:
                result = currentValue * operandValue;
                return;

            case RpgStatOperation.Divide:
                // Early exit: prevent division by zero
                if (operandValue == 0f)
                {
                    result = currentValue;
                    return;
                }

                result = currentValue / operandValue;
                return;

            case RpgStatOperation.AddPercent:
                result = currentValue + baseValue * operandValue;
                return;

            case RpgStatOperation.SubtractPercent:
                result = currentValue - baseValue * operandValue;
                return;

            case RpgStatOperation.AddPercentOfCurrent:
                result = currentValue + currentValue * operandValue;
                return;

            case RpgStatOperation.SubtractPercentOfCurrent:
                result = currentValue - currentValue * operandValue;
                return;

            case RpgStatOperation.Min:
                result = currentValue < operandValue ? currentValue : operandValue;
                return;

            case RpgStatOperation.Max:
                result = currentValue > operandValue ? currentValue : operandValue;
                return;

            default:
                result = currentValue;
                return;
        }
    }
}