namespace Variable.Random;

/// <summary>
///     Provides static methods for random number generation logic using Xoshiro128**.
///     All methods operate on primitives only - NO STRUCTS.
/// </summary>
public static class RandomLogic
{
    /// <summary>
    ///     Generates the next random uint using Xoshiro128** algorithm.
    ///     Updates the state in-place via ref parameters.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Next(ref uint s0, ref uint s1, ref uint s2, ref uint s3, out uint result)
    {
        // xoshiro128** 1.0
        // result = rotl(s0 * 5, 7) * 9;

        // Manual RotateLeft for netstandard2.1 compatibility without extra deps
        // (s0 * 5)
        uint v = s0 * 5;
        // rotl(v, 7) -> (v << 7) | (v >> 25)
        uint rot = (v << 7) | (v >> 25);
        result = rot * 9;

        uint t = s1 << 9;

        s2 ^= s0;
        s3 ^= s1;
        s1 ^= s2;
        s0 ^= s3;

        s2 ^= t;

        // s3 = rotl(s3, 11) -> (s3 << 11) | (s3 >> 21)
        s3 = (s3 << 11) | (s3 >> 21);
    }

    /// <summary>
    ///     Generates a float in range [0, 1) from a random uint.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NextFloat(in uint input, out float result)
    {
        // Standard trick: (x >> 9) * (1.0f / 8388608.0f) for 23 bits of mantissa
        // 0x3f800000 is 1.0f in hex representation.
        // We can mask the top 9 bits to 0 and set the exponent to 127 (1.0), then subtract 1.0.
        // Or just divide. Division is fast enough usually, but multiplication is better.
        // 1.0 / 2^32 = 2.3283064365386962890625e-10

        // A common fast way for [0,1) with 23 bits of precision:
        // result = (input >> 8) * (1.0f / 16777216.0f);

        result = (input >> 8) * 5.9604645E-08f;
    }

    /// <summary>
    ///     Generates an integer in range [min, max).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Range(in uint input, in int min, in int max, out int result)
    {
        // Avoid modulo bias by rejection sampling?
        // For game dev "good enough" performance, we often use floating point scaling or simple modulo if strict uniformity isn't critical.
        // Or: ((ulong)input * (ulong)(max - min)) >> 32
        // This is Lemire's method, very fast and uniform enough.

        uint range = (uint)(max - min);
        uint scaled = (uint)(((ulong)input * (ulong)range) >> 32);
        result = min + (int)scaled;
    }
}
