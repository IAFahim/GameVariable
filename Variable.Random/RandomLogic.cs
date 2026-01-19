namespace Variable.Random;

/// <summary>
///     Core logic for the Xoshiro128** random number generator.
///     <para>
///         This class is stateless. It operates on <see cref="RandomState"/> passed by reference.
///     </para>
/// </summary>
public static class RandomLogic
{
    /// <summary>
    ///     Initializes the random state using a seed.
    ///     Uses a SplitMix-style algorithm to populate the 128-bit state from a single integer.
    /// </summary>
    /// <param name="state">The state to initialize.</param>
    /// <param name="seed">The seed value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Initialize(ref RandomState state, int seed)
    {
        // Use a 64-bit mixer to generate high-quality initial state
        ulong z = (ulong)seed;

        // Generate S0
        z = (z + 0x9e3779b97f4a7c15);
        ulong t = (z ^ (z >> 30)) * 0xbf58476d1ce4e5b9;
        t = (t ^ (t >> 27)) * 0x94d049bb133111eb;
        state.S0 = (uint)(t ^ (t >> 31));

        // Generate S1
        z = (z + 0x9e3779b97f4a7c15);
        t = (z ^ (z >> 30)) * 0xbf58476d1ce4e5b9;
        t = (t ^ (t >> 27)) * 0x94d049bb133111eb;
        state.S1 = (uint)(t ^ (t >> 31));

        // Generate S2
        z = (z + 0x9e3779b97f4a7c15);
        t = (z ^ (z >> 30)) * 0xbf58476d1ce4e5b9;
        t = (t ^ (t >> 27)) * 0x94d049bb133111eb;
        state.S2 = (uint)(t ^ (t >> 31));

        // Generate S3
        z = (z + 0x9e3779b97f4a7c15);
        t = (z ^ (z >> 30)) * 0xbf58476d1ce4e5b9;
        t = (t ^ (t >> 27)) * 0x94d049bb133111eb;
        state.S3 = (uint)(t ^ (t >> 31));
    }

    /// <summary>
    ///     Generates the next random unsigned integer.
    /// </summary>
    /// <param name="state">The random state.</param>
    /// <param name="result">The generated uint.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NextUInt(ref RandomState state, out uint result)
    {
        // Xoshiro128** algorithm
        // https://prng.di.unimi.it/xoshiro128starstar.c

        uint s0 = state.S0;
        uint s1 = state.S1;
        uint s2 = state.S2;
        uint s3 = state.S3;

        result = Rotl(s1 * 5, 7) * 9;

        uint t = s1 << 9;

        state.S2 ^= s0;
        state.S3 ^= s1;
        state.S1 ^= state.S2;
        state.S0 ^= state.S3;

        state.S2 ^= t;
        state.S3 = Rotl(state.S3, 11);
    }

    /// <summary>
    ///     Generates a random float in the range [0.0, 1.0).
    /// </summary>
    /// <param name="state">The random state.</param>
    /// <param name="result">The generated float.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NextFloat(ref RandomState state, out float result)
    {
        NextUInt(ref state, out uint u);
        // Standard conversion: construct a float from the upper 24 bits
        result = (u >> 8) * (1.0f / 16777216.0f);
    }

    /// <summary>
    ///     Generates a random boolean.
    /// </summary>
    /// <param name="state">The random state.</param>
    /// <param name="result">True or False.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NextBool(ref RandomState state, out bool result)
    {
        NextUInt(ref state, out uint u);
        result = (u & 1) == 0;
    }

    /// <summary>
    ///     Generates a random integer in the range [min, max).
    /// </summary>
    /// <param name="state">The random state.</param>
    /// <param name="min">The inclusive lower bound.</param>
    /// <param name="max">The exclusive upper bound.</param>
    /// <param name="result">The generated integer.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NextInt(ref RandomState state, int min, int max, out int result)
    {
        if (min >= max)
        {
            result = min;
            return;
        }

        // Use NextFloat implementation to avoid modulo bias and handle negative ranges easily
        NextFloat(ref state, out float f);
        result = min + (int)(f * (max - min));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint Rotl(uint x, int k)
    {
        return (x << k) | (x >> (32 - k));
    }
}
