namespace Variable.RPG;

/// <summary>
///     Extension methods for RPGStat struct.
///     Adapts the struct to the primitive-only RpgStatLogic.
/// </summary>
public static class RpgStatExtensions
{
    /// <summary>
    ///     Recalculates the cached value of the attribute based on base value and modifiers.
    /// </summary>
    /// <param name="attr">The attribute to recalculate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Recalculate(ref this RpgStat attr)
    {
        RpgStatLogic.Recalculate(
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
    public static void AddModifier(ref this RpgStat attr, float flat, float percent)
    {
        RpgStatLogic.AddModifier(
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
    public static void ClearModifiers(ref this RpgStat attr)
    {
        RpgStatLogic.ClearModifiers(
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
    public static float GetValue(ref this RpgStat attr)
    {
        return RpgStatLogic.GetValueRecalculated(
            ref attr.Value,
            attr.Base,
            attr.ModAdd,
            attr.ModMult,
            attr.Min,
            attr.Max
        );
    }

    /// <summary>
    /// Returns a compact equation string: "Value ◀ [Base + Add] × Mult"
    /// Example: "165 ◀ [100 + 50] × 110%"
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToStringCompact(ref this RpgStat s)
    {
        // F1 = 1 decimal place (10.5)
        // P0 = Percentage format (1.1 -> 110%)
        // ◀  = Visual separator indicating "Derived from"

        return $"{s.Value:F1} ◀ [{s.Base:F1} + {s.ModAdd:F1}] × {s.ModMult:P0}";
    }

    /// <summary>
    /// Returns a verbose string including bounds, useful for deep debugging.
    /// Example: "165 ◀ [100 + 50] × 1.1 :: Bounds(0, 999)"
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToString(ref this RpgStat s)
    {
        return $"{s.Value:F2} ◀ [{s.Base:F2} + {s.ModAdd:F2}] × {s.ModMult:F2} :: Bounds({s.Min}, {s.Max})";
    }
}