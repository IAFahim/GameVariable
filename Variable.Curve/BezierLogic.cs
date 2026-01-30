namespace Variable.Curve;

/// <summary>
///     Pure logic for Bezier curve evaluation.
///     Provides methods to evaluate Quadratic (3 points) and Cubic (4 points) Bezier curves.
/// </summary>
public static class BezierLogic
{
    /// <summary>
    ///     Evaluates a Quadratic Bezier curve at time t.
    ///     Formula: (1-t)^2 * p0 + 2(1-t)t * p1 + t^2 * p2
    /// </summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="p0">Start point.</param>
    /// <param name="p1">Control point.</param>
    /// <param name="p2">End point.</param>
    /// <returns>The interpolated value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Evaluate(float t, float p0, float p1, float p2)
    {
        // t = Math.Clamp(t, 0f, 1f); // Should we clamp? Usually raw evaluation allows overshoot if t > 1.
        // Let's stick to raw math for Logic layer. Caller can clamp t if needed.

        float u = 1f - t;
        float tt = t * t;
        float uu = u * u;

        return uu * p0 + 2f * u * t * p1 + tt * p2;
    }

    /// <summary>
    ///     Evaluates a Cubic Bezier curve at time t.
    ///     Formula: (1-t)^3 * p0 + 3(1-t)^2 * t * p1 + 3(1-t) * t^2 * p2 + t^3 * p3
    /// </summary>
    /// <param name="t">Normalized time [0, 1].</param>
    /// <param name="p0">Start point.</param>
    /// <param name="p1">First control point.</param>
    /// <param name="p2">Second control point.</param>
    /// <param name="p3">End point.</param>
    /// <returns>The interpolated value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Evaluate(float t, float p0, float p1, float p2, float p3)
    {
        float u = 1f - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        return uuu * p0 + 3f * uu * t * p1 + 3f * u * tt * p2 + ttt * p3;
    }

    /// <summary>
    ///     Evaluates the first derivative (velocity) of a Quadratic Bezier curve at time t.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float EvaluateDerivative(float t, float p0, float p1, float p2)
    {
        return 2f * (1f - t) * (p1 - p0) + 2f * t * (p2 - p1);
    }

    /// <summary>
    ///     Evaluates the first derivative (velocity) of a Cubic Bezier curve at time t.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float EvaluateDerivative(float t, float p0, float p1, float p2, float p3)
    {
        float u = 1f - t;
        return 3f * u * u * (p1 - p0) + 6f * u * t * (p2 - p1) + 3f * t * t * (p3 - p2);
    }
}
