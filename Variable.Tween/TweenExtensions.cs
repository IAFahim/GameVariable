namespace Variable.Tween;

/// <summary>
///     Extension methods for <see cref="TweenFloat" /> providing mutation and convenience APIs.
/// </summary>
public static class TweenExtensions
{
    /// <summary>
    ///     Advances the tween by the specified delta time.
    /// </summary>
    /// <param name="tween">The tween instance.</param>
    /// <param name="dt">The time to advance by.</param>
    /// <returns>True if the tween is finished, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Tick(ref this TweenFloat tween, float dt)
    {
        TweenLogic.Tick(
            tween.Start,
            tween.End,
            tween.Duration,
            tween.Elapsed,
            tween.Easing,
            dt,
            out tween.Elapsed,
            out tween.Current,
            out bool isFinished
        );

        return isFinished;
    }

    /// <summary>
    ///     Resets the tween to its starting state.
    /// </summary>
    /// <param name="tween">The tween instance.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Reset(ref this TweenFloat tween)
    {
        tween.Elapsed = 0f;
        tween.Current = tween.Start;
    }

    /// <summary>
    ///     Configures and restarts the tween with new parameters.
    /// </summary>
    /// <param name="tween">The tween instance.</param>
    /// <param name="start">The starting value.</param>
    /// <param name="end">The target value.</param>
    /// <param name="duration">The duration.</param>
    /// <param name="easing">The easing function.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Play(ref this TweenFloat tween, float start, float end, float duration, EasingType easing = EasingType.Linear)
    {
        tween.Start = start;
        tween.End = end;
        tween.Duration = duration;
        tween.Easing = easing;
        tween.Elapsed = 0f;
        tween.Current = start;
    }

    /// <summary>
    ///     Checks if the tween has completed.
    /// </summary>
    /// <param name="tween">The tween instance.</param>
    /// <returns>True if elapsed time is greater than or equal to duration.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsComplete(ref this TweenFloat tween)
    {
        return tween.Elapsed >= tween.Duration;
    }
}
