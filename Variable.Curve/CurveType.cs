namespace Variable.Curve;

/// <summary>
///     Defines the type of mathematical curve to evaluate.
/// </summary>
public enum CurveType : byte
{
    /// <summary>
    ///     Linear interpolation: y = Slope * x + Offset.
    ///     Simple and fast.
    /// </summary>
    Linear = 0,

    /// <summary>
    ///     Polynomial curve: y = Slope * pow(x, Exponent) + Offset.
    ///     Great for standard RPG leveling (square/cubic).
    /// </summary>
    Power = 1,

    /// <summary>
    ///     Exponential curve: y = Slope * pow(Exponent, x) + Offset.
    ///     Fast growth (inflation, population).
    /// </summary>
    Exponential = 2,

    /// <summary>
    ///     Logarithmic curve: y = Slope * log(x + 1) / log(Exponent) + Offset.
    ///     Diminishing returns (armor, efficiency).
    /// </summary>
    Logarithmic = 3,

    /// <summary>
    ///     Logistic (S-curve): y = Slope / (1 + pow(Math.E, -Exponent * (x - Intercept))) + Offset.
    ///     Natural growth with a cap (population, adoption).
    /// </summary>
    Logistic = 4,

    /// <summary>
    ///     Sine wave: y = Slope * sin(x * Exponent + Intercept) + Offset.
    ///     Oscillating values (day/night cycle, tides).
    /// </summary>
    Sine = 5
}
