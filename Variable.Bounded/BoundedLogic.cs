namespace Variable.Bounded;

/// <summary>
///     Pure primitive logic for bounding values.
///     All methods operate on primitives only - NO STRUCTS.
/// </summary>
public static class BoundedLogic
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Clamp(float value, float min, float max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref float current, float min, float max, float value)
    {
        current = Clamp(value, min, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Normalize(ref float current, float min, float max)
    {
        current = Clamp(current, min, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(float current, float max, float tolerance)
    {
        return current >= max - tolerance;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(float current, float min, float tolerance)
    {
        return current <= min + tolerance;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetRatio(float current, float min, float max)
    {
        var range = max - min;
        return Math.Abs(range) < MathConstants.Tolerance ? 0.0 : (current - min) / range;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetRange(float min, float max)
    {
        return max - min;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetRemaining(float current, float max)
    {
        return max - current;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Clamp(int value, int min, int max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref int current, int min, int max, int value)
    {
        current = Clamp(value, min, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Normalize(ref int current, int min, int max)
    {
        current = Clamp(current, min, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(int current, int max)
    {
        return current == max;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(int current, int min)
    {
        return current == min;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetRatio(int current, int min, int max)
    {
        var range = max - min;
        return range == 0 ? 0.0 : (double)(current - min) / range;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetRange(int min, int max)
    {
        return max - min;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetRemaining(int current, int max)
    {
        return max - current;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte Clamp(byte value, byte max)
    {
        return value > max ? max : value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte Clamp(int value, byte max)
    {
        if (value > max) return max;
        if (value < 0) return 0;
        return (byte)value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref byte current, byte max, byte value)
    {
        current = Clamp(value, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Normalize(ref byte current, byte max)
    {
        current = Clamp(current, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(byte current, byte max)
    {
        return current == max;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(byte current)
    {
        return current == 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetRatio(byte current, byte max)
    {
        return max == 0 ? 0.0 : (double)current / max;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte GetRange(byte max)
    {
        return max;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte GetRemaining(byte current, byte max)
    {
        return (byte)(max - current);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Add(int current, int min, int max, int amount)
    {
        var result = (long)current + amount;
        if (result > max) result = max;
        else if (result < min) result = min;
        return (int)result;
    }
}