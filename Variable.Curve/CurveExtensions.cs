namespace Variable.Curve;

/// <summary>
///     Extension methods for CurveFloat to make it user-friendly.
/// </summary>
public static class CurveExtensions
{
    /// <summary>
    ///     Evaluates the curve at the given input <paramref name="t"/>.
    /// </summary>
    /// <param name="curve">The curve definition.</param>
    /// <param name="t">The input value (e.g., Level, Time).</param>
    /// <returns>The calculated value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Evaluate(in this CurveFloat curve, float t)
    {
        CurveLogic.Evaluate(in curve, in t, out var result);
        return result;
    }

    /// <summary>
    ///     Evaluates the curve for an integer input (e.g., Level).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int EvaluateInt(in this CurveFloat curve, int t)
    {
        CurveLogic.Evaluate(in curve, (float)t, out var result);
        return (int)result;
    }
}
