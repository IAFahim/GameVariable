namespace Variable.Core;

/// <summary>
///     Represents a bounded numeric value constrained by a minimum and maximum range,
///     providing common queries for state inspection and normalization.
/// </summary>
/// <remarks>
///     This interface is implemented by all bounded types in the GameVariable ecosystem,
///     including <c>BoundedFloat</c>, <c>BoundedInt</c>, <c>Timer</c>, <c>Cooldown</c>, and similar constructs.
///     <para/>
///     Implementations are expected to guarantee that the current value always lies within
///     the inclusive range defined by <see cref="GetMin"/> and <see cref="GetMax"/>.
/// </remarks>
public interface IBoundedInfo
{
    /// <summary>
    ///     Gets the minimum bound of the range.
    /// </summary>
    /// <returns>
    ///     The inclusive lower limit of the bounded range.
    /// </returns>
    float GetMin();

    /// <summary>
    ///     Gets the current value within the bounded range.
    /// </summary>
    /// <returns>
    ///     The current value, clamped between <see cref="GetMin"/> and <see cref="GetMax"/>.
    /// </returns>
    float GetCurrent();

    /// <summary>
    ///     Gets the maximum bound of the range.
    /// </summary>
    /// <returns>
    ///     The inclusive upper limit of the bounded range.
    /// </returns>
    float GetMax();


    /// <summary>
    ///     Determines whether the current value has reached its maximum bound.
    /// </summary>
    /// <returns>
    ///     <c>true</c> if the current value is equal to <see cref="GetMax"/>; otherwise, <c>false</c>.
    /// </returns>
    bool IsFull();

    /// <summary>
    ///     Determines whether the current value has reached its minimum bound.
    /// </summary>
    /// <returns>
    ///     <c>true</c> if the current value is equal to <see cref="GetMin"/>; otherwise, <c>false</c>.
    /// </returns>
    bool IsEmpty();

    /// <summary>
    ///     Gets the normalized ratio of the current value within the bounded range.
    /// </summary>
    /// <returns>
    ///     A value between 0.0 and 1.0 representing how full the range is, where:
    ///     <list type="bullet">
    ///         <item><description>0.0 indicates the minimum bound</description></item>
    ///         <item><description>1.0 indicates the maximum bound</description></item>
    ///     </list>
    ///     Returns 0.0 if <see cref="GetMax"/> equals <see cref="GetMin"/> to avoid division by zero.
    /// </returns>
    double GetRatio();
}