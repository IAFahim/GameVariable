namespace Variable.Tween;

/// <summary>
///     Defines the easing functions available for tweening.
/// </summary>
public enum EasingType
{
    /// <summary>No easing, no acceleration.</summary>
    Linear,

    /// <summary>Accelerates from zero velocity.</summary>
    QuadIn,

    /// <summary>Decelerates to zero velocity.</summary>
    QuadOut,

    /// <summary>Accelerates until halfway, then decelerates.</summary>
    QuadInOut,

    /// <summary>Accelerates from zero velocity (cubic).</summary>
    CubicIn,

    /// <summary>Decelerates to zero velocity (cubic).</summary>
    CubicOut,

    /// <summary>Accelerates until halfway, then decelerates (cubic).</summary>
    CubicInOut,

    /// <summary>Accelerates from zero velocity (quartic).</summary>
    QuartIn,

    /// <summary>Decelerates to zero velocity (quartic).</summary>
    QuartOut,

    /// <summary>Accelerates until halfway, then decelerates (quartic).</summary>
    QuartInOut,

    /// <summary>Accelerates from zero velocity (quintic).</summary>
    QuintIn,

    /// <summary>Decelerates to zero velocity (quintic).</summary>
    QuintOut,

    /// <summary>Accelerates until halfway, then decelerates (quintic).</summary>
    QuintInOut,

    /// <summary>Accelerates from zero velocity (sine).</summary>
    SineIn,

    /// <summary>Decelerates to zero velocity (sine).</summary>
    SineOut,

    /// <summary>Accelerates until halfway, then decelerates (sine).</summary>
    SineInOut,

    /// <summary>Accelerates from zero velocity (exponential).</summary>
    ExpoIn,

    /// <summary>Decelerates to zero velocity (exponential).</summary>
    ExpoOut,

    /// <summary>Accelerates until halfway, then decelerates (exponential).</summary>
    ExpoInOut,

    /// <summary>Accelerates from zero velocity (circular).</summary>
    CircIn,

    /// <summary>Decelerates to zero velocity (circular).</summary>
    CircOut,

    /// <summary>Accelerates until halfway, then decelerates (circular).</summary>
    CircInOut,

    /// <summary>Overshoots slightly, then returns (back).</summary>
    BackIn,

    /// <summary>Overshoots slightly, then returns (back).</summary>
    BackOut,

    /// <summary>Overshoots slightly, then returns (back).</summary>
    BackInOut,

    /// <summary>Elastic bounce effect (elastic).</summary>
    ElasticIn,

    /// <summary>Elastic bounce effect (elastic).</summary>
    ElasticOut,

    /// <summary>Elastic bounce effect (elastic).</summary>
    ElasticInOut,

    /// <summary>Bouncing effect (bounce).</summary>
    BounceIn,

    /// <summary>Bouncing effect (bounce).</summary>
    BounceOut,

    /// <summary>Bouncing effect (bounce).</summary>
    BounceInOut
}
