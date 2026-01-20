namespace Variable.Random;

/// <summary>
///     Core logic for the Xoshiro128** random number generator.
///     Stateless, static methods for high-performance RNG.
/// </summary>
public static class RandomLogic
{
    private const float FloatMultiplier = 1.0f / (1u << 24); // 1 / 16777216

    /// <summary>
    ///     Initializes the state using a SplitMix-style mixer.
    ///     This ensures that even adjacent seeds produce widely different states.
    /// </summary>
    /// <param name="seed">The initialization seed.</param>
    /// <param name="state">The initialized state.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Initialize(uint seed, out RandomState state)
    {
        // SplitMix32 implementation to seed the 128-bit state
        uint x = seed;

        state.S0 = Mix(ref x);
        state.S1 = Mix(ref x);
        state.S2 = Mix(ref x);
        state.S3 = Mix(ref x);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint Mix(ref uint x)
    {
        uint z = (x += 0x9E3779B9);
        z = (z ^ (z >> 16)) * 0x85EBCA6B;
        z = (z ^ (z >> 13)) * 0xC2B2AE35;
        return z ^ (z >> 16);
    }

    /// <summary>
    ///     Generates the next random uint and updates the state.
    ///     Algorithm: Xoshiro128** 1.0
    /// </summary>
    /// <param name="state">The current RNG state.</param>
    /// <param name="result">The generated random uint.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NextUInt(ref RandomState state, out uint result)
    {
        // result = rotl(s1 * 5, 7) * 9
        result = RotL(state.S1 * 5, 7) * 9;

        // t = s1 << 9
        uint t = state.S1 << 9;

        state.S2 ^= state.S0;
        state.S3 ^= state.S1;
        state.S1 ^= state.S2;
        state.S0 ^= state.S3;

        state.S2 ^= t;

        // s3 = rotl(s3, 11)
        state.S3 = RotL(state.S3, 11);
    }

    /// <summary>
    ///     Generates the next random float in the range [0.0, 1.0).
    /// </summary>
    /// <param name="state">The current RNG state.</param>
    /// <param name="result">The generated random float.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NextFloat(ref RandomState state, out float result)
    {
        NextUInt(ref state, out uint u);
        // Use top 24 bits for the mantissa
        result = (u >> 8) * FloatMultiplier;
    }

    /// <summary>
    ///     Generates the next random boolean.
    /// </summary>
    /// <param name="state">The current RNG state.</param>
    /// <param name="result">The generated random boolean.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NextBool(ref RandomState state, out bool result)
    {
        NextUInt(ref state, out uint u);
        // Use the sign bit
        result = (int)u < 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint RotL(uint x, int k)
    {
        return (x << k) | (x >> (32 - k));
    }
}
