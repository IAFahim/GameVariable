namespace Variable.Random;

/// <summary>
///     Pure logic for random number generation.
///     Implements Xoshiro128** algorithm.
/// </summary>
public static class RandomLogic
{
    private const float FloatMultiplier = 1.0f / (1u << 24); // 1 / 2^24

    /// <summary>
    ///     Initializes the state using a seed (SplitMix64-like).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Initialize(in int seed, out uint s0, out uint s1, out uint s2, out uint s3)
    {
        // Use a 64-bit state for seeding derived from the input seed
        ulong z = (ulong)seed;

        // Run SplitMix64 4 times to generate the 4 state integers
        NextSplitMix64(ref z, out ulong r0);
        NextSplitMix64(ref z, out ulong r1);
        NextSplitMix64(ref z, out ulong r2);
        NextSplitMix64(ref z, out ulong r3);

        s0 = (uint)r0;
        s1 = (uint)r1;
        s2 = (uint)r2;
        s3 = (uint)r3;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void NextSplitMix64(ref ulong x, out ulong result)
    {
        x += 0x9e3779b97f4a7c15;
        ulong z = x;
        z = (z ^ (z >> 30)) * 0xbf58476d1ce4e5b9;
        z = (z ^ (z >> 27)) * 0x94d049bb133111eb;
        result = z ^ (z >> 31);
    }

    /// <summary>
    ///     Generates the next random uint using Xoshiro128**.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Next(ref uint s0, ref uint s1, ref uint s2, ref uint s3, out uint result)
    {
        result = Rotl(s1 * 5, 7) * 9;

        uint t = s1 << 9;

        s2 ^= s0;
        s3 ^= s1;
        s1 ^= s2;
        s0 ^= s3;

        s2 ^= t;

        s3 = Rotl(s3, 11);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint Rotl(uint x, int k)
    {
        return (x << k) | (x >> (32 - k));
    }

    /// <summary>
    ///     Generates a float between 0.0 (inclusive) and 1.0 (exclusive).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NextFloat(ref uint s0, ref uint s1, ref uint s2, ref uint s3, out float result)
    {
        Next(ref s0, ref s1, ref s2, ref s3, out uint next);
        result = (next >> 8) * FloatMultiplier;
    }

    /// <summary>
    ///     Generates an int within range [min, max).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Range(ref uint s0, ref uint s1, ref uint s2, ref uint s3, in int min, in int max, out int result)
    {
        if (min >= max)
        {
            result = min;
            return;
        }

        NextFloat(ref s0, ref s1, ref s2, ref s3, out float f);
        result = min + (int)(f * (max - min));
    }

    /// <summary>
    ///     Returns true based on probability (0.0 to 1.0).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Chance(ref uint s0, ref uint s1, ref uint s2, ref uint s3, in float probability, out bool result)
    {
        NextFloat(ref s0, ref s1, ref s2, ref s3, out float val);
        result = val < probability;
    }
}
