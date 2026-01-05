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
    /// <returns>The clamped value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Clamp(float value, float min, float max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    /// <summary>Sets a value within bounds.</summary>
    /// <param name="current">Reference to the current value.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="value">The new value to set.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref float current, float min, float max, float value)
    {
        current = Clamp(value, min, max);
    }

    /// <summary>Normalizes the current value to be within bounds.</summary>
    /// <param name="current">Reference to the current value.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Normalize(ref float current, float min, float max)
    {
        current = Clamp(current, min, max);
    }

    /// <summary>Checks if the value is full (at max) within a tolerance.</summary>
    /// <param name="current">The current value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="tolerance">The tolerance for floating point comparison.</param>
    /// <returns>True if full; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(float current, float max, float tolerance)
    {
        return current >= max - tolerance;
    }

    /// <summary>Checks if the value is empty (at min) within a tolerance.</summary>
    /// <param name="current">The current value.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="tolerance">The tolerance for floating point comparison.</param>
    /// <returns>True if empty; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(float current, float min, float tolerance)
    {
        return current <= min + tolerance;
    }

    /// <summary>Calculates the ratio of the current value within the range.</summary>
    /// <param name="current">The current value.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The ratio between 0.0 and 1.0.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetRatio(float current, float min, float max)
    {
        var range = max - min;
        return Math.Abs(range) < MathConstants.Tolerance ? 0.0 : (current - min) / range;
    }

    /// <summary>Calculates the range (max - min).</summary>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The range.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetRange(float min, float max)
    {
        return max - min;
    }

    /// <summary>Calculates the remaining amount to reach max.</summary>
    /// <param name="current">The current value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The remaining amount.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetRemaining(float current, float max)
    {
        return max - current;
    }

    /// <summary>Clamps an int value between min and max.</summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The clamped value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Clamp(int value, int min, int max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    /// <summary>Sets an int value within bounds.</summary>
    /// <param name="current">Reference to the current value.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="value">The new value to set.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref int current, int min, int max, int value)
    {
        current = Clamp(value, min, max);
    }

    /// <summary>Normalizes the current int value to be within bounds.</summary>
    /// <param name="current">Reference to the current value.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Normalize(ref int current, int min, int max)
    {
        current = Clamp(current, min, max);
    }

    /// <summary>Checks if the int value is full (at max).</summary>
    /// <param name="current">The current value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>True if full; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(int current, int max)
    {
        return current == max;
    }

    /// <summary>Checks if the int value is empty (at min).</summary>
    /// <param name="current">The current value.</param>
    /// <param name="min">The minimum value.</param>
    /// <returns>True if empty; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(int current, int min)
    {
        return current == min;
    }

    /// <summary>Calculates the ratio of the current int value within the range.</summary>
    /// <param name="current">The current value.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The ratio between 0.0 and 1.0.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetRatio(int current, int min, int max)
    {
        var range = max - min;
        return range == 0 ? 0.0 : (double)(current - min) / range;
    }

    /// <summary>Calculates the range (max - min) for int values.</summary>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The range.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetRange(int min, int max)
    {
        return max - min;
    }

    /// <summary>Calculates the remaining amount to reach max for int values.</summary>
    /// <param name="current">The current value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The remaining amount.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetRemaining(int current, int max)
    {
        return max - current;
    }

    /// <summary>Clamps a byte value up to max.</summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The clamped value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte Clamp(byte value, byte max)
    {
        return value > max ? max : value;
    }

    /// <summary>Clamps an int value to a byte range [0, max].</summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The clamped byte value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte Clamp(int value, byte max)
    {
        if (value > max) return max;
        if (value < 0) return 0;
        return (byte)value;
    }

    /// <summary>Sets a byte value within bounds.</summary>
    /// <param name="current">Reference to the current value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="value">The new value to set.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref byte current, byte max, byte value)
    {
        current = Clamp(value, max);
    }

    /// <summary>Normalizes the current byte value to be within bounds.</summary>
    /// <param name="current">Reference to the current value.</param>
    /// <param name="max">The maximum value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Normalize(ref byte current, byte max)
    {
        current = Clamp(current, max);
    }

    /// <summary>Checks if the byte value is full (at max).</summary>
    /// <param name="current">The current value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>True if full; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(byte current, byte max)
    {
        return current == max;
    }

    /// <summary>Checks if the byte value is empty (0).</summary>
    /// <param name="current">The current value.</param>
    /// <returns>True if empty; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(byte current)
    {
        return current == 0;
    }

    /// <summary>Calculates the ratio of the current byte value within the range [0, max].</summary>
    /// <param name="current">The current value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The ratio between 0.0 and 1.0.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetRatio(byte current, byte max)
    {
        return max == 0 ? 0.0 : (double)current / max;
    }

    /// <summary>Returns the range for byte values (max).</summary>
    /// <param name="max">The maximum value.</param>
    /// <returns>The range.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte GetRange(byte max)
    {
        return max;
    }

    /// <summary>Calculates the remaining amount to reach max for byte values.</summary>
    /// <param name="current">The current value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The remaining amount.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte GetRemaining(byte current, byte max)
    {
        return (byte)(max - current);
    }

    /// <summary>Adds an amount to the current value, clamping within bounds.</summary>
    /// <param name="current">The current value.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="amount">The amount to add.</param>
    /// <returns>The new clamped value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Add(int current, int min, int max, int amount)
    {
        var result = (long)current + amount;
        if (result > max) result = max;
        else if (result < min) result = min;
        return (int)result;
    }
}