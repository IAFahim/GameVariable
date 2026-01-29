namespace Variable.Curve;

/// <summary>
///     Pure logic for Bezier curve evaluation.
/// </summary>
public static class BezierLogic
{
    /// <summary>Evaluates a 1D Cubic Bezier curve at a specific progress point.</summary>
    /// <param name="start">The start point (P0).</param>
    /// <param name="control1">The first control point (P1).</param>
    /// <param name="control2">The second control point (P2).</param>
    /// <param name="end">The end point (P3).</param>
    /// <param name="progress">The progress along the curve (0-1).</param>
    /// <param name="result">The interpolated value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Evaluate(in float start, in float control1, in float control2, in float end, in float progress, out float result)
    {
        float t = progress;
        float u = 1f - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        result = (uuu * start) + (3f * uu * t * control1) + (3f * u * tt * control2) + (ttt * end);
    }
}
