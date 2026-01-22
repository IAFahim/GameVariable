using System;

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
        RpgStatLogic.GetValueRecalculated(
            ref attr.Value,
            attr.Base,
            attr.ModAdd,
            attr.ModMult,
            attr.Min,
            attr.Max,
            out var result
        );
        return result;
    }

    /// <summary>
    ///     Returns a compact equation string: "Value ◀ [Base + Add] × Mult"
    ///     Example: "165 ◀ [100 + 50] × 110%"
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToStringCompact(ref this RpgStat stat)
    {
        // F1 = 1 decimal place (10.5)
        // P0 = Percentage format (1.1 -> 110%)
        // ◀  = Visual separator indicating "Derived from"

        // Estimate length: 4 floats (say 10 chars each) + fixed chars (~11) = ~51.
        // 128 is plenty safe.
        Span<char> buffer = stackalloc char[128];
        int pos = 0;

        // {0:F1}
        if (stat.Value.TryFormat(buffer.Slice(pos), out int charsWritten, "F1", CultureInfo.InvariantCulture))
        {
            pos += charsWritten;
        }

        // " ◀ ["
        " ◀ [".AsSpan().CopyTo(buffer.Slice(pos));
        pos += 4;

        // {1:F1}
        if (stat.Base.TryFormat(buffer.Slice(pos), out charsWritten, "F1", CultureInfo.InvariantCulture))
        {
            pos += charsWritten;
        }

        // " + "
        " + ".AsSpan().CopyTo(buffer.Slice(pos));
        pos += 3;

        // {2:F1}
        if (stat.ModAdd.TryFormat(buffer.Slice(pos), out charsWritten, "F1", CultureInfo.InvariantCulture))
        {
            pos += charsWritten;
        }

        // "] × "
        "] × ".AsSpan().CopyTo(buffer.Slice(pos));
        pos += 4;

        // {3:P0}
        if (stat.ModMult.TryFormat(buffer.Slice(pos), out charsWritten, "P0", CultureInfo.InvariantCulture))
        {
            pos += charsWritten;
        }

        return new string(buffer.Slice(0, pos));
    }

    /// <summary>
    ///     Returns a verbose string including bounds, useful for deep debugging.
    ///     Example: "165 ◀ [100 + 50] × 1.1 :: Bounds(0, 999)"
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToStringVerbose(ref this RpgStat stat)
    {
        return string.Format(CultureInfo.InvariantCulture,
            "{0:F2} ◀ [{1:F2} + {2:F2}] × {3:F2} :: Bounds({4}, {5})",
            stat.Value, stat.Base, stat.ModAdd, stat.ModMult, stat.Min, stat.Max);
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
    public static bool TrySetField(ref this RpgStat stat, RpgStatField field, float newValue,
        bool autoRecalculate = true)
    {
        var success = RpgStatLogic.TrySetField(
            field, newValue,
            ref stat.Base, ref stat.ModAdd, ref stat.ModMult,
            ref stat.Min, ref stat.Max, ref stat.Value
        );

        if (success && autoRecalculate && field != RpgStatField.Value) stat.Recalculate();

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
        RpgStatLogic.ApplyOperation(
            modifier.Operation,
            currentValue,
            modifier.Value,
            out var newValue,
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
    public static int ApplyModifiers(ref this RpgStat stat, ReadOnlySpan<RpgStatModifier> modifiers,
        bool autoRecalculate = true)
    {
        var appliedCount = 0;

        // Apply all modifiers without recalculation
        for (var i = 0; i < modifiers.Length; i++)
            if (stat.ApplyModifier(modifiers[i], false))
                appliedCount++;

        // Recalculate once at the end if requested
        if (autoRecalculate && appliedCount > 0)
            stat.Recalculate();

        return appliedCount;
    }

    /// <summary>
    ///     Removes a modifier by applying its inverse.
    ///     Example: If you added +5, this subtracts 5.
    ///     Only works for invertible operations (Add, Subtract, Multiply, Divide, percentages).
    /// </summary>
    /// <param name="stat">The stat to modify.</param>
    /// <param name="modifier">The original modifier to remove.</param>
    /// <param name="autoRecalculate">If true, recalculates after removal.</param>
    /// <returns>True if successfully removed, false if operation is not invertible.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool RemoveModifier(ref this RpgStat stat, RpgStatModifier modifier, bool autoRecalculate = true)
    {
        if (!modifier.TryGetInverse(out var inverse))
            return false;

        return stat.ApplyModifier(inverse, autoRecalculate);
    }

    /// <summary>
    ///     Removes multiple modifiers by applying their inverses.
    ///     Example: Remove all bonuses from unequipped item.
    /// </summary>
    /// <param name="stat">The stat to modify.</param>
    /// <param name="modifiers">Array of modifiers to remove.</param>
    /// <param name="autoRecalculate">If true, recalculates once after all removals.</param>
    /// <returns>Number of successfully removed modifiers.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int RemoveModifiers(ref this RpgStat stat, ReadOnlySpan<RpgStatModifier> modifiers,
        bool autoRecalculate = true)
    {
        var removedCount = 0;

        // Remove all modifiers without recalculation
        for (var i = 0; i < modifiers.Length; i++)
            if (stat.RemoveModifier(modifiers[i], false))
                removedCount++;

        // Recalculate once at the end if requested
        if (autoRecalculate && removedCount > 0)
            stat.Recalculate();

        return removedCount;
    }
}