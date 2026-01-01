namespace Variable.Timer;

/// <summary>
///     Extension methods for <see cref="Timer"/> and <see cref="Cooldown"/>.
///     These methods bridge from structs to primitive-only logic.
/// </summary>
public static class TimerExtensions
{
    private const float Tolerance = 0.0001f;

    // Timer Extensions
    
    /// <summary>
    ///     Advances the timer by the specified delta time.
    /// </summary>
    /// <param name="timer">The timer to advance.</param>
    /// <param name="deltaTime">The time elapsed since the last tick.</param>
    /// <returns>True if the timer is full (completed); otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryTick(ref this Timer timer, float deltaTime)
    {
        return TimerLogic.Tick(ref timer.Current, timer.Duration, deltaTime);
    }

    /// <summary>
    ///     Advances the timer by the specified delta time (void version).
    ///     Use this for "fire and forget" patterns where you don't need to check completion.
    /// </summary>
    /// <param name="timer">The timer to advance.</param>
    /// <param name="deltaTime">The time elapsed since the last tick.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Tick(ref this Timer timer, float deltaTime)
    {
        TimerLogic.Tick(ref timer.Current, timer.Duration, deltaTime);
    }

    /// <summary>
    ///     Advances the timer and outputs overflow time.
    /// </summary>
    /// <param name="timer">The timer to advance.</param>
    /// <param name="deltaTime">The time elapsed since the last tick.</param>
    /// <param name="overflow">The amount of time that exceeded the duration.</param>
    /// <returns>True if the timer is full (completed); otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryTick(ref this Timer timer, float deltaTime, out float overflow)
    {
        return TimerLogic.Tick(ref timer.Current, timer.Duration, deltaTime, out overflow);
    }

    /// <summary>
    ///     Resets the timer elapsed time to 0.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Reset(ref this Timer timer)
    {
        TimerLogic.Reset(ref timer.Current, 0f);
    }

    /// <summary>
    ///     Resets the timer and applies an overflow offset.
    /// </summary>
    /// <param name="timer">The timer to reset.</param>
    /// <param name="overflow">The amount of time to start the timer with (clamped to duration).</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Reset(ref this Timer timer, float overflow)
    {
        TimerLogic.ResetWithOverflow(ref timer.Current, 0f, overflow, 0f, timer.Duration);
    }

    /// <summary>
    ///     Completes the timer immediately by setting current to duration.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Finish(ref this Timer timer)
    {
        TimerLogic.Reset(ref timer.Current, timer.Duration);
    }

    /// <summary>
    ///     Returns true when the timer has reached or exceeded its duration.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(this Timer timer) => timer.Current >= timer.Duration - Tolerance;

    /// <summary>
    ///     Returns true when the timer is at zero (not yet started).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(this Timer timer) => timer.Current <= Tolerance;

    /// <summary>
    ///     Gets the progress of the timer as a ratio from 0 (not started) to 1 (complete).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetRatio(this Timer timer)
    {
        return Math.Abs(timer.Duration) < Tolerance ? 0.0 : (double)timer.Current / timer.Duration;
    }

    // Cooldown Extensions
    
    /// <summary>
    ///     Advances the cooldown by the specified delta time.
    /// </summary>
    /// <param name="cooldown">The cooldown to advance.</param>
    /// <param name="deltaTime">The time elapsed since the last tick.</param>
    /// <returns>True if the cooldown is ready (Current is 0); otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryTick(ref this Cooldown cooldown, float deltaTime)
    {
        return TimerLogic.TickCooldown(ref cooldown.Current, deltaTime);
    }

    /// <summary>
    ///     Advances the cooldown by the specified delta time (void version).
    ///     Use this for "fire and forget" patterns where you don't need to check if ready.
    /// </summary>
    /// <param name="cooldown">The cooldown to advance.</param>
    /// <param name="deltaTime">The time elapsed since the last tick.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Tick(ref this Cooldown cooldown, float deltaTime)
    {
        TimerLogic.TickCooldown(ref cooldown.Current, deltaTime);
    }

    /// <summary>
    ///     Advances the cooldown and outputs overflow time.
    /// </summary>
    /// <param name="cooldown">The cooldown to advance.</param>
    /// <param name="deltaTime">The time elapsed since the last tick.</param>
    /// <param name="overflow">The amount of time that exceeded zero.</param>
    /// <returns>True if the cooldown is ready (Current is 0); otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryTick(ref this Cooldown cooldown, float deltaTime, out float overflow)
    {
        return TimerLogic.TickCooldown(ref cooldown.Current, deltaTime, out overflow);
    }

    /// <summary>
    ///     Starts the cooldown by setting Current to Duration.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Reset(ref this Cooldown cooldown)
    {
        TimerLogic.Reset(ref cooldown.Current, cooldown.Duration);
    }

    /// <summary>
    ///     Starts the cooldown, reducing the start time by the provided overflow.
    /// </summary>
    /// <param name="cooldown">The cooldown to reset.</param>
    /// <param name="overflow">Excess time to subtract from the new duration.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Reset(ref this Cooldown cooldown, float overflow)
    {
        TimerLogic.ResetWithOverflow(ref cooldown.Current, cooldown.Duration, -overflow, 0f, cooldown.Duration);
    }

    /// <summary>
    ///     Finishes the cooldown immediately by setting Current to 0.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Finish(ref this Cooldown cooldown)
    {
        TimerLogic.Reset(ref cooldown.Current, 0f);
    }

    /// <summary>
    ///     Returns true when the cooldown is ready (Current &lt;= 0).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsReady(this Cooldown cooldown) => cooldown.Current <= Tolerance;

    /// <summary>
    ///     Returns true when the cooldown is ready. Alias for IsReady.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(this Cooldown cooldown) => cooldown.Current <= Tolerance;

    /// <summary>
    ///     Returns true when the cooldown is at its maximum duration (just started).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(this Cooldown cooldown) => cooldown.Current >= cooldown.Duration - Tolerance;

    /// <summary>
    ///     Returns true when the cooldown is active (not yet ready).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOnCooldown(this Cooldown cooldown) => cooldown.Current > Tolerance;

    /// <summary>
    ///     Gets the ratio of remaining time.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetRatio(this Cooldown cooldown)
    {
        return Math.Abs(cooldown.Duration) < Tolerance ? 0.0 : (double)cooldown.Current / cooldown.Duration;
    }

    /// <summary>
    ///     Gets the progress of the cooldown as a ratio from 0 (just started) to 1 (ready).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetProgress(this Cooldown cooldown)
    {
        return MathF.Abs(cooldown.Duration) < Tolerance ? 1f : 1f - MathF.Max(0, cooldown.Current / cooldown.Duration);
    }
}

