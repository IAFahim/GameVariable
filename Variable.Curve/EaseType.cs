namespace Variable.Curve;

/// <summary>
///     Defines the mathematical easing function to apply to a value over time.
/// </summary>
public enum EaseType
{
    /// <summary>Linear interpolation (no easing).</summary>
    Linear,

    /// <summary>Quadratic easing in (t^2).</summary>
    InQuad,

    /// <summary>Quadratic easing out.</summary>
    OutQuad,

    /// <summary>Quadratic easing in and out.</summary>
    InOutQuad,

    /// <summary>Cubic easing in (t^3).</summary>
    InCubic,

    /// <summary>Cubic easing out.</summary>
    OutCubic,

    /// <summary>Cubic easing in and out.</summary>
    InOutCubic,

    /// <summary>Quartic easing in (t^4).</summary>
    InQuart,

    /// <summary>Quartic easing out.</summary>
    OutQuart,

    /// <summary>Quartic easing in and out.</summary>
    InOutQuart,

    /// <summary>Quintic easing in (t^5).</summary>
    InQuint,

    /// <summary>Quintic easing out.</summary>
    OutQuint,

    /// <summary>Quintic easing in and out.</summary>
    InOutQuint,

    /// <summary>Sinusoidal easing in.</summary>
    InSine,

    /// <summary>Sinusoidal easing out.</summary>
    OutSine,

    /// <summary>Sinusoidal easing in and out.</summary>
    InOutSine,

    /// <summary>Exponential easing in (2^10(t-1)).</summary>
    InExpo,

    /// <summary>Exponential easing out.</summary>
    OutExpo,

    /// <summary>Exponential easing in and out.</summary>
    InOutExpo,

    /// <summary>Circular easing in.</summary>
    InCirc,

    /// <summary>Circular easing out.</summary>
    OutCirc,

    /// <summary>Circular easing in and out.</summary>
    InOutCirc,

    /// <summary>Elastic easing in.</summary>
    InElastic,

    /// <summary>Elastic easing out.</summary>
    OutElastic,

    /// <summary>Elastic easing in and out.</summary>
    InOutElastic,

    /// <summary>Back easing in.</summary>
    InBack,

    /// <summary>Back easing out.</summary>
    OutBack,

    /// <summary>Back easing in and out.</summary>
    InOutBack,

    /// <summary>Bounce easing in.</summary>
    InBounce,

    /// <summary>Bounce easing out.</summary>
    OutBounce,

    /// <summary>Bounce easing in and out.</summary>
    InOutBounce
}
