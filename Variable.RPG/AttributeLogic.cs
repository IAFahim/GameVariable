namespace Variable.RPG;

/// <summary>
///     Pure logic for recalculating attributes.
///     All methods operate on primitives only - NO STRUCTS in parameters.
/// </summary>
public static class AttributeLogic
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
}