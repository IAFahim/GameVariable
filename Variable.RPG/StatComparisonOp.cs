namespace Variable.RPG;

/// <summary>
///     Defines the mathematical comparison operation to perform on stat values.
///     Used by the condition system to evaluate whether a stat meets specific criteria.
/// </summary>
public enum StatComparisonOp
{
    /// <summary>Value == Target (Exact equality match).</summary>
    Equal = 0,

    /// <summary>Value != Target (Not equal).</summary>
    NotEqual = 1,

    /// <summary>Value &gt; Target (Strictly greater than).</summary>
    GreaterThan = 2,

    /// <summary>Value &gt;= Target (Greater than or equal).</summary>
    GreaterOrEqual = 3,

    /// <summary>Value &lt; Target (Strictly less than).</summary>
    LessThan = 4,

    /// <summary>Value &lt;= Target (Less than or equal).</summary>
    LessOrEqual = 5
}

/// <summary>
///     Extension methods for StatComparisonOp.
/// </summary>
public static class StatComparisonOpExtensions
{
    /// <summary>
    ///     Gets a human-readable symbol for the comparison operation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetSymbol(this StatComparisonOp operation)
    {
        return operation switch
        {
            StatComparisonOp.Equal => "==",
            StatComparisonOp.NotEqual => "!=",
            StatComparisonOp.GreaterThan => ">",
            StatComparisonOp.GreaterOrEqual => ">=",
            StatComparisonOp.LessThan => "<",
            StatComparisonOp.LessOrEqual => "<=",
            _ => "?"
        };
    }

    /// <summary>
    ///     Gets a descriptive name for the comparison operation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetDisplayName(this StatComparisonOp operation)
    {
        return operation switch
        {
            StatComparisonOp.Equal => "Equals",
            StatComparisonOp.NotEqual => "Not Equals",
            StatComparisonOp.GreaterThan => "Greater Than",
            StatComparisonOp.GreaterOrEqual => "Greater or Equal",
            StatComparisonOp.LessThan => "Less Than",
            StatComparisonOp.LessOrEqual => "Less or Equal",
            _ => "Unknown"
        };
    }
}
