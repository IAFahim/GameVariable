namespace Variable.Random;

/// <summary>
///     Extension methods for <see cref="RandomState" /> providing a convenient API.
/// </summary>
public static class RandomExtensions
{
    /// <summary>
    ///     Returns the next random uint.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint NextUInt(ref this RandomState state)
    {
        RandomLogic.NextUInt(ref state, out var result);
        return result;
    }

    /// <summary>
    ///     Returns the next random float in the range [0.0, 1.0).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float NextFloat(ref this RandomState state)
    {
        RandomLogic.NextFloat(ref state, out var result);
        return result;
    }

    /// <summary>
    ///     Returns the next random boolean.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NextBool(ref this RandomState state)
    {
        RandomLogic.NextBool(ref state, out var result);
        return result;
    }

    /// <summary>
    ///     Returns a random integer in the range [0, max).
    ///     <paramref name="max"/> must be positive.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int NextInt(ref this RandomState state, int max)
    {
        if (max <= 0) return 0;

        RandomLogic.NextUInt(ref state, out var u);
        // Fast mapping using fixed-point multiplication
        return (int)(((ulong)u * (ulong)max) >> 32);
    }

    /// <summary>
    ///     Returns a random integer in the range [min, max).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int NextInt(ref this RandomState state, int min, int max)
    {
        if (max <= min) return min;

        uint range = (uint)(max - min);
        RandomLogic.NextUInt(ref state, out var u);
        return min + (int)(((ulong)u * (ulong)range) >> 32);
    }
}
