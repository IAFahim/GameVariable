namespace Variable.Curve;

/// <summary>
///     Core logic for evaluating mathematical curves.
///     Stateless, static, and Burst-compatible.
/// </summary>
public static class CurveLogic
{
    /// <summary>
    ///     Evaluates the curve at a given point t.
    /// </summary>
    /// <param name="curve">The curve definition.</param>
    /// <param name="t">The input value (e.g., Level, Time, Stat).</param>
    /// <param name="result">The calculated output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Evaluate(in CurveFloat curve, in float t, out float result)
    {
        switch (curve.Type)
        {
            case CurveType.Linear:
                // y = mx + c
                result = curve.Slope * t + curve.Offset;
                break;

            case CurveType.Power:
                // y = m * x^p + c
                result = curve.Slope * MathF.Pow(t, curve.Exponent) + curve.Offset;
                break;

            case CurveType.Exponential:
                // y = m * b^x + c
                result = curve.Slope * MathF.Pow(curve.Exponent, t) + curve.Offset;
                break;

            case CurveType.Logarithmic:
                // y = m * log_b(x + 1) + c
                // Note: We use log(x+1) to safely handle 0 input.
                // Log base b of x = Log(x) / Log(b)
                if (t <= -1f)
                {
                    result = curve.Offset; // Undefined for log(<=0)
                    return;
                }
                var logBase = MathF.Log(curve.Exponent);
                if (MathF.Abs(logBase) < 0.000001f) // Avoid divide by zero if base is 1
                {
                     result = curve.Offset;
                     return;
                }
                result = curve.Slope * (MathF.Log(t + 1f) / logBase) + curve.Offset;
                break;

            case CurveType.Logistic:
                // y = L / (1 + e^(-k(x - x0))) + c
                // L = Slope, k = Exponent, x0 = Intercept
                var k = curve.Exponent;
                var x0 = curve.Intercept;
                var exponentVal = -k * (t - x0);

                // Optimization: If exponent is too large positive, denominator explodes -> 0
                // If exponent is too large negative, denominator -> 1
                if (exponentVal > 50f)
                {
                     result = curve.Offset; // Denom huge, fraction 0
                     return;
                }

                var denom = 1f + MathF.Exp(exponentVal);
                result = (curve.Slope / denom) + curve.Offset;
                break;

            case CurveType.Sine:
                // y = A * sin(x * f + p) + c
                // A = Slope, f = Exponent, p = Intercept
                result = curve.Slope * MathF.Sin(t * curve.Exponent + curve.Intercept) + curve.Offset;
                break;

            default:
                result = 0f;
                break;
        }
    }
}
