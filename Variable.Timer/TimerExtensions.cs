namespace Variable.Timer;

/// <summary>
///     Extension methods for <see cref="Timer" /> and <see cref="Cooldown" />.
///     These methods bridge from structs to primitive-only logic.
/// </summary>
public static class TimerExtensions
{
    // Timer Extensions

    /// <summary>
    ///     Advances the timer by delta time and checks if it completed.
    /// </summary>
    /// <param name="self">The timer to advance.</param>
    /// <param name="deltaTime">The time elapsed since the last tick.</param>
    /// <returns>True if the timer COMPLETED (Current &gt;= Duration); false if still running.</returns>
    /// <remarks>Use this when you need to detect completion, e.g., for spell casts or loading bars.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TickAndCheckComplete(ref this Timer self, float deltaTime)
    {
        return TimerLogic.TickAndCheckComplete(ref self.Current, self.Duration, deltaTime);
    }

    /// <summary>
    ///     Advances the timer by delta time without returning completion status.
    /// </summary>
    /// <param name="self">The timer to advance.</param>
    /// <param name="deltaTime">The time elapsed since the last tick.</param>
    /// <remarks>Use this for "fire and forget" patterns where you don't need to check completion.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Tick(ref this Timer self, float deltaTime)
    {
        TimerLogic.TickAndCheckComplete(ref self.Current, self.Duration, deltaTime);
    }

    /// <summary>
    ///     Advances the timer and captures overflow time beyond the duration.
    /// </summary>
    /// <param name="self">The timer to advance.</param>
    /// <param name="deltaTime">The time elapsed since the last tick.</param>
    /// <param name="overflow">The amount of time that exceeded the duration (useful for chaining timers).</param>
    /// <returns>True if the timer COMPLETED (Current &gt;= Duration); false if still running.</returns>
    /// <remarks>Use overflow to start the next timer with remaining time for seamless chaining.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TickAndCheckComplete(ref this Timer self, float deltaTime, out float overflow)
    {
        return TimerLogic.TickAndCheckComplete(ref self.Current, self.Duration, deltaTime, out overflow);
    }

    /// <summary>
    ///     Resets the timer elapsed time to 0.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Reset(ref this Timer self)
    {
        TimerLogic.Reset(ref self.Current, 0f);
    }

    /// <summary>
    ///     Resets the timer and applies an overflow offset.
    /// </summary>
    /// <param name="self">The timer to reset.</param>
    /// <param name="overflow">The amount of time to start the timer with (clamped to duration).</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Reset(ref this Timer self, float overflow)
    {
        TimerLogic.ResetWithOverflow(ref self.Current, 0f, overflow, 0f, self.Duration);
    }

    /// <summary>
    ///     Completes the timer immediately by setting current to duration.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Finish(ref this Timer self)
    {
        TimerLogic.Reset(ref self.Current, self.Duration);
    }

    /// <summary>
    ///     Returns true when the timer has reached or exceeded its duration.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(this Timer self)
    {
        return TimerLogic.IsFull(self.Current, self.Duration);
    }

    /// <summary>
    ///     Returns true when the timer is at zero (not yet started).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(this Timer self)
    {
        return TimerLogic.IsEmpty(self.Current);
    }

    /// <summary>
    ///     Gets the progress of the timer as a ratio from 0 (not started) to 1 (complete).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetRatio(this Timer self)
    {
        TimerLogic.GetRatio(self.Current, self.Duration, out var result);
        return result;
    }

    // Cooldown Extensions

    /// <summary>
    ///     Advances the cooldown by delta time and checks if it's ready.
    /// </summary>
    /// <param name="self">The cooldown to advance.</param>
    /// <param name="deltaTime">The time elapsed since the last tick.</param>
    /// <returns>True if the cooldown is READY (Current &lt;= 0); false if still cooling down.</returns>
    /// <remarks>Use this to check if an ability can be used again.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TickAndCheckReady(ref this Cooldown self, float deltaTime)
    {
        return TimerLogic.TickAndCheckReady(ref self.Current, deltaTime);
    }

    /// <summary>
    ///     Advances the cooldown by delta time without returning ready status.
    /// </summary>
    /// <param name="self">The cooldown to advance.</param>
    /// <param name="deltaTime">The time elapsed since the last tick.</param>
    /// <remarks>Use this for "fire and forget" patterns where you don't need to check if ready.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Tick(ref this Cooldown self, float deltaTime)
    {
        TimerLogic.TickAndCheckReady(ref self.Current, deltaTime);
    }

    /// <summary>
    ///     Advances the cooldown and captures overflow time beyond zero.
    /// </summary>
    /// <param name="self">The cooldown to advance.</param>
    /// <param name="deltaTime">The time elapsed since the last tick.</param>
    /// <param name="overflow">The amount of time that exceeded zero (useful for ability chains).</param>
    /// <returns>True if the cooldown is READY (Current &lt;= 0); false if still cooling down.</returns>
    /// <remarks>Use overflow to reduce the next cooldown start time for fluid ability rotations.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TickAndCheckReady(ref this Cooldown self, float deltaTime, out float overflow)
    {
        return TimerLogic.TickAndCheckReady(ref self.Current, deltaTime, out overflow);
    }

    /// <summary>
    ///     Starts the cooldown by setting Current to Duration.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Reset(ref this Cooldown self)
    {
        TimerLogic.Reset(ref self.Current, self.Duration);
    }

    /// <summary>
    ///     Starts the cooldown, reducing the start time by the provided overflow.
    /// </summary>
    /// <param name="self">The cooldown to reset.</param>
    /// <param name="overflow">Excess time to subtract from the new duration.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Reset(ref this Cooldown self, float overflow)
    {
        TimerLogic.ResetWithOverflow(ref self.Current, self.Duration, -overflow, 0f, self.Duration);
    }

    /// <summary>
    ///     Finishes the cooldown immediately by setting Current to 0.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Finish(ref this Cooldown self)
    {
        TimerLogic.Reset(ref self.Current, 0f);
    }

    /// <summary>
    ///     Returns true when the cooldown is ready (Current &lt;= 0).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsReady(this Cooldown self)
    {
        return TimerLogic.IsEmpty(self.Current);
    }

    /// <summary>
    ///     Returns true when the cooldown is ready. Alias for IsReady.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(this Cooldown self)
    {
        return TimerLogic.IsEmpty(self.Current);
    }

    /// <summary>
    ///     Returns true when the cooldown is at its maximum duration (just started).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(this Cooldown self)
    {
        return TimerLogic.IsFull(self.Current, self.Duration);
    }

    /// <summary>
    ///     Returns true when the cooldown is active (not yet ready).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOnCooldown(this Cooldown self)
    {
        return !TimerLogic.IsEmpty(self.Current);
    }

    /// <summary>
    ///     Gets the ratio of remaining time.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetRatio(this Cooldown self)
    {
        TimerLogic.GetRatio(self.Current, self.Duration, out var result);
        return result;
    }

    /// <summary>
    ///     Gets the progress of the cooldown as a ratio from 0 (just started) to 1 (ready).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetProgress(this Cooldown self)
    {
        TimerLogic.GetInverseRatio(self.Current, self.Duration, out var result);
        return result;
    }
}