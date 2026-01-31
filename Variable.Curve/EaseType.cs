namespace Variable.Curve;

/// <summary>
///     Defines standard easing function types.
///     Based on Robert Penner's easing functions.
/// </summary>
public enum EaseType
{
    /// <summary>No easing, no acceleration.</summary>
    Linear,

    /// <summary>Accelerates from zero velocity.</summary>
    InQuad,
    /// <summary>Decelerates to zero velocity.</summary>
    OutQuad,
    /// <summary>Accelerates until halfway, then decelerates.</summary>
    InOutQuad,

    /// <summary>Accelerates from zero velocity (cubic).</summary>
    InCubic,
    /// <summary>Decelerates to zero velocity (cubic).</summary>
    OutCubic,
    /// <summary>Accelerates until halfway, then decelerates (cubic).</summary>
    InOutCubic,

    /// <summary>Accelerates from zero velocity (quartic).</summary>
    InQuart,
    /// <summary>Decelerates to zero velocity (quartic).</summary>
    OutQuart,
    /// <summary>Accelerates until halfway, then decelerates (quartic).</summary>
    InOutQuart,

    /// <summary>Accelerates from zero velocity (quintic).</summary>
    InQuint,
    /// <summary>Decelerates to zero velocity (quintic).</summary>
    OutQuint,
    /// <summary>Accelerates until halfway, then decelerates (quintic).</summary>
    InOutQuint,

    /// <summary>Accelerates from zero velocity (sine).</summary>
    InSine,
    /// <summary>Decelerates to zero velocity (sine).</summary>
    OutSine,
    /// <summary>Accelerates until halfway, then decelerates (sine).</summary>
    InOutSine,

    /// <summary>Accelerates from zero velocity (exponential).</summary>
    InExpo,
    /// <summary>Decelerates to zero velocity (exponential).</summary>
    OutExpo,
    /// <summary>Accelerates until halfway, then decelerates (exponential).</summary>
    InOutExpo,

    /// <summary>Accelerates from zero velocity (circular).</summary>
    InCirc,
    /// <summary>Decelerates to zero velocity (circular).</summary>
    OutCirc,
    /// <summary>Accelerates until halfway, then decelerates (circular).</summary>
    InOutCirc,

    /// <summary>Elastic bounce effect.</summary>
    InElastic,
    /// <summary>Elastic bounce effect.</summary>
    OutElastic,
    /// <summary>Elastic bounce effect.</summary>
    InOutElastic,

    /// <summary>Overshoots slightly then returns.</summary>
    InBack,
    /// <summary>Overshoots slightly then returns.</summary>
    OutBack,
    /// <summary>Overshoots slightly then returns.</summary>
    InOutBack,

    /// <summary>Bouncing ball effect.</summary>
    InBounce,
    /// <summary>Bouncing ball effect.</summary>
    OutBounce,
    /// <summary>Bouncing ball effect.</summary>
    InOutBounce
}
