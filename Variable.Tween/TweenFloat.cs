namespace Variable.Tween;

/// <summary>
/// A struct representing a float value that interpolates over time.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct TweenFloat
{
    /// <summary>The current interpolated value.</summary>
    public float CurrentValue;

    /// <summary>The starting value of the tween.</summary>
    public float StartValue;

    /// <summary>The target value of the tween.</summary>
    public float EndValue;

    /// <summary>The amount of time that has passed since the tween started.</summary>
    public float ElapsedSeconds;

    /// <summary>The total duration of the tween in seconds.</summary>
    public float DurationSeconds;

    /// <summary>The easing function to apply.</summary>
    public EasingType EasingType;

    /// <summary>
    /// Initializes a new instance of the <see cref="TweenFloat"/> struct.
    /// Sets CurrentValue to startValue immediately.
    /// </summary>
    /// <param name="start">The starting value.</param>
    /// <param name="end">The target value.</param>
    /// <param name="duration">The duration in seconds.</param>
    /// <param name="easing">The easing type.</param>
    public TweenFloat(float start, float end, float duration, EasingType easing = EasingType.Linear)
    {
        StartValue = start;
        CurrentValue = start; // Initialize current to start
        EndValue = end;
        DurationSeconds = duration;
        EasingType = easing;
        ElapsedSeconds = 0f;
    }
}
