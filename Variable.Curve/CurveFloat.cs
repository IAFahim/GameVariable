using System.Globalization;

namespace Variable.Curve;

/// <summary>
///     A zero-allocation struct that defines a mathematical curve.
///     Can represent Linear, Power, Exponential, Logarithmic, Logistic, and Sine curves.
/// </summary>
/// <remarks>
///     <para>
///         Designed for game mechanics like XP requirements, damage scaling,
///         diminishing returns, and procedural generation.
///     </para>
///     <para>
///         Use <see cref="CurveExtensions.Evaluate(in CurveFloat, float)" /> to calculate values.
///     </para>
/// </remarks>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct CurveFloat : IEquatable<CurveFloat>
{
    /// <summary>The type of curve algorithm to use.</summary>
    public CurveType Type;

    /// <summary>
    ///     Primary multiplier.
    ///     Linear: Slope (m).
    ///     Power: Multiplier (a).
    ///     Logistic: Max Value (L).
    ///     Sine: Amplitude (A).
    /// </summary>
    public float Slope;

    /// <summary>
    ///     Power or Rate.
    ///     Power: Exponent (b).
    ///     Exponential: Base (b).
    ///     Logarithmic: Base (b).
    ///     Logistic: Steepness (k).
    ///     Sine: Frequency (f).
    /// </summary>
    public float Exponent;

    /// <summary>
    ///     Vertical shift (y-intercept/baseline).
    ///     Linear: Intercept (c).
    ///     Logistic: Minimum value shift.
    /// </summary>
    public float Offset;

    /// <summary>
    ///     Horizontal shift or Phase.
    ///     Logistic: Midpoint (x0).
    ///     Sine: Phase shift (phi).
    /// </summary>
    public float Intercept;

    /// <summary>
    ///     Creates a Linear curve: y = slope * x + offset.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CurveFloat Linear(float slope, float offset = 0f)
    {
        return new CurveFloat
        {
            Type = CurveType.Linear,
            Slope = slope,
            Offset = offset,
            Exponent = 1f,
            Intercept = 0f
        };
    }

    /// <summary>
    ///     Creates a Power curve: y = slope * pow(x, exponent) + offset.
    ///     Good for RPG leveling (e.g., exponent = 2 for quadratic).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CurveFloat Power(float slope, float exponent, float offset = 0f)
    {
        return new CurveFloat
        {
            Type = CurveType.Power,
            Slope = slope,
            Exponent = exponent,
            Offset = offset,
            Intercept = 0f
        };
    }

    /// <summary>
    ///     Creates an Exponential curve: y = slope * pow(base, x) + offset.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CurveFloat Exponential(float slope, float baseValue, float offset = 0f)
    {
        return new CurveFloat
        {
            Type = CurveType.Exponential,
            Slope = slope,
            Exponent = baseValue,
            Offset = offset,
            Intercept = 0f
        };
    }

    /// <summary>
    ///     Creates a Logarithmic curve: y = slope * log_base(x + 1) + offset.
    ///     Good for diminishing returns.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CurveFloat Logarithmic(float slope, float baseValue, float offset = 0f)
    {
        return new CurveFloat
        {
            Type = CurveType.Logarithmic,
            Slope = slope,
            Exponent = baseValue,
            Offset = offset,
            Intercept = 0f
        };
    }

    /// <summary>
    ///     Creates a Logistic (S-curve): y = slope / (1 + exp(-steepness * (x - midpoint))) + offset.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CurveFloat Logistic(float maxValue, float steepness, float midpoint, float offset = 0f)
    {
        return new CurveFloat
        {
            Type = CurveType.Logistic,
            Slope = maxValue,
            Exponent = steepness,
            Intercept = midpoint,
            Offset = offset
        };
    }

    /// <summary>
    ///     Creates a Sine wave: y = amplitude * sin(x * frequency + phase) + offset.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CurveFloat Sine(float amplitude, float frequency, float phase = 0f, float offset = 0f)
    {
        return new CurveFloat
        {
            Type = CurveType.Sine,
            Slope = amplitude,
            Exponent = frequency,
            Intercept = phase,
            Offset = offset
        };
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is CurveFloat other && Equals(other);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(CurveFloat other)
    {
        return Type == other.Type &&
               Slope.Equals(other.Slope) &&
               Exponent.Equals(other.Exponent) &&
               Offset.Equals(other.Offset) &&
               Intercept.Equals(other.Intercept);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine((int)Type, Slope, Exponent, Offset, Intercept);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Type switch
        {
            CurveType.Linear => string.Format(CultureInfo.InvariantCulture, "Linear(y={0:0.##}x+{1:0.##})", Slope, Offset),
            CurveType.Power => string.Format(CultureInfo.InvariantCulture, "Power(y={0:0.##}x^{1:0.##}+{2:0.##})", Slope, Exponent, Offset),
            _ => string.Format(CultureInfo.InvariantCulture, "{0} Curve", Type)
        };
    }

    /// <summary>
    ///     Compares two CurveFloat instances for equality.
    /// </summary>
    public static bool operator ==(CurveFloat left, CurveFloat right)
    {
        return left.Equals(right);
    }

    /// <summary>
    ///     Compares two CurveFloat instances for inequality.
    /// </summary>
    public static bool operator !=(CurveFloat left, CurveFloat right)
    {
        return !left.Equals(right);
    }
}
