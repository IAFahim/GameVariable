namespace Variable.Core;

/// <summary>
///     Represents a bounded numeric value constrained by a minimum and maximum range.
///     This interface is intended primarily for editor tooling and UI visualization.
/// </summary>
/// <remarks>
///     This interface is implemented by all bounded types in the GameVariable ecosystem,
///     including <c>BoundedFloat</c>, <c>BoundedInt</c>, <c>Timer</c>, <c>Cooldown</c>, and similar constructs.
///     <para />
///     Implementations are expected to guarantee that the current value always lies within
///     the inclusive range defined by <see cref="Min" /> and <see cref="Max" />.
///     <para />
///     For logic operations like IsFull(), IsEmpty(), and GetRatio(), use the extension methods
///     in <see cref="BoundedInfoExtensions" />.
/// </remarks>
public interface IBoundedInfo
{
    /// <summary>
    ///     Gets the minimum bound of the range.
    /// </summary>
    float Min { get; }

    /// <summary>
    ///     Gets the current value within the bounded range.
    /// </summary>
    float Current { get; }

    /// <summary>
    ///     Gets the maximum bound of the range.
    /// </summary>
    float Max { get; }
}