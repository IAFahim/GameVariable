namespace Variable.RPG;

/// <summary>
///     Pure logic for recalculating attributes.
///     All methods operate on primitives only - NO STRUCTS in parameters.
/// </summary>
public static class AttributeLogic
{
    /// <summary>
    ///     Recalculates the CachedValue if Dirty.
    ///     Formula: (Base + Add) * Mult, clamped to [Min, Max].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Recalculate(float baseVal, float modAdd, float modMult, float min, float max,
        out float cachedValue, out byte isDirty)
    {
        var val = (baseVal + modAdd) * modMult;

        if (val > max) val = max;
        else if (val < min) val = min;

        cachedValue = val;
        isDirty = 0;
    }

    /// <summary>
    ///     Adds a temporary modifier to the attribute.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddModifier(ref float modAdd, ref float modMult, ref byte isDirty, float flat, float percent)
    {
        modAdd += flat;
        modMult += percent;
        isDirty = 1;
    }

    /// <summary>
    ///     Resets modifiers to default (Add=0, Mult=1).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ClearModifiers(out float modAdd, out float modMult, out byte isDirty)
    {
        modAdd = 0f;
        modMult = 1f;
        isDirty = 1;
    }

    /// <summary>
    ///     Gets the value, recalculating immediately if dirty.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetValue(ref float cachedValue, ref byte isDirty, float baseVal, float modAdd, float modMult,
        float min, float max)
    {
        if (isDirty != 0) Recalculate(baseVal, modAdd, modMult, min, max, out cachedValue, out isDirty);
        return cachedValue;
    }
}