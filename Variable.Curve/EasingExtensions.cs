using System.Runtime.CompilerServices;

namespace Variable.Curve;

/// <summary>
///     Extension methods for applying easing functions to float values.
/// </summary>
public static class EasingExtensions
{
    /// <summary>
    ///     Applies the specified easing function to the normalized time value.
    /// </summary>
    /// <param name="t">The normalized time (usually 0 to 1).</param>
    /// <param name="type">The type of easing to apply.</param>
    /// <returns>The eased value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Ease(this float t, EaseType type)
    {
        CurveLogic.Evaluate(in t, in type, out float result);
        return result;
    }
}
