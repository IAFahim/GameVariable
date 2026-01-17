namespace Variable.Random;

/// <summary>
///     Extension methods for <see cref="RandomState" />.
///     Provides the user-friendly API for random number generation.
/// </summary>
public static class RandomExtensions
{
    /// <summary>
    ///     Returns the next random uint.
    ///     Updates the state.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Next(ref this RandomState state)
    {
        RandomLogic.Next(ref state.S0, ref state.S1, ref state.S2, ref state.S3, out uint result);
        return result;
    }

    /// <summary>
    ///     Returns the next random float in range [0, 1).
    ///     Updates the state.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float NextFloat(ref this RandomState state)
    {
        RandomLogic.Next(ref state.S0, ref state.S1, ref state.S2, ref state.S3, out uint raw);
        RandomLogic.NextFloat(in raw, out float result);
        return result;
    }

    /// <summary>
    ///     Returns a random integer in range [min, max).
    ///     Updates the state.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Range(ref this RandomState state, int min, int max)
    {
        if (min >= max) return min;

        RandomLogic.Next(ref state.S0, ref state.S1, ref state.S2, ref state.S3, out uint raw);
        RandomLogic.Range(in raw, in min, in max, out int result);
        return result;
    }

    /// <summary>
    ///     Returns a random boolean.
    ///     Updates the state.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NextBool(ref this RandomState state)
    {
        // Check the highest bit or lowest bit.
        // Lowest bit of Xoshiro is decent.
        RandomLogic.Next(ref state.S0, ref state.S1, ref state.S2, ref state.S3, out uint raw);
        return (raw & 1) == 1;
    }

    /// <summary>
    ///     Returns a random vector-like point inside a unit circle (2D).
    ///     Updates the state.
    ///     Returns tuple (x, y).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (float x, float y) InsideUnitCircle(ref this RandomState state)
    {
        // Rejection sampling or polar coordinates?
        // Polar is safer for uniformity if we use sqrt(r).
        // theta = 2 * PI * u
        // r = sqrt(v)
        // x = r * cos(theta)
        // y = r * sin(theta)

        // Assuming we have access to System.MathF
        float u = state.NextFloat();
        float v = state.NextFloat();

        float theta = u * 6.28318530718f; // 2 * PI
        float r = MathF.Sqrt(v);

        return (r * MathF.Cos(theta), r * MathF.Sin(theta));
    }
}
