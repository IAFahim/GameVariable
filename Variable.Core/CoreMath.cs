namespace Variable.Core;

/// <summary>
///     Core mathematical operations shared across all Variable packages.
///     Provides Burst-compatible primitive-only methods to eliminate code duplication.
/// </summary>
[SkipLocalsInit]
public static class CoreMath
{
    /// <summary>Clamps a float value between min and max.</summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="result">The clamped value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clamp(float value, float min, float max, out float result)
    {
        result = Math.Clamp(value, min, max);
    }

    /// <summary>Clamps an int value between min and max.</summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="result">The clamped value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clamp(int value, int min, int max, out int result)
    {
        result = Math.Clamp(value, min, max);
    }

    /// <summary>Clamps a long value between min and max.</summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="result">The clamped value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clamp(long value, long min, long max, out long result)
    {
        result = Math.Clamp(value, min, max);
    }

    /// <summary>Clamps a byte value to a maximum.</summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="result">The clamped value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clamp(byte value, byte max, out byte result)
    {
        result = Math.Min(value, max);
    }

    /// <summary>Clamps an int value to a byte range [0, max].</summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="result">The clamped byte value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clamp(int value, byte max, out byte result)
    {
        result = (byte)Math.Clamp(value, 0, max);
    }

    /// <summary>Clamps a double value between min and max.</summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="result">The clamped value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clamp(double value, double min, double max, out double result)
    {
        result = Math.Clamp(value, min, max);
    }

    /// <summary>Returns the smaller of two float values.</summary>
    /// <param name="a">First value.</param>
    /// <param name="b">Second value.</param>
    /// <param name="result">The smaller value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Min(float a, float b, out float result)
    {
        result = MathF.Min(a, b);
    }

    /// <summary>Returns the smaller of two int values.</summary>
    /// <param name="a">First value.</param>
    /// <param name="b">Second value.</param>
    /// <param name="result">The smaller value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Min(int a, int b, out int result)
    {
        result = Math.Min(a, b);
    }

    /// <summary>Returns the smaller of two long values.</summary>
    /// <param name="a">First value.</param>
    /// <param name="b">Second value.</param>
    /// <param name="result">The smaller value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Min(long a, long b, out long result)
    {
        result = Math.Min(a, b);
    }

    /// <summary>Returns the larger of two float values.</summary>
    /// <param name="a">First value.</param>
    /// <param name="b">Second value.</param>
    /// <param name="result">The larger value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Max(float a, float b, out float result)
    {
        result = MathF.Max(a, b);
    }

    /// <summary>Returns the larger of two int values.</summary>
    /// <param name="a">First value.</param>
    /// <param name="b">Second value.</param>
    /// <param name="result">The larger value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Max(int a, int b, out int result)
    {
        result = Math.Max(a, b);
    }

    /// <summary>Returns the larger of two long values.</summary>
    /// <param name="a">First value.</param>
    /// <param name="b">Second value.</param>
    /// <param name="result">The larger value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Max(long a, long b, out long result)
    {
        result = Math.Max(a, b);
    }

    /// <summary>Returns the absolute value of a float.</summary>
    /// <param name="value">The value.</param>
    /// <param name="result">The absolute value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Abs(float value, out float result)
    {
        result = MathF.Abs(value);
    }

    /// <summary>Returns the absolute value of an int.</summary>
    /// <param name="value">The value.</param>
    /// <param name="result">The absolute value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Abs(int value, out int result)
    {
        result = Math.Abs(value);
    }
}
