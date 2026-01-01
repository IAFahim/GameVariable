namespace Variable.Timer;

/// <summary>
///     Pure logic for timer operations.
///     All methods operate on primitives only - NO STRUCTS.
/// </summary>
public static class TimerLogic
{
    private const float Tolerance = 0.0001f;

    /// <summary>
    ///     Advances a timer by delta time.
    /// </summary>
    /// <param name="current">The current elapsed time.</param>
    /// <param name="duration">The target duration.</param>
    /// <param name="deltaTime">The time to advance.</param>
    /// <returns>True if the timer reached or exceeded duration; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Tick(ref float current, float duration, float deltaTime)
    {
        if (current >= duration - Tolerance) return true;

        current += deltaTime;
        if (current >= duration - Tolerance)
        {
            current = duration;
            return true;
        }

        return false;
    }

    /// <summary>
    ///     Advances a timer and outputs overflow time.
    /// </summary>
    /// <param name="current">The current elapsed time.</param>
    /// <param name="duration">The target duration.</param>
    /// <param name="deltaTime">The time to advance.</param>
    /// <param name="overflow">The amount of time that exceeded the duration.</param>
    /// <returns>True if the timer reached or exceeded duration; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Tick(ref float current, float duration, float deltaTime, out float overflow)
    {
        if (current >= duration - Tolerance)
        {
            overflow = deltaTime;
            return true;
        }

        current += deltaTime;
        if (current >= duration - Tolerance)
        {
            overflow = current - duration;
            current = duration;
            return true;
        }

        overflow = 0f;
        return false;
    }

    /// <summary>
    ///     Advances a cooldown by delta time (counts down).
    /// </summary>
    /// <param name="current">The current remaining time.</param>
    /// <param name="deltaTime">The time to advance.</param>
    /// <returns>True if the cooldown is ready (reached or passed zero); otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TickCooldown(ref float current, float deltaTime)
    {
        if (current <= Tolerance) return true;

        current -= deltaTime;
        if (current <= Tolerance)
        {
            current = 0f;
            return true;
        }

        return false;
    }

    /// <summary>
    ///     Advances a cooldown and outputs overflow time.
    /// </summary>
    /// <param name="current">The current remaining time.</param>
    /// <param name="deltaTime">The time to advance.</param>
    /// <param name="overflow">The amount of time that exceeded zero.</param>
    /// <returns>True if the cooldown is ready (reached or passed zero); otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TickCooldown(ref float current, float deltaTime, out float overflow)
    {
        if (current <= Tolerance)
        {
            overflow = deltaTime;
            return true;
        }

        current -= deltaTime;
        if (current <= Tolerance)
        {
            overflow = -current;
            current = 0f;
            return true;
        }

        overflow = 0f;
        return false;
    }

    /// <summary>
    ///     Resets a value to a target.
    /// </summary>
    /// <param name="current">The current value to reset.</param>
    /// <param name="targetValue">The target value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Reset(ref float current, float targetValue)
    {
        current = targetValue;
    }

    /// <summary>
    ///     Resets a value with overflow adjustment.
    /// </summary>
    /// <param name="current">The current value to reset.</param>
    /// <param name="targetValue">The target value.</param>
    /// <param name="overflow">The overflow to apply.</param>
    /// <param name="clampMin">Minimum clamp value.</param>
    /// <param name="clampMax">Maximum clamp value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ResetWithOverflow(ref float current, float targetValue, float overflow, float clampMin,
        float clampMax)
    {
        current = targetValue + overflow;
        if (current < clampMin) current = clampMin;
        else if (current > clampMax) current = clampMax;
    }
}