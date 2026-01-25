using System;
using System.Runtime.CompilerServices;

namespace Variable.Curve;

/// <summary>
///     Stateless core logic for evaluating easing curves.
///     Implements Robert Penner's easing equations.
/// </summary>
public static class CurveLogic
{
    private const float PI = (float)Math.PI;
    private const float HalfPI = (float)(Math.PI / 2.0);
    private const float C1 = 1.70158f;
    private const float C2 = C1 * 1.525f;
    private const float C3 = C1 + 1f;
    private const float C4 = (float)(2 * Math.PI / 3);
    private const float C5 = (float)(2 * Math.PI / 4.5);

    /// <summary>
    ///     Evaluates the specified easing curve at time t.
    /// </summary>
    /// <param name="t">The normalized time (usually 0 to 1).</param>
    /// <param name="type">The type of easing to apply.</param>
    /// <param name="result">The calculated value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Evaluate(in float t, in EaseType type, out float result)
    {
        switch (type)
        {
            case EaseType.Linear:
                Linear(in t, out result);
                break;
            case EaseType.InQuad:
                InQuad(in t, out result);
                break;
            case EaseType.OutQuad:
                OutQuad(in t, out result);
                break;
            case EaseType.InOutQuad:
                InOutQuad(in t, out result);
                break;
            case EaseType.InCubic:
                InCubic(in t, out result);
                break;
            case EaseType.OutCubic:
                OutCubic(in t, out result);
                break;
            case EaseType.InOutCubic:
                InOutCubic(in t, out result);
                break;
            case EaseType.InQuart:
                InQuart(in t, out result);
                break;
            case EaseType.OutQuart:
                OutQuart(in t, out result);
                break;
            case EaseType.InOutQuart:
                InOutQuart(in t, out result);
                break;
            case EaseType.InQuint:
                InQuint(in t, out result);
                break;
            case EaseType.OutQuint:
                OutQuint(in t, out result);
                break;
            case EaseType.InOutQuint:
                InOutQuint(in t, out result);
                break;
            case EaseType.InSine:
                InSine(in t, out result);
                break;
            case EaseType.OutSine:
                OutSine(in t, out result);
                break;
            case EaseType.InOutSine:
                InOutSine(in t, out result);
                break;
            case EaseType.InExpo:
                InExpo(in t, out result);
                break;
            case EaseType.OutExpo:
                OutExpo(in t, out result);
                break;
            case EaseType.InOutExpo:
                InOutExpo(in t, out result);
                break;
            case EaseType.InCirc:
                InCirc(in t, out result);
                break;
            case EaseType.OutCirc:
                OutCirc(in t, out result);
                break;
            case EaseType.InOutCirc:
                InOutCirc(in t, out result);
                break;
            case EaseType.InBack:
                InBack(in t, out result);
                break;
            case EaseType.OutBack:
                OutBack(in t, out result);
                break;
            case EaseType.InOutBack:
                InOutBack(in t, out result);
                break;
            case EaseType.InElastic:
                InElastic(in t, out result);
                break;
            case EaseType.OutElastic:
                OutElastic(in t, out result);
                break;
            case EaseType.InOutElastic:
                InOutElastic(in t, out result);
                break;
            case EaseType.InBounce:
                InBounce(in t, out result);
                break;
            case EaseType.OutBounce:
                OutBounce(in t, out result);
                break;
            case EaseType.InOutBounce:
                InOutBounce(in t, out result);
                break;
            default:
                Linear(in t, out result);
                break;
        }
    }

    /// <summary>Applies Linear easing.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Linear(in float t, out float result)
    {
        result = t;
    }

    /// <summary>Applies Quadratic easing in.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InQuad(in float t, out float result)
    {
        result = t * t;
    }

    /// <summary>Applies Quadratic easing out.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutQuad(in float t, out float result)
    {
        result = 1 - (1 - t) * (1 - t);
    }

    /// <summary>Applies Quadratic easing in and out.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutQuad(in float t, out float result)
    {
        result = t < 0.5f ? 2 * t * t : 1 - (float)Math.Pow(-2 * t + 2, 2) / 2;
    }

    /// <summary>Applies Cubic easing in.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InCubic(in float t, out float result)
    {
        result = t * t * t;
    }

    /// <summary>Applies Cubic easing out.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutCubic(in float t, out float result)
    {
        result = 1 - (float)Math.Pow(1 - t, 3);
    }

    /// <summary>Applies Cubic easing in and out.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutCubic(in float t, out float result)
    {
        result = t < 0.5f ? 4 * t * t * t : 1 - (float)Math.Pow(-2 * t + 2, 3) / 2;
    }

    /// <summary>Applies Quartic easing in.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InQuart(in float t, out float result)
    {
        result = t * t * t * t;
    }

    /// <summary>Applies Quartic easing out.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutQuart(in float t, out float result)
    {
        result = 1 - (float)Math.Pow(1 - t, 4);
    }

    /// <summary>Applies Quartic easing in and out.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutQuart(in float t, out float result)
    {
        result = t < 0.5f ? 8 * t * t * t * t : 1 - (float)Math.Pow(-2 * t + 2, 4) / 2;
    }

    /// <summary>Applies Quintic easing in.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InQuint(in float t, out float result)
    {
        result = t * t * t * t * t;
    }

    /// <summary>Applies Quintic easing out.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutQuint(in float t, out float result)
    {
        result = 1 - (float)Math.Pow(1 - t, 5);
    }

