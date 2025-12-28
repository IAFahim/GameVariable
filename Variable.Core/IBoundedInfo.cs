namespace Variable.Core;

/// <summary>
///     Represents a bounded value with a defined range, providing common operations
///     for checking fullness, emptiness, and calculating the ratio within the range.
/// </summary>
/// <remarks>
///     This interface is implemented by all bounded types in the GameVariable ecosystem,
///     including <c>BoundedFloat</c>, <c>BoundedInt</c>, <c>Timer</c>, <c>Cooldown</c>, etc.
/// </remarks>
public interface IBoundedInfo
{
    /// <summary>
    ///     Determines whether the current value has reached its maximum bound.
    /// </summary>
    /// <returns><c>true</c> if the current value equals the maximum bound; otherwise, <c>false</c>.</returns>
    bool IsFull();

    /// <summary>
    ///     Determines whether the current value has reached its minimum bound.
    /// </summary>
    /// <returns><c>true</c> if the current value equals the minimum bound; otherwise, <c>false</c>.</returns>
    bool IsEmpty();

    /// <summary>
    ///     Gets the ratio of the current value relative to the total range.
    /// </summary>
    /// <returns>
    ///     A value between 0.0 and 1.0 representing the percentage of the range filled.
    ///     Returns 0.0 if the range is zero to avoid division by zero.
    /// </returns>
    double GetRatio();
}