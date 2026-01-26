namespace Variable.Bounded;

/// <summary>
///     Pure primitive logic for bounding values.
///     All methods operate on primitives only - NO STRUCTS.
/// </summary>
public static class BoundedLogic
{
    /// <summary>Clamps a float value between min and max.</summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="result">The clamped value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clamp(in float value, in float min, in float max, out float result)
    {
        CoreMath.Clamp(value, min, max, out result);
    }

    /// <summary>Sets a value within bounds.</summary>
    /// <param name="current">Reference to the current value.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="value">The new value to set.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref float current, in float min, in float max, in float value)
    {
        CoreMath.Clamp(value, min, max, out current);
    }

    /// <summary>Normalizes the current value to be within bounds.</summary>
    /// <param name="current">Reference to the current value.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Normalize(ref float current, in float min, in float max)
    {
        CoreMath.Clamp(current, min, max, out current);
    }

    /// <summary>Checks if the value is full (at max) within a tolerance.</summary>
    /// <param name="current">The current value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="tolerance">The tolerance for floating point comparison.</param>
    /// <returns>True if full; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(in float current, in float max, in float tolerance)
    {
        return current >= max - tolerance;
    }

    /// <summary>Checks if the value is empty (at min) within a tolerance.</summary>
    /// <param name="current">The current value.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="tolerance">The tolerance for floating point comparison.</param>
    /// <returns>True if empty; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(in float current, in float min, in float tolerance)
    {
        return current <= min + tolerance;
    }

    /// <summary>Calculates the ratio of the current value within the range.</summary>
    /// <param name="current">The current value.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="result">The ratio between 0.0 and 1.0.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetRatio(in float current, in float min, in float max, out double result)
    {
        var range = max - min;
        result = Math.Abs(range) < MathConstants.Tolerance ? 0.0 : (current - min) / range;
    }

    /// <summary>Calculates the range (max - min).</summary>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="result">The range.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetRange(in float min, in float max, out float result)
    {
        result = max - min;
    }

    /// <summary>Calculates the remaining amount to reach max.</summary>
    /// <param name="current">The current value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="result">The remaining amount.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetRemaining(in float current, in float max, out float result)
    {
        result = max - current;
    }

    /// <summary>Clamps an int value between min and max.</summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="result">The clamped value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clamp(in int value, in int min, in int max, out int result)
    {
        CoreMath.Clamp(value, min, max, out result);
    }

    /// <summary>Sets an int value within bounds.</summary>
    /// <param name="current">Reference to the current value.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="value">The new value to set.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref int current, in int min, in int max, in int value)
    {
        CoreMath.Clamp(value, min, max, out current);
    }

    /// <summary>Normalizes the current int value to be within bounds.</summary>
    /// <param name="current">Reference to the current value.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Normalize(ref int current, in int min, in int max)
    {
        CoreMath.Clamp(current, min, max, out current);
    }

    /// <summary>Checks if the int value is full (at max).</summary>
    /// <param name="current">The current value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>True if full; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(in int current, in int max)
    {
        return current == max;
    }

    /// <summary>Checks if the int value is empty (at min).</summary>
    /// <param name="current">The current value.</param>
    /// <param name="min">The minimum value.</param>
    /// <returns>True if empty; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(in int current, in int min)
    {
        return current == min;
    }

    /// <summary>Calculates the ratio of the current int value within the range.</summary>
    /// <param name="current">The current value.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="result">The ratio between 0.0 and 1.0.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetRatio(in int current, in int min, in int max, out double result)
    {
        var range = max - min;
        result = range == 0 ? 0.0 : (double)(current - min) / range;
    }

    /// <summary>Calculates the range (max - min) for int values.</summary>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="result">The range.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetRange(in int min, in int max, out int result)
    {
        result = max - min;
    }

    /// <summary>Calculates the remaining amount to reach max for int values.</summary>
    /// <param name="current">The current value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="result">The remaining amount.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetRemaining(in int current, in int max, out int result)
    {
        result = max - current;
    }

    /// <summary>Clamps a byte value up to max.</summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="result">The clamped value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clamp(in byte value, in byte max, out byte result)
    {
        CoreMath.Clamp(value, max, out result);
    }

    /// <summary>Clamps an int value to a byte range [0, max].</summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="result">The clamped byte value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clamp(in int value, in byte max, out byte result)
    {
        CoreMath.Clamp(value, max, out result);
    }

    /// <summary>Sets a byte value within bounds.</summary>
    /// <param name="current">Reference to the current value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="value">The new value to set.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref byte current, in byte max, in byte value)
    {
        CoreMath.Clamp(value, max, out current);
    }

    /// <summary>Normalizes the current byte value to be within bounds.</summary>
    /// <param name="current">Reference to the current value.</param>
    /// <param name="max">The maximum value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Normalize(ref byte current, in byte max)
    {
        CoreMath.Clamp(current, max, out current);
    }

    /// <summary>Checks if the byte value is full (at max).</summary>
    /// <param name="current">The current value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>True if full; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(in byte current, in byte max)
    {
        return current == max;
    }

    /// <summary>Checks if the byte value is empty (0).</summary>
    /// <param name="current">The current value.</param>
    /// <returns>True if empty; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(in byte current)
    {
        return current == 0;
    }

    /// <summary>Calculates the ratio of the current byte value within the range [0, max].</summary>
    /// <param name="current">The current value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="result">The ratio between 0.0 and 1.0.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetRatio(in byte current, in byte max, out double result)
    {
        result = max == 0 ? 0.0 : (double)current / max;
    }

    /// <summary>Returns the range for byte values (max).</summary>
    /// <param name="max">The maximum value.</param>
    /// <param name="result">The range.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetRange(in byte max, out byte result)
    {
        result = max;
    }

    /// <summary>Calculates the remaining amount to reach max for byte values.</summary>
    /// <param name="current">The current value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="result">The remaining amount.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetRemaining(in byte current, in byte max, out byte result)
    {
        result = (byte)(max - current);
    }

    /// <summary>Adds an amount to the current value, clamping within bounds.</summary>
    /// <param name="current">The current value.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="amount">The amount to add.</param>
    /// <param name="result">The new clamped value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add(in int current, in int min, in int max, in int amount, out int result)
    {
        // ⚡ Bolt: This implementation avoids the overhead of casting to a long, instead using pre-emptive checks to handle potential integer overflow and underflow. This results in a more performant `Add` operation, which is critical in performance-sensitive game development scenarios.
        if (amount > 0 && current > int.MaxValue - amount)
        {
            result = max;
            return;
        }

        if (amount < 0 && current < int.MinValue - amount)
        {
            result = min;
            return;
        }

        CoreMath.Clamp(current + amount, min, max, out result);
    }
}