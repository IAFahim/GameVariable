namespace Variable.RPG;

/// <summary>
///     Defines the source of the reference value when comparing stat values.
///     Determines what the comparison target is calculated from.
/// </summary>
public enum StatReferenceSource
{
    /// <summary>
    ///     Compare against a hardcoded fixed value.
    ///     Example: "Is Health &gt; 50?" (compares to literal 50).
    /// </summary>
    FixedValue = 0,

    /// <summary>
    ///     Compare against a percentage of the stat's maximum value.
    ///     Example: "Is Health &lt; 20% of Max?" (compares to Max * 0.2).
    /// </summary>
    Max = 1,

    /// <summary>
    ///     Compare against a percentage of the stat's base value.
    ///     Example: "Is Current &gt; 150% of Base?" (compares to Base * 1.5).
    /// </summary>
    Base = 2,

    /// <summary>
    ///     Compare against a percentage of the stat's current value.
    ///     Example: "Is Base &lt; 80% of Current?" (compares to Value * 0.8).
    /// </summary>
    Value = 3
}

/// <summary>
///     Extension methods for StatReferenceSource.
/// </summary>
public static class StatReferenceSourceExtensions
{
    /// <summary>
    ///     Gets a descriptive name for the reference source.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetDisplayName(this StatReferenceSource source)
    {
        return source switch
        {
            StatReferenceSource.FixedValue => "Fixed Value",
            StatReferenceSource.Max => "Max",
            StatReferenceSource.Base => "Base",
            StatReferenceSource.Value => "Current Value",
            _ => "Unknown"
        };
    }

    /// <summary>
    ///     Checks if the reference source requires a percentage interpretation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPercentage(this StatReferenceSource source)
    {
        return source switch
        {
            StatReferenceSource.Max => true,
            StatReferenceSource.Base => true,
            StatReferenceSource.Value => true,
            _ => false
        };
    }
}
