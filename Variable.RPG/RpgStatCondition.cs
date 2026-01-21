using System.Diagnostics;
using System.Text;

namespace Variable.RPG;

/// <summary>
///     Represents a query condition against an RPG Stat state.
///     Example: "Is Value &lt; 20% of Max?" or "Is Multiplier &gt; 5x?"
///     Data Layout: 16 bytes (fits in cache line, zero allocation when passed by reference).
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public struct RpgStatCondition : IEquatable<RpgStatCondition>
{
    /// <summary>
    ///     Which field of the stat to inspect (typically Value, but can be ModMult, Base, etc.).
    ///     This is the left-hand side of the comparison.
    /// </summary>
    public RpgStatField FieldToTest;

    /// <summary>
    ///     The mathematical comparison operation (Greater, Less, Equal, etc.).
    /// </summary>
    public StatComparisonOp Operator;

    /// <summary>
    ///     What are we comparing against? (Fixed number, Max, Base, Value).
    ///     This is the reference source for the right-hand side.
    /// </summary>
    public StatReferenceSource ReferenceSource;

    /// <summary>
    ///     The value or multiplier for the reference.
    ///     If ReferenceSource is FixedValue: This is the raw number (e.g., 50).
    ///     If ReferenceSource is Max/Base/Value: This is the percentage (e.g., 0.2 for 20%).
    /// </summary>
    public float ReferenceValue;

    /// <summary>
    ///     Private property for debugger display.
    /// </summary>
    private string DebuggerDisplay
    {
        get
        {
            var opSymbol = Operator switch
            {
                StatComparisonOp.Equal => "==",
                StatComparisonOp.NotEqual => "!=",
                StatComparisonOp.GreaterThan => ">",
                StatComparisonOp.GreaterOrEqual => ">=",
                StatComparisonOp.LessThan => "<",
                StatComparisonOp.LessOrEqual => "<=",
                _ => "?"
            };

            var refSource = ReferenceSource switch
            {
                StatReferenceSource.FixedValue => "Fixed",
                StatReferenceSource.Max => "Max",
                StatReferenceSource.Base => "Base",
                StatReferenceSource.Value => "Value",
                _ => "?"
            };

            return $"{FieldToTest} {opSymbol} {refSource} * {ReferenceValue:F2}";
        }
    }

    /// <summary>
    ///     Creates a "Percentage of Max" condition.
    ///     Use this for threshold checks like "Health &lt; 30%".
    /// </summary>
    /// <param name="operator">The comparison operation (e.g., LessThan).</param>
    /// <param name="percent">The percentage as a float (e.g., 0.3 for 30%).</param>
    /// <returns>A condition that tests Value against a percentage of Max.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RpgStatCondition PercentOfMax(StatComparisonOp @operator, float percent)
    {
        return new RpgStatCondition
        {
            FieldToTest = RpgStatField.Value,
            Operator = @operator,
            ReferenceSource = StatReferenceSource.Max,
            ReferenceValue = percent
        };
    }

    /// <summary>
    ///     Creates an absolute value condition.
    ///     Use this for fixed threshold checks like "Strength &gt;= 100".
    /// </summary>
    /// <param name="operator">The comparison operation.</param>
    /// <param name="value">The fixed value to compare against.</param>
    /// <returns>A condition that tests Value against a fixed number.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RpgStatCondition Absolute(StatComparisonOp @operator, float value)
    {
        return new RpgStatCondition
        {
            FieldToTest = RpgStatField.Value,
            Operator = @operator,
            ReferenceSource = StatReferenceSource.FixedValue,
            ReferenceValue = value
        };
    }

    /// <summary>
    ///     Creates a specific field condition.
    ///     Use this for complex checks like "Multiplier &gt; 5x" or "Base != Current".
    /// </summary>
    /// <param name="field">Which field to test (Base, ModAdd, ModMult, etc.).</param>
    /// <param name="operator">The comparison operation.</param>
    /// <param name="value">The value to compare against.</param>
    /// <returns>A condition that tests a specific field.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RpgStatCondition FieldCheck(RpgStatField field, StatComparisonOp @operator, float value)
    {
        return new RpgStatCondition
        {
            FieldToTest = field,
            Operator = @operator,
            ReferenceSource = StatReferenceSource.FixedValue,
            ReferenceValue = value
        };
    }

    /// <summary>
    ///     Creates a custom condition with full control over all parameters.
    /// </summary>
    /// <param name="fieldToTest">Which field to test.</param>
    /// <param name="operator">The comparison operation.</param>
    /// <param name="referenceSource">What to compare against.</param>
    /// <param name="referenceValue">The reference value or multiplier.</param>
    /// <returns>A fully customized condition.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RpgStatCondition Custom(
        RpgStatField fieldToTest,
        StatComparisonOp @operator,
        StatReferenceSource referenceSource,
        float referenceValue)
    {
        return new RpgStatCondition
        {
            FieldToTest = fieldToTest,
            Operator = @operator,
            ReferenceSource = referenceSource,
            ReferenceValue = referenceValue
        };
    }

    /// <summary>
    ///     Returns a human-readable string representation of this condition.
    ///     Useful for debugging and UI display.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        var sb = new StringBuilder(64);

        // Left side: Field name
        sb.Append(FieldToTest.ToString());
        sb.Append(' ');
        sb.Append(Operator.GetSymbol());
        sb.Append(' ');

        // Right side: Reference
        if (ReferenceSource == StatReferenceSource.FixedValue)
        {
            sb.Append(ReferenceValue.ToString("F2", CultureInfo.InvariantCulture));
        }
        else
        {
            var percent = ReferenceValue * 100f;
            sb.Append(ReferenceSource.GetDisplayName());
            sb.Append(" × ");
            sb.Append(percent.ToString("F1", CultureInfo.InvariantCulture));
            sb.Append('%');
        }

        return sb.ToString();
    }

    /// <summary>
    ///     Determines if two conditions are equal.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(RpgStatCondition other)
    {
        return FieldToTest == other.FieldToTest &&
               Operator == other.Operator &&
               ReferenceSource == other.ReferenceSource &&
               Math.Abs(ReferenceValue - other.ReferenceValue) < 0.0001f;
    }

    /// <summary>
    ///     Determines if two conditions are equal.
    /// </summary>
    public override bool Equals(object? obj)
    {
        return obj is RpgStatCondition other && Equals(other);
    }

    /// <summary>
    ///     Gets the hash code for this condition.
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine((int)FieldToTest, (int)Operator, (int)ReferenceSource, ReferenceValue);
    }

    /// <summary>
    ///     Equality operator.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(RpgStatCondition left, RpgStatCondition right)
    {
        return left.Equals(right);
    }

    /// <summary>
    ///     Inequality operator.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(RpgStatCondition left, RpgStatCondition right)
    {
        return !(left == right);
    }
}
