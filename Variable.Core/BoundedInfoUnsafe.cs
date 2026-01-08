using System;
using System.Runtime.CompilerServices;

namespace Variable.Core;

/// <summary>
///     Unsafe (non-boxing) extension methods for <see cref="IBoundedInfo" /> providing common logic operations.
/// </summary>
public static class BoundedInfoUnsafe
{
    /// <summary>
    ///     Determines whether the current value has reached its maximum bound (non-boxing).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull<T>(this T bounded, float tolerance = MathConstants.Tolerance) where T : struct, IBoundedInfo
    {
        return bounded.Current >= bounded.Max - tolerance;
    }

    /// <summary>
    ///     Determines whether the current value has reached its minimum bound (non-boxing).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty<T>(this T bounded, float tolerance = MathConstants.Tolerance) where T : struct, IBoundedInfo
    {
        return bounded.Current <= bounded.Min + tolerance;
    }

    /// <summary>
    ///     Gets the normalized ratio of the current value within the bounded range (non-boxing).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetRatio<T>(this T bounded) where T : struct, IBoundedInfo
    {
        var range = bounded.Max - bounded.Min;
        return Math.Abs(range) < MathConstants.Tolerance ? 0.0 : (bounded.Current - bounded.Min) / range;
    }
}
