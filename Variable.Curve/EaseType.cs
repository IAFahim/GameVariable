namespace Variable.Curve;

/// <summary>
///     Defines the standard easing function types.
/// </summary>
public enum EaseType
{
    /// <summary>Linear interpolation (no easing).</summary>
    Linear,

    /// <summary>Quadratic ease-in (t^2).</summary>
    InQuad,

    /// <summary>Quadratic ease-out.</summary>
    OutQuad,

    /// <summary>Quadratic ease-in-out.</summary>
    InOutQuad,

    /// <summary>Cubic ease-in (t^3).</summary>
    InCubic,

    /// <summary>Cubic ease-out.</summary>
    OutCubic,

    /// <summary>Cubic ease-in-out.</summary>
    InOutCubic,

    /// <summary>Quartic ease-in (t^4).</summary>
    InQuart,

    /// <summary>Quartic ease-out.</summary>
    OutQuart,

    /// <summary>Quartic ease-in-out.</summary>
    InOutQuart,

    /// <summary>Quintic ease-in (t^5).</summary>
    InQuint,

    /// <summary>Quintic ease-out.</summary>
    OutQuint,

    /// <summary>Quintic ease-in-out.</summary>
    InOutQuint,

    /// <summary>Sinusoidal ease-in.</summary>
    InSine,

    /// <summary>Sinusoidal ease-out.</summary>
    OutSine,

    /// <summary>Sinusoidal ease-in-out.</summary>
    InOutSine,

    /// <summary>Exponential ease-in.</summary>
    InExpo,

    /// <summary>Exponential ease-out.</summary>
    OutExpo,

    /// <summary>Exponential ease-in-out.</summary>
    InOutExpo,

    /// <summary>Circular ease-in.</summary>
    InCirc,

    /// <summary>Circular ease-out.</summary>
    OutCirc,

    /// <summary>Circular ease-in-out.</summary>
    InOutCirc,

    /// <summary>Back ease-in (overshoots).</summary>
    InBack,

    /// <summary>Back ease-out (overshoots).</summary>
    OutBack,

    /// <summary>Back ease-in-out (overshoots).</summary>
    InOutBack,

    /// <summary>Elastic ease-in.</summary>
    InElastic,

    /// <summary>Elastic ease-out.</summary>
    OutElastic,

    /// <summary>Elastic ease-in-out.</summary>
    InOutElastic,

    /// <summary>Bounce ease-in.</summary>
    InBounce,

    /// <summary>Bounce ease-out.</summary>
    OutBounce,

    /// <summary>Bounce ease-in-out.</summary>
    InOutBounce
}