    /// <summary>Applies Quintic easing in and out.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutQuint(in float t, out float result)
    {
        result = t < 0.5f ? 16 * t * t * t * t * t : 1 - (float)Math.Pow(-2 * t + 2, 5) / 2;
    }

    /// <summary>Applies Sinusoidal easing in.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InSine(in float t, out float result)
    {
        result = 1 - (float)Math.Cos(t * HalfPI);
    }

    /// <summary>Applies Sinusoidal easing out.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutSine(in float t, out float result)
    {
        result = (float)Math.Sin(t * HalfPI);
    }

    /// <summary>Applies Sinusoidal easing in and out.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutSine(in float t, out float result)
    {
        result = -(float)(Math.Cos(PI * t) - 1) / 2;
    }

    /// <summary>Applies Exponential easing in.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InExpo(in float t, out float result)
    {
        result = t == 0 ? 0 : (float)Math.Pow(2, 10 * t - 10);
    }

    /// <summary>Applies Exponential easing out.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutExpo(in float t, out float result)
    {
        result = t == 1 ? 1 : 1 - (float)Math.Pow(2, -10 * t);
    }

    /// <summary>Applies Exponential easing in and out.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutExpo(in float t, out float result)
    {
        if (t == 0)
        {
            result = 0;
            return;
        }

        if (t == 1)
        {
            result = 1;
            return;
        }

        result = t < 0.5f
            ? (float)Math.Pow(2, 20 * t - 10) / 2
            : (2 - (float)Math.Pow(2, -20 * t + 10)) / 2;
    }

    /// <summary>Applies Circular easing in.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InCirc(in float t, out float result)
    {
        result = 1 - (float)Math.Sqrt(1 - Math.Pow(t, 2));
    }

    /// <summary>Applies Circular easing out.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutCirc(in float t, out float result)
    {
        result = (float)Math.Sqrt(1 - Math.Pow(t - 1, 2));
    }

    /// <summary>Applies Circular easing in and out.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutCirc(in float t, out float result)
    {
        result = t < 0.5f
            ? (1 - (float)Math.Sqrt(1 - Math.Pow(2 * t, 2))) / 2
            : ((float)Math.Sqrt(1 - Math.Pow(-2 * t + 2, 2)) + 1) / 2;
    }

    /// <summary>Applies Back easing in.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InBack(in float t, out float result)
    {
        result = C3 * t * t * t - C1 * t * t;
    }

    /// <summary>Applies Back easing out.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutBack(in float t, out float result)
    {
        float t2 = t - 1;
        result = 1 + C3 * (float)Math.Pow(t2, 3) + C1 * (float)Math.Pow(t2, 2);
    }

    /// <summary>Applies Back easing in and out.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutBack(in float t, out float result)
    {
        result = t < 0.5f
            ? (float)(Math.Pow(2 * t, 2) * ((C2 + 1) * 2 * t - C2)) / 2
            : (float)(Math.Pow(2 * t - 2, 2) * ((C2 + 1) * (t * 2 - 2) + C2) + 2) / 2;
    }

    /// <summary>Applies Elastic easing in.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InElastic(in float t, out float result)
    {
        if (t == 0)
        {
            result = 0;
            return;
        }
        if (t == 1)
        {
            result = 1;
            return;
        }
        result = -(float)Math.Pow(2, 10 * t - 10) * (float)Math.Sin((t * 10 - 10.75) * C4);
    }

    /// <summary>Applies Elastic easing out.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutElastic(in float t, out float result)
    {
        if (t == 0)
        {
            result = 0;
            return;
        }
        if (t == 1)
        {
            result = 1;
            return;
        }
        result = (float)Math.Pow(2, -10 * t) * (float)Math.Sin((t * 10 - 0.75) * C4) + 1;
    }

    /// <summary>Applies Elastic easing in and out.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutElastic(in float t, out float result)
    {
        if (t == 0)
        {
            result = 0;
            return;
        }
        if (t == 1)
        {
            result = 1;
            return;
        }
        result = t < 0.5f
            ? -(float)(Math.Pow(2, 20 * t - 10) * Math.Sin((20 * t - 11.125) * C5)) / 2
            : (float)(Math.Pow(2, -20 * t + 10) * Math.Sin((20 * t - 11.125) * C5)) / 2 + 1;
    }

    /// <summary>Applies Bounce easing in.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InBounce(in float t, out float result)
    {
        OutBounce(1 - t, out float inverse);
        result = 1 - inverse;
    }

    /// <summary>Applies Bounce easing out.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void OutBounce(in float t, out float result)
    {
        float n1 = 7.5625f;
        float d1 = 2.75f;

        if (t < 1 / d1)
        {
            result = n1 * t * t;
        }
        else if (t < 2 / d1)
        {
            float t2 = t - 1.5f / d1;
            result = n1 * t2 * t2 + 0.75f;
        }
        else if (t < 2.5 / d1)
        {
            float t2 = t - 2.25f / d1;
            result = n1 * t2 * t2 + 0.9375f;
        }
        else
        {
            float t2 = t - 2.625f / d1;
            result = n1 * t2 * t2 + 0.984375f;
        }
    }

    /// <summary>Applies Bounce easing in and out.</summary>
    /// <param name="t">Time.</param>
    /// <param name="result">Result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InOutBounce(in float t, out float result)
    {
        if (t < 0.5f)
        {
            InBounce(t * 2, out float res);
            result = res * 0.5f;
        }
        else
        {
            OutBounce(t * 2 - 1, out float res);
            result = res * 0.5f + 0.5f;
        }
    }
}
