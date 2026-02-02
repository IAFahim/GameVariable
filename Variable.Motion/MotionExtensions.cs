namespace Variable.Motion;

/// <summary>
///     Extensions for updating Motion structs.
/// </summary>
public static class MotionExtensions
{
    /// <summary>
    ///     Advances the tween by deltaTime and updates its Value.
    /// </summary>
    /// <param name="tween">The tween to update.</param>
    /// <param name="deltaTime">Time elapsed since last frame.</param>
    /// <returns>True if the tween is complete.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Tick(ref this TweenFloat tween, float deltaTime)
    {
        if (tween.Time >= tween.Duration)
        {
            // Already complete, ensure value is End
            tween.Value = tween.End;
            return true;
        }

        tween.Time += deltaTime;

        // Clamp time
        if (tween.Time >= tween.Duration)
        {
            tween.Time = tween.Duration;
            tween.Value = tween.End;
            return true;
        }

        // Calculate value
        tween.Value = tween.CalculateValue();
        return false;
    }

    /// <summary>
    ///     Advances the spring physics by deltaTime.
    /// </summary>
    /// <param name="spring">The spring to update.</param>
    /// <param name="deltaTime">Time elapsed since last frame.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Tick(ref this SpringFloat spring, float deltaTime)
    {
        MotionLogic.IntegrateSpring(
            spring.Current,
            spring.Target,
            ref spring.Velocity,
            spring.Stiffness,
            spring.Damping,
            deltaTime,
            out spring.Current
        );
    }
}
