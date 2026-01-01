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
    public static void Recalculate(ref Attribute attr)
    {
        var val = (attr.Base + attr.ModAdd) * attr.ModMult;

        if (val > attr.Max) val = attr.Max;
        else if (val < attr.Min) val = attr.Min;

        attr.CachedValue = val;
        attr.IsDirty = 0;
    }

    /// <summary>
    ///     Adds a temporary modifier to the attribute.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddModifier(ref Attribute attr, float flat, float percent)
    {
        attr.ModAdd += flat;
        attr.ModMult += percent;
        attr.IsDirty = 1;
    }

    /// <summary>
    ///     Resets modifiers to default (Add=0, Mult=1).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ClearModifiers(ref Attribute attr)
    {
        attr.ModAdd = 0f;
        attr.ModMult = 1f;
        attr.IsDirty = 1;
    }

    /// <summary>
    ///     Gets the value, recalculating immediately if dirty.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetValue(ref Attribute attr)
    {
        if (attr.IsDirty != 0) Recalculate(ref attr);
        return attr.CachedValue;
    }
}
