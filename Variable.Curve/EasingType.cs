namespace Variable.Curve;

/// <summary>
///     Enumeration of standard easing function types.
///     Useful for serialization in Unity inspectors or data-driven configurations.
/// </summary>
public enum EasingType
{
    /// <summary>No easing, linear interpolation.</summary>
    Linear,

    /// <summary>Sine In easing.</summary>
    InSine,
    /// <summary>Sine Out easing.</summary>
    OutSine,
    /// <summary>Sine InOut easing.</summary>
    InOutSine,

    /// <summary>Quadratic In easing.</summary>
    InQuad,
    /// <summary>Quadratic Out easing.</summary>
    OutQuad,
    /// <summary>Quadratic InOut easing.</summary>
    InOutQuad,

    /// <summary>Cubic In easing.</summary>
    InCubic,
    /// <summary>Cubic Out easing.</summary>
    OutCubic,
    /// <summary>Cubic InOut easing.</summary>
    InOutCubic,

    /// <summary>Quartic In easing.</summary>
    InQuart,
    /// <summary>Quartic Out easing.</summary>
    OutQuart,
    /// <summary>Quartic InOut easing.</summary>
    InOutQuart,

    /// <summary>Quintic In easing.</summary>
    InQuint,
    /// <summary>Quintic Out easing.</summary>
    OutQuint,
    /// <summary>Quintic InOut easing.</summary>
    InOutQuint,

    /// <summary>Exponential In easing.</summary>
    InExpo,
    /// <summary>Exponential Out easing.</summary>
    OutExpo,
    /// <summary>Exponential InOut easing.</summary>
    InOutExpo,

    /// <summary>Circular In easing.</summary>
    InCirc,
    /// <summary>Circular Out easing.</summary>
    OutCirc,
    /// <summary>Circular InOut easing.</summary>
    InOutCirc,

    /// <summary>Back In easing.</summary>
    InBack,
    /// <summary>Back Out easing.</summary>
    OutBack,
    /// <summary>Back InOut easing.</summary>
    InOutBack,

    /// <summary>Elastic In easing.</summary>
    InElastic,
    /// <summary>Elastic Out easing.</summary>
    OutElastic,
    /// <summary>Elastic InOut easing.</summary>
    InOutElastic,

    /// <summary>Bounce In easing.</summary>
    InBounce,
    /// <summary>Bounce Out easing.</summary>
    OutBounce,
    /// <summary>Bounce InOut easing.</summary>
    InOutBounce
}
