namespace Variable.Tween;

/// <summary>
/// Extension methods for TweenFloat to provide a user-friendly API.
/// </summary>
public static class TweenExtensions
{
    /// <summary>
    /// Updates the tween by the specified delta time.
    /// </summary>
    /// <param name="tween">The tween to update.</param>
    /// <param name="deltaTime">The time to advance by (in seconds).</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Tick(ref this TweenFloat tween, float deltaTime)
    {
        TweenLogic.Tick(
            in tween.ElapsedSeconds,
            in tween.DurationSeconds,
            in deltaTime,
            out tween.ElapsedSeconds,
            out _
        );

        TweenLogic.GetNormalizedTime(
            in tween.ElapsedSeconds,
            in tween.DurationSeconds,
            out float t
        );

        TweenLogic.Evaluate(
            in tween.StartValue,
            in tween.EndValue,
            in t,
            in tween.EasingType,
            out tween.CurrentValue
        );
    }

    /// <summary>
    /// Checks if the tween has finished playing.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsComplete(in this TweenFloat tween)
    {
        return tween.ElapsedSeconds >= tween.DurationSeconds;
    }

    /// <summary>
    /// Gets the normalized progress (0.0 to 1.0) of the tween.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetProgress(in this TweenFloat tween)
    {
        TweenLogic.GetNormalizedTime(in tween.ElapsedSeconds, in tween.DurationSeconds, out float t);
        return t;
    }

    /// <summary>
    /// Resets the tween to its initial state.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Reset(ref this TweenFloat tween)
    {
        tween.ElapsedSeconds = 0f;
        tween.CurrentValue = tween.StartValue;
    }

    /// <summary>
    /// Forces the tween to complete immediately (sets time to duration and value to end).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Complete(ref this TweenFloat tween)
    {
        tween.ElapsedSeconds = tween.DurationSeconds;
        tween.CurrentValue = tween.EndValue;
    }
}
