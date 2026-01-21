namespace Variable.RPG;

/// <summary>
///     Pure logic for evaluating stat conditions.
///     All methods are stateless and operate on primitive values only.
///     Zero allocation, Burst-compatible.
/// </summary>
public static class RpgStatConditionLogic
{
    /// <summary>
    ///     Evaluates a condition against a set of stat fields.
    ///     This is the core evaluation method - decomposes the condition and performs the comparison.
    /// </summary>
    /// <param name="fieldToTest">Which field to inspect (left-hand side).</param>
    /// <param name="op">The comparison operation.</param>
    /// <param name="refSource">The reference source for the target value.</param>
    /// <param name="refValue">The reference value or percentage multiplier.</param>
    /// <param name="statBase">The stat's Base value.</param>
    /// <param name="statModAdd">The stat's ModAdd value.</param>
    /// <param name="statModMult">The stat's ModMult value.</param>
    /// <param name="statMin">The stat's Min value.</param>
    /// <param name="statMax">The stat's Max value.</param>
    /// <param name="statValue">The stat's Value (calculated result).</param>
    /// <returns>True if the condition is satisfied, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Evaluate(
        // Condition parameters
        in RpgStatField fieldToTest,
        in StatComparisonOp op,
        in StatReferenceSource refSource,
        in float refValue,
        // Stat data
        in float statBase,
        in float statModAdd,
        in float statModMult,
        in float statMin,
        in float statMax,
        in float statValue)
    {
        // Step 1: Extract the value we are testing (Left Hand Side)
        if (!TryGetLeftSide(fieldToTest, in statBase, in statModAdd, in statModMult, in statMin, in statMax, in statValue, out var leftSide))
        {
            // Invalid field queries always fail
            return false;
        }

        // Step 2: Calculate the target value we are comparing against (Right Hand Side)
        CalculateRightSide(refSource, refValue, in statBase, in statMax, in statValue, out var rightSide);

        // Step 3: Perform the comparison
        return CompareValues(leftSide, rightSide, op);
    }

    /// <summary>
    ///     Extracts the left-hand side value from the stat based on the field selector.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool TryGetLeftSide(
        in RpgStatField field,
        in float statBase,
        in float statModAdd,
        in float statModMult,
        in float statMin,
        in float statMax,
        in float statValue,
        out float result)
    {
        // Delegate to the existing RpgStatLogic method for consistency
        return RpgStatLogic.TryGetField(field, in statBase, in statModAdd, in statModMult, in statMin, in statMax, in statValue, out result);
    }

    /// <summary>
    ///     Calculates the right-hand side target value based on the reference source.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CalculateRightSide(
        in StatReferenceSource refSource,
        in float refValue,
        in float statBase,
        in float statMax,
        in float statValue,
        out float result)
    {
        switch (refSource)
        {
            case StatReferenceSource.FixedValue:
                // Direct comparison: compare to the raw reference value
                result = refValue;
                return;

            case StatReferenceSource.Max:
                // Percentage of max: e.g., Max * 0.2 for 20%
                result = statMax * refValue;
                return;

            case StatReferenceSource.Base:
                // Percentage of base: e.g., Base * 0.5 for 50%
                result = statBase * refValue;
                return;

            case StatReferenceSource.Value:
                // Percentage of current value: e.g., Value * 1.5 for 150%
                result = statValue * refValue;
                return;

            default:
                // Fallback: treat as fixed value
                result = refValue;
                return;
        }
    }

    /// <summary>
    ///     Performs the actual comparison between two values.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool CompareValues(float leftSide, float rightSide, in StatComparisonOp op)
    {
        const float epsilon = 0.00001f;

        return op switch
        {
            StatComparisonOp.Equal => Math.Abs(leftSide - rightSide) < epsilon,
            StatComparisonOp.NotEqual => Math.Abs(leftSide - rightSide) >= epsilon,
            StatComparisonOp.GreaterThan => leftSide > rightSide,
            StatComparisonOp.GreaterOrEqual => leftSide >= rightSide,
            StatComparisonOp.LessThan => leftSide < rightSide,
            StatComparisonOp.LessOrEqual => leftSide <= rightSide,
            _ => false
        };
    }

    /// <summary>
    ///     Evaluates multiple conditions with AND logic.
    ///     Returns true only if ALL conditions are satisfied.
    /// </summary>
    /// <param name="conditions">Span of conditions to evaluate.</param>
    /// <param name="statBase">The stat's Base value.</param>
    /// <param name="statModAdd">The stat's ModAdd value.</param>
    /// <param name="statModMult">The stat's ModMult value.</param>
    /// <param name="statMin">The stat's Min value.</param>
    /// <param name="statMax">The stat's Max value.</param>
    /// <param name="statValue">The stat's Value (calculated result).</param>
    /// <returns>True if all conditions are satisfied, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EvaluateAll(
        ReadOnlySpan<RpgStatCondition> conditions,
        in float statBase,
        in float statModAdd,
        in float statModMult,
        in float statMin,
        in float statMax,
        in float statValue)
    {
        for (var i = 0; i < conditions.Length; i++)
        {
            ref readonly var condition = ref conditions[i];

            if (!Evaluate(
                    in condition.FieldToTest,
                    in condition.Operator,
                    in condition.ReferenceSource,
                    in condition.ReferenceValue,
                    in statBase,
                    in statModAdd,
                    in statModMult,
                    in statMin,
                    in statMax,
                    in statValue))
            {
                return false; // Early exit on first failure
            }
        }

        return true; // All conditions passed
    }

    /// <summary>
    ///     Evaluates multiple conditions with OR logic.
    ///     Returns true if AT LEAST ONE condition is satisfied.
    /// </summary>
    /// <param name="conditions">Span of conditions to evaluate.</param>
    /// <param name="statBase">The stat's Base value.</param>
    /// <param name="statModAdd">The stat's ModAdd value.</param>
    /// <param name="statModMult">The stat's ModMult value.</param>
    /// <param name="statMin">The stat's Min value.</param>
    /// <param name="statMax">The stat's Max value.</param>
    /// <param name="statValue">The stat's Value (calculated result).</param>
    /// <returns>True if at least one condition is satisfied, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EvaluateAny(
        ReadOnlySpan<RpgStatCondition> conditions,
        in float statBase,
        in float statModAdd,
        in float statModMult,
        in float statMin,
        in float statMax,
        in float statValue)
    {
        if (conditions.Length == 0)
            return false;

        for (var i = 0; i < conditions.Length; i++)
        {
            ref readonly var condition = ref conditions[i];

            if (Evaluate(
                    in condition.FieldToTest,
                    in condition.Operator,
                    in condition.ReferenceSource,
                    in condition.ReferenceValue,
                    in statBase,
                    in statModAdd,
                    in statModMult,
                    in statMin,
                    in statMax,
                    in statValue))
            {
                return true; // Early exit on first success
            }
        }

        return false; // No condition passed
    }

    /// <summary>
    ///     Counts how many conditions are satisfied.
    ///     Useful for progression systems (e.g., "Meet at least 3 of these requirements").
    /// </summary>
    /// <param name="conditions">Span of conditions to evaluate.</param>
    /// <param name="statBase">The stat's Base value.</param>
    /// <param name="statModAdd">The stat's ModAdd value.</param>
    /// <param name="statModMult">The stat's ModMult value.</param>
    /// <param name="statMin">The stat's Min value.</param>
    /// <param name="statMax">The stat's Max value.</param>
    /// <param name="statValue">The stat's Value (calculated result).</param>
    /// <returns>The number of conditions that were satisfied.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CountSatisfied(
        ReadOnlySpan<RpgStatCondition> conditions,
        in float statBase,
        in float statModAdd,
        in float statModMult,
        in float statMin,
        in float statMax,
        in float statValue)
    {
        var count = 0;

        for (var i = 0; i < conditions.Length; i++)
        {
            ref readonly var condition = ref conditions[i];

            if (Evaluate(
                    in condition.FieldToTest,
                    in condition.Operator,
                    in condition.ReferenceSource,
                    in condition.ReferenceValue,
                    in statBase,
                    in statModAdd,
                    in statModMult,
                    in statMin,
                    in statMax,
                    in statValue))
            {
                count++;
            }
        }

        return count;
    }
}
