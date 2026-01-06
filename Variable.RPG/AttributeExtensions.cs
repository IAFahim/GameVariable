namespace Variable.RPG;

/// <summary>
///     Extension methods for Attribute struct.
///     Adapts the struct to the primitive-only AttributeLogic.
/// </summary>
public static class AttributeExtensions
{
    /// <summary>
    ///     Recalculates the cached value of the attribute based on base value and modifiers.
    /// </summary>
    /// <param name="attr">The attribute to recalculate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Recalculate(ref this Attribute attr)
    {
        AttributeLogic.Recalculate(
            attr.Base,
            attr.ModAdd,
            attr.ModMult,
            attr.Min,
            attr.Max,
            out attr.Value
        );
    }

    /// <summary>
    ///     Adds a modifier to the attribute.
    /// </summary>
    /// <param name="attr">The attribute to modify.</param>
    /// <param name="flat">The flat amount to add.</param>
    /// <param name="percent">The percentage to add (1.0 = 100%).</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddModifier(ref this Attribute attr, float flat, float percent)
    {
        AttributeLogic.AddModifier(
            ref attr.ModAdd,
            ref attr.ModMult,
            flat,
            percent
        );
    }

    /// <summary>
    ///     Clears all modifiers from the attribute.
    /// </summary>
    /// <param name="attr">The attribute to clear modifiers from.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ClearModifiers(ref this Attribute attr)
    {
        AttributeLogic.ClearModifiers(
            out attr.ModAdd,
            out attr.ModMult
        );
    }

    /// <summary>
    ///     Gets the current calculated value of the attribute.
    /// </summary>
    /// <param name="attr">The attribute to get the value from.</param>
    /// <returns>The calculated value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetValue(ref this Attribute attr)
    {
        return AttributeLogic.GetValueRecalculated(
            ref attr.Value,
            attr.Base,
            attr.ModAdd,
            attr.ModMult,
            attr.Min,
            attr.Max
        );
    }
}