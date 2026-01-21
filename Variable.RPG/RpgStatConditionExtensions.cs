namespace Variable.RPG;

/// <summary>
///     Extension methods for evaluating conditions on RpgStat.
///     This is the developer-facing API - unpacks structs and calls the primitive-only logic layer.
///     Zero allocation, Burst-compatible.
/// </summary>
public static class RpgStatConditionExtensions
{
    /// <summary>
    ///     Evaluates a single condition against this stat.
    ///     This is the primary API for condition checking.
    ///     Example: if (health.Satisfies(lowHealthCondition)) { ... }
    /// </summary>
    /// <param name="stat">The stat to evaluate.</param>
    /// <param name="condition">The condition to check.</param>
    /// <returns>True if the condition is satisfied, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Satisfies(ref this RpgStat stat, in RpgStatCondition condition)
    {
        return RpgStatConditionLogic.Evaluate(
            in condition.FieldToTest,
            in condition.Operator,
            in condition.ReferenceSource,
            in condition.ReferenceValue,
            in stat.Base,
            in stat.ModAdd,
            in stat.ModMult,
            in stat.Min,
            in stat.Max,
            in stat.Value
        );
    }

    /// <summary>
    ///     Evaluates a list of conditions with AND logic.
    ///     Returns true only if ALL conditions are satisfied.
    ///     Example: "Str >= 10 AND Dex >= 10 AND Int >= 10"
    /// </summary>
    /// <param name="stat">The stat to evaluate.</param>
    /// <param name="conditions">Array of conditions to check.</param>
    /// <returns>True if all conditions are satisfied, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool SatisfiesAll(ref this RpgStat stat, ReadOnlySpan<RpgStatCondition> conditions)
    {
        return RpgStatConditionLogic.EvaluateAll(
            conditions,
            in stat.Base,
            in stat.ModAdd,
            in stat.ModMult,
            in stat.Min,
            in stat.Max,
            in stat.Value
        );
    }

    /// <summary>
    ///     Evaluates a list of conditions with OR logic.
    ///     Returns true if AT LEAST ONE condition is satisfied.
    ///     Example: "HP > 100 OR HP > 50% Max"
    /// </summary>
    /// <param name="stat">The stat to evaluate.</param>
    /// <param name="conditions">Array of conditions to check.</param>
    /// <returns>True if at least one condition is satisfied, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool SatisfiesAny(ref this RpgStat stat, ReadOnlySpan<RpgStatCondition> conditions)
    {
        return RpgStatConditionLogic.EvaluateAny(
            conditions,
            in stat.Base,
            in stat.ModAdd,
            in stat.ModMult,
            in stat.Min,
            in stat.Max,
            in stat.Value
        );
    }

    /// <summary>
    ///     Evaluates a list of conditions and returns a count of how many were satisfied.
    ///     Useful for progression systems (e.g., "Meet at least 3 of 5 requirements").
    /// </summary>
    /// <param name="stat">The stat to evaluate.</param>
    /// <param name="conditions">Array of conditions to check.</param>
    /// <returns>The number of conditions that were satisfied.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CountSatisfied(ref this RpgStat stat, ReadOnlySpan<RpgStatCondition> conditions)
    {
        return RpgStatConditionLogic.CountSatisfied(
            conditions,
            in stat.Base,
            in stat.ModAdd,
            in stat.ModMult,
            in stat.Min,
            in stat.Max,
            in stat.Value
        );
    }

    /// <summary>
    ///     Checks if a specific number of conditions are satisfied (exact match).
    /// </summary>
    /// <param name="stat">The stat to evaluate.</param>
    /// <param name="conditions">Array of conditions to check.</param>
    /// <param name="requiredCount">The exact number of conditions that must be satisfied.</param>
    /// <returns>True if exactly the required count of conditions are satisfied.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool SatisfiesCount(ref this RpgStat stat, ReadOnlySpan<RpgStatCondition> conditions, int requiredCount)
    {
        return stat.CountSatisfied(conditions) == requiredCount;
    }

    /// <summary>
    ///     Checks if at least a minimum number of conditions are satisfied.
    /// </summary>
    /// <param name="stat">The stat to evaluate.</param>
    /// <param name="conditions">Array of conditions to check.</param>
    /// <param name="minimumCount">The minimum number of conditions that must be satisfied.</param>
    /// <returns>True if at least the minimum count of conditions are satisfied.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool SatisfiesAtLeast(ref this RpgStat stat, ReadOnlySpan<RpgStatCondition> conditions, int minimumCount)
    {
        return stat.CountSatisfied(conditions) >= minimumCount;
    }

