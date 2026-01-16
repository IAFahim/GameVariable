namespace Variable.Tween;

/// <summary>
///     A zero-allocation struct that interpolates a float value over time using an easing function.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct TweenFloat
{
    /// <summary>The starting value.</summary>
    public float Start;

    /// <summary>The target value.</summary>
    public float End;

    /// <summary>The current interpolated value.</summary>
    public float Current;

    /// <summary>The total duration of the tween in seconds.</summary>
    public float Duration;

    /// <summary>The elapsed time in seconds.</summary>
    public float Elapsed;

    /// <summary>The easing function to use.</summary>
    public EasingType Easing;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TweenFloat" /> struct.
    /// </summary>
    /// <param name="start">The starting value.</param>
    /// <param name="end">The target value.</param>
    /// <param name="duration">The duration in seconds.</param>
    /// <param name="easing">The easing function (default: Linear).</param>
    public TweenFloat(float start, float end, float duration, EasingType easing = EasingType.Linear)
    {
        Start = start;
        End = end;
        Current = start;
        Duration = duration;
        Elapsed = 0f;
        Easing = easing;
    }
}
