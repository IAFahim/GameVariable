namespace Variable.Random;

/// <summary>
///     Extension methods for RandomState to provide a convenient API.
/// </summary>
public static class RandomExtensions
{
    /// <summary>
    ///     Initializes the random state with a seed.
    /// </summary>
    /// <param name="state">The state to initialize.</param>
    /// <param name="seed">The seed value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Init(ref this RandomState state, int seed)
    {
        RandomLogic.Initialize(in seed, out state.S0, out state.S1, out state.S2, out state.S3);
    }

    /// <summary>
    ///     Generates the next random uint.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Next(ref this RandomState state, out uint result)
    {
        RandomLogic.Next(ref state.S0, ref state.S1, ref state.S2, ref state.S3, out result);
    }

    /// <summary>
    ///     Generates the next random positive int [0, int.MaxValue].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NextInt(ref this RandomState state, out int result)
    {
        RandomLogic.Next(ref state.S0, ref state.S1, ref state.S2, ref state.S3, out uint val);
        result = (int)(val >> 1);
    }

    /// <summary>
    ///     Generates a float between 0.0 (inclusive) and 1.0 (exclusive).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NextFloat(ref this RandomState state, out float result)
    {
        RandomLogic.NextFloat(ref state.S0, ref state.S1, ref state.S2, ref state.S3, out result);
    }

    /// <summary>
    ///     Generates an int within range [min, max).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Range(ref this RandomState state, int min, int max, out int result)
    {
        RandomLogic.Range(ref state.S0, ref state.S1, ref state.S2, ref state.S3, in min, in max, out result);
    }

    /// <summary>
    ///     Returns true based on probability (0.0 to 1.0).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Chance(ref this RandomState state, float probability, out bool result)
    {
        RandomLogic.Chance(ref state.S0, ref state.S1, ref state.S2, ref state.S3, in probability, out result);
    }
}
