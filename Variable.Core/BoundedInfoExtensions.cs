using System;
using System.Runtime.CompilerServices;

namespace Variable.Core;

/// <summary>
///     Extension methods for <see cref="IBoundedInfo"/> providing common logic operations.
/// </summary>
public static class BoundedInfoExtensions
{
    /// <summary>
    ///     Determines whether the current value has reached its maximum bound.
    /// </summary>
    /// <param name="bounded">The bounded value to check.</param>
    /// <param name="tolerance">Tolerance for floating point comparison. Defaults to <see cref="MathConstants.Tolerance"/>.</param>
    /// <returns>
    ///     <c>true</c> if the current value is equal to Max; otherwise, <c>false</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(this IBoundedInfo bounded, float tolerance = MathConstants.Tolerance)
    {
        return bounded.Current >= bounded.Max - tolerance;
    }

    /// <summary>
    ///     Determines whether the current value has reached its minimum bound.
    /// </summary>
    /// <param name="bounded">The bounded value to check.</param>
    /// <param name="tolerance">Tolerance for floating point comparison. Defaults to <see cref="MathConstants.Tolerance"/>.</param>
    /// <returns>
    ///     <c>true</c> if the current value is equal to Min; otherwise, <c>false</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(this IBoundedInfo bounded, float tolerance = MathConstants.Tolerance)
    {
        return bounded.Current <= bounded.Min + tolerance;
    }

    /// <summary>
    ///     Gets the normalized ratio of the current value within the bounded range.
    /// </summary>
    /// <param name="bounded">The bounded value to query.</param>
    /// <returns>
    ///     A value between 0.0 and 1.0 representing how full the range is, where:
    ///     <list type="bullet">
    ///         <item><description>0.0 indicates the minimum bound</description></item>
    ///         <item><description>1.0 indicates the maximum bound</description></item>
    ///     </list>
    ///     Returns 0.0 if Max equals Min to avoid division by zero.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetRatio(this IBoundedInfo bounded)
    {
        var range = bounded.Max - bounded.Min;
        return Math.Abs(range) < MathConstants.Tolerance ? 0.0 : (bounded.Current - bounded.Min) / range;
    }
}
