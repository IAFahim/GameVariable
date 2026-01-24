namespace Variable.Random;

/// <summary>
///     Core logic for the Random generator.
///     Contains the stateless Xorshift32 implementation.
/// </summary>
public static class RngLogic
{
    // 1.0 / 2^24. Used to map 24 bits of randomness to [0, 1).
    private const float FloatScale = 1.0f / 16777216.0f;

    /// <summary>
    ///     Generates the next random uint using Xorshift32 algorithm.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Next(in uint state, out uint newState, out uint result)
    {
        uint x = state;
        // Xorshift32
        x ^= x << 13;
        x ^= x >> 17;
        x ^= x << 5;

        newState = x;
        result = x;
    }

    /// <summary>
    ///     Generates a random float in range [0, 1).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NextFloat(in uint state, out uint newState, out float result)
    {
        Next(in state, out newState, out uint raw);
        // Use top 24 bits for float mantissa to ensure uniform distribution
        result = (raw >> 8) * FloatScale;
    }

    /// <summary>
    ///     Generates a random integer in range [min, max).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Range(in uint state, in int min, in int max, out uint newState, out int result)
    {
        if (min >= max)
        {
            // Advance state anyway to maintain determinism cadence if needed,
            // or just return to avoid unused out param issue.
            // Let's advance state to be consistent with "Next call happened".
            Next(in state, out newState, out _);
            result = min;
            return;
        }

        Next(in state, out newState, out uint raw);

        // Calculate range in 64-bit to avoid overflow if min is negative
        uint range = (uint)((long)max - min);

        // Simple modulus. Note: has slight bias if range is not power of 2,
        // but acceptable for high-performance game logic.
        uint offset = raw % range;

        result = min + (int)offset;
    }
}
