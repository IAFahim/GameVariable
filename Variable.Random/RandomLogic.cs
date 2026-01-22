namespace Variable.Random;

/// <summary>
///     Pure logic for the PCG-XSH-RR random number generator.
///     Stateless, static, and allocation-free.
/// </summary>
public static class RandomLogic
{
    /// <summary>
    ///     Initializes the generator state.
    /// </summary>
    /// <param name="seed">The seed value.</param>
    /// <param name="sequence">The sequence/stream identifier.</param>
    /// <param name="state">The initialized state.</param>
    /// <param name="increment">The initialized increment.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Initialize(ulong seed, ulong sequence, out ulong state, out ulong increment)
    {
        state = 0UL;
        increment = (sequence << 1) | 1UL;
        Next(ref state, increment, out _);
        state += seed;
        Next(ref state, increment, out _);
    }

    /// <summary>
    ///     Generates the next random unsigned integer.
    /// </summary>
    /// <param name="state">The current state (updated).</param>
    /// <param name="increment">The increment value.</param>
    /// <param name="result">The generated random number.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Next(ref ulong state, ulong increment, out uint result)
    {
        var oldState = state;
        state = oldState * 6364136223846793005UL + increment;
        var xorshifted = (uint)(((oldState >> 18) ^ oldState) >> 27);
        var rot = (int)(oldState >> 59);
        result = (xorshifted >> rot) | (xorshifted << ((-rot) & 31));
    }

    /// <summary>
    ///     Generates the next random float in range [0, 1).
    /// </summary>
    /// <param name="state">The current state (updated).</param>
    /// <param name="increment">The increment value.</param>
    /// <param name="result">The generated random float.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NextFloat(ref ulong state, ulong increment, out float result)
    {
        Next(ref state, increment, out var val);
        // 1.0 / 4294967296.0 = 2.3283064365386963E-10
        result = val * 2.3283064365386963E-10f;
    }

    /// <summary>
    ///     Generates a random integer within a range [min, max).
    /// </summary>
    /// <param name="state">The current state (updated).</param>
    /// <param name="increment">The increment value.</param>
    /// <param name="min">The inclusive lower bound.</param>
    /// <param name="max">The exclusive upper bound.</param>
    /// <param name="result">The generated random integer.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Range(ref ulong state, ulong increment, int min, int max, out int result)
    {
        if (min >= max)
        {
            result = min;
            return;
        }

        var range = (uint)((long)max - min);
        Next(ref state, increment, out var val);

        // Unbiased generation using rejection sampling would be better,
        // but for game dev, simple modulo is often acceptable if range is small compared to uint.max.
        // However, to be "Standard Library" quality, let's do it slightly better or just modulo.
        // PCG's bounded_rand typically uses threshold rejection.

        // Simple modulo:
        // result = min + (int)(val % range);

        // Let's implement basic threshold rejection for uniformity.
        var threshold = (uint)((0x100000000UL - range) % range);
        while (val < threshold)
        {
             Next(ref state, increment, out val);
        }

        result = min + (int)(val % range);
    }

    /// <summary>
    ///     Generates a random float within a range [min, max).
    /// </summary>
    /// <param name="state">The current state (updated).</param>
    /// <param name="increment">The increment value.</param>
    /// <param name="min">The inclusive lower bound.</param>
    /// <param name="max">The inclusive upper bound.</param>
    /// <param name="result">The generated random float.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Range(ref ulong state, ulong increment, float min, float max, out float result)
    {
        if (min >= max)
        {
            result = min;
            return;
        }

        NextFloat(ref state, increment, out var t);
        result = min + t * (max - min);
    }

    /// <summary>
    ///     Performs a coin flip (Bernoulli trial).
    /// </summary>
    /// <param name="state">The current state (updated).</param>
    /// <param name="increment">The increment value.</param>
    /// <param name="probability">The probability of success (0.0 to 1.0).</param>
    /// <param name="result">True if successful.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CoinFlip(ref ulong state, ulong increment, float probability, out bool result)
    {
        if (probability <= 0f) { result = false; return; }
        if (probability >= 1f) { result = true; return; }

        NextFloat(ref state, increment, out var val);
        result = val < probability;
    }
}
