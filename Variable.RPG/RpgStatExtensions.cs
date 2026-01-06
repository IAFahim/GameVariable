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

    /// <summary>
    ///     Sets a specific field of the RpgStat.
    /// </summary>
    /// <param name="stat">The stat to modify.</param>
    /// <param name="field">Which field to modify.</param>
    /// <param name="newValue">The new value to set.</param>
    /// <param name="autoRecalculate">If true, recalculates Value after setting (default: true).</param>
    /// <returns>True if the field was valid and set, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TrySetField(ref this RpgStat stat, RpgStatField field, float newValue, bool autoRecalculate = true)
    {
        var success = RpgStatLogic.TrySetField(
            field, newValue,
            ref stat.Base, ref stat.ModAdd, ref stat.ModMult,
            ref stat.Min, ref stat.Max, ref stat.Value
        );

        if (success && autoRecalculate && field != RpgStatField.Value)
        {
            stat.Recalculate();
        }

        return success;
    }

    /// <summary>
    ///     Gets a specific field value from the RpgStat.
    /// </summary>
    /// <param name="stat">The stat to read from.</param>
    /// <param name="field">Which field to read.</param>
    /// <param name="value">The output value.</param>
    /// <returns>True if the field was valid, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGetField(ref this RpgStat stat, RpgStatField field, out float value)
    {
        return RpgStatLogic.TryGetField(
            field,
            stat.Base, stat.ModAdd, stat.ModMult,
            stat.Min, stat.Max, stat.Value,
            out value
        );
    }

    /// <summary>
    ///     Applies a modifier to the RpgStat.
    ///     Combines field selection, operation, and value in one call.
    /// </summary>
    /// <param name="stat">The stat to modify.</param>
    /// <param name="modifier">The modifier describing the operation.</param>
    /// <param name="autoRecalculate">If true, recalculates Value after modification.</param>
    /// <returns>True if the modification was successful, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApplyModifier(ref this RpgStat stat, RpgStatModifier modifier, bool autoRecalculate = true)
    {
        // Early exit: Invalid field
        if (!stat.TryGetField(modifier.Field, out var currentValue))
            return false;

        // Calculate new value using operation
        var newValue = RpgStatLogic.ApplyOperation(
            modifier.Operation,
            currentValue,
            modifier.Value,
            stat.Base
        );

        // Set the new value
        return stat.TrySetField(modifier.Field, newValue, autoRecalculate);
    }

    /// <summary>
    ///     Applies multiple modifiers in sequence.
    ///     Useful for equipment that grants multiple bonuses.
    /// </summary>
    /// <param name="stat">The stat to modify.</param>
    /// <param name="modifiers">Array of modifiers to apply.</param>
    /// <param name="autoRecalculate">If true, recalculates once after all modifications.</param>
    /// <returns>Number of successfully applied modifiers.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ApplyModifiers(ref this RpgStat stat, ReadOnlySpan<RpgStatModifier> modifiers, bool autoRecalculate = true)
    {
        var appliedCount = 0;

        // Apply all modifiers without recalculation
        for (var i = 0; i < modifiers.Length; i++)
        {
            if (stat.ApplyModifier(modifiers[i], autoRecalculate: false))
                appliedCount++;
        }

        // Recalculate once at the end if requested
        if (autoRecalculate && appliedCount > 0)
            stat.Recalculate();

        return appliedCount;
    }
}