    #region Convenience Factory Methods

    /// <summary>
    ///     Creates a condition checking if Value is less than a percentage of Max.
    ///     Example: "Is Health below 30%?"
    /// </summary>
    /// <param name="stat">The stat (used for type inference).</param>
    /// <param name="percent">Percentage threshold (0.3 = 30%).</param>
    /// <returns>A configured condition.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RpgStatCondition BelowPercentOfMax(this ref RpgStat stat, float percent)
    {
        return RpgStatCondition.PercentOfMax(StatComparisonOp.LessThan, percent);
    }

    /// <summary>
    ///     Creates a condition checking if Value is greater than or equal to a percentage of Max.
    ///     Example: "Is Health above 90%?"
    /// </summary>
    /// <param name="stat">The stat (used for type inference).</param>
    /// <param name="percent">Percentage threshold (0.9 = 90%).</param>
    /// <returns>A configured condition.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RpgStatCondition AbovePercentOfMax(this ref RpgStat stat, float percent)
    {
        return RpgStatCondition.PercentOfMax(StatComparisonOp.GreaterOrEqual, percent);
    }

    /// <summary>
    ///     Creates a condition checking if Value is greater than or equal to a fixed value.
    ///     Example: "Is Strength at least 50?"
    /// </summary>
    /// <param name="stat">The stat (used for type inference).</param>
    /// <param name="value">The minimum value required.</param>
    /// <returns>A configured condition.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RpgStatCondition AtLeast(this ref RpgStat stat, float value)
    {
        return RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, value);
    }

    /// <summary>
    ///     Creates a condition checking if Value is less than or equal to a fixed value.
    ///     Example: "Is Health no more than 100?"
    /// </summary>
    /// <param name="stat">The stat (used for type inference).</param>
    /// <param name="value">The maximum value allowed.</param>
    /// <returns>A configured condition.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RpgStatCondition AtMost(this ref RpgStat stat, float value)
    {
        return RpgStatCondition.Absolute(StatComparisonOp.LessOrEqual, value);
    }

    /// <summary>
    ///     Creates a condition checking if ModMult is greater than a threshold.
    ///     Example: "Is the damage multiplier above 2x?"
    /// </summary>
    /// <param name="stat">The stat (used for type inference).</param>
    /// <param name="multiplier">The multiplier threshold.</param>
    /// <returns>A configured condition.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RpgStatCondition MultiplierAbove(this ref RpgStat stat, float multiplier)
    {
        return RpgStatCondition.FieldCheck(RpgStatField.ModMult, StatComparisonOp.GreaterThan, multiplier);
    }

    /// <summary>
    ///     Creates a condition checking if ModMult is less than a threshold.
    ///     Example: "Is the damage multiplier below 0.5x?"
    /// </summary>
    /// <param name="stat">The stat (used for type inference).</param>
    /// <param name="multiplier">The multiplier threshold.</param>
    /// <returns>A configured condition.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RpgStatCondition MultiplierBelow(this ref RpgStat stat, float multiplier)
    {
        return RpgStatCondition.FieldCheck(RpgStatField.ModMult, StatComparisonOp.LessThan, multiplier);
    }

    #endregion

    #region Inspector-Friendly Display Methods

    /// <summary>
    ///     Returns a description of how this stat compares to a condition.
    ///     Useful for tooltips and debug displays.
    /// </summary>
    /// <param name="stat">The stat to evaluate.</param>
    /// <param name="condition">The condition to check.</param>
    /// <returns>A description string like "Health (150) is greater than 50% of Max (200)."</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string DescribeCondition(ref this RpgStat stat, in RpgStatCondition condition)
    {
        if (!stat.TryGetField(condition.FieldToTest, out var current))
        {
            return $"Invalid field: {condition.FieldToTest}";
        }

        var target = CalculateTarget(stat, condition.ReferenceSource, condition.ReferenceValue);

        var satisfied = stat.Satisfies(in condition);

        return $"{condition.FieldToTest} ({current:F1}) {condition.Operator.GetSymbol()} {target:F1} [{(satisfied ? "PASS" : "FAIL")}]";
    }

    /// <summary>
    ///     Calculates the target value for a reference source.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float CalculateTarget(in RpgStat stat, StatReferenceSource source, float value)
    {
        return source switch
        {
            StatReferenceSource.FixedValue => value,
            StatReferenceSource.Max => stat.Max * value,
            StatReferenceSource.Base => stat.Base * value,
            StatReferenceSource.Value => stat.Value * value,
            _ => value
        };
    }

    #endregion
}
