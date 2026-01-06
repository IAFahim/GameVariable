namespace Variable.Timer;

/// <summary>
///     Pure logic for timer operations.
///     All methods operate on primitives only - NO STRUCTS.
/// </summary>
public static class TimerLogic
{
    /// <summary>
    ///     Clamps a value between min and max.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clamp(float value, float min, float max, out float result)
    {
        CoreMath.Clamp(value, min, max, out result);
    }

    /// <summary>
    ///     Checks if a value is close enough to max to be considered full.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(float current, float max)
    {
        return current >= max - MathConstants.Tolerance;
    }

    /// <summary>
    ///     Checks if a value is close enough to min (0) to be considered empty.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(float current)
    {
        return current <= MathConstants.Tolerance;
    }

    /// <summary>
    ///     Calculates the ratio of current vs duration.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetRatio(float current, float duration, out double result)
    {
        result = Math.Abs(duration) < MathConstants.Tolerance ? 0.0 : (double)current / duration;
    }

    /// <summary>
    ///     Calculates the inverse ratio (1 - ratio).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetInverseRatio(float current, float duration, out float result)
    {
        result = Math.Abs(duration) < MathConstants.Tolerance ? 1f : 1f - Math.Max(0, current / duration);
    }

    /// <summary>
    ///     Advances a timer by delta time.
    /// </summary>
    /// <param name="current">The current elapsed time.</param>
    /// <param name="duration">The target duration.</param>
    /// <param name="deltaTime">The time to advance.</param>
    /// <returns>True if the timer COMPLETED (reached or exceeded duration); false if still running.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TickAndCheckComplete(ref float current, float duration, float deltaTime)
    {
        if (current >= duration - MathConstants.Tolerance) return true;

        current += deltaTime;
        if (current >= duration - MathConstants.Tolerance)
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
    /// <returns>True if the timer COMPLETED (reached or exceeded duration); false if still running.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TickAndCheckComplete(ref float current, float duration, float deltaTime, out float overflow)
    {
        if (current >= duration - MathConstants.Tolerance)
        {
            overflow = deltaTime;
            return true;
        }

        current += deltaTime;
        if (current >= duration - MathConstants.Tolerance)
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
    /// <returns>True if the cooldown is READY (reached or passed zero); false if still cooling down.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TickAndCheckReady(ref float current, float deltaTime)
    {
        if (current <= MathConstants.Tolerance) return true;

        current -= deltaTime;
        if (current <= MathConstants.Tolerance)
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
    /// <returns>True if the cooldown is READY (reached or passed zero); false if still cooling down.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TickAndCheckReady(ref float current, float deltaTime, out float overflow)
    {
        if (current <= MathConstants.Tolerance)
        {
            overflow = deltaTime;
            return true;
        }

        current -= deltaTime;
        if (current <= MathConstants.Tolerance)
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
        CoreMath.Clamp(current, clampMin, clampMax, out current);
    }
}