namespace Variable.Random;

/// <summary>
///     User-friendly extensions for <see cref="PcgRandom" />.
///     Provides a standard API for generating random numbers.
/// </summary>
public static class RandomExtensions
{
    /// <summary>
    ///     Returns a random unsigned integer.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Next(ref this PcgRandom rng)
    {
        RandomLogic.Next(ref rng.State, rng.Increment, out var result);
        return result;
    }

    /// <summary>
    ///     Returns a random float in the range [0.0, 1.0).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float NextFloat(ref this PcgRandom rng)
    {
        RandomLogic.NextFloat(ref rng.State, rng.Increment, out var result);
        return result;
    }

    /// <summary>
    ///     Returns a random integer within the range [min, max).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Range(ref this PcgRandom rng, int min, int max)
    {
        RandomLogic.Range(ref rng.State, rng.Increment, min, max, out var result);
        return result;
    }

    /// <summary>
    ///     Returns a random integer within the range [0, max).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Range(ref this PcgRandom rng, int max)
    {
        RandomLogic.Range(ref rng.State, rng.Increment, 0, max, out var result);
        return result;
    }

    /// <summary>
    ///     Returns a random float within the range [min, max).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Range(ref this PcgRandom rng, float min, float max)
    {
        RandomLogic.Range(ref rng.State, rng.Increment, min, max, out var result);
        return result;
    }

    /// <summary>
    ///     Returns true with the specified probability.
    /// </summary>
    /// <param name="rng">The random generator.</param>
    /// <param name="probability">Probability of returning true (0.0 to 1.0).</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Chance(ref this PcgRandom rng, float probability)
    {
        RandomLogic.CoinFlip(ref rng.State, rng.Increment, probability, out var result);
        return result;
    }

    /// <summary>
    ///     Shuffles a list in-place using the Fisher-Yates algorithm.
    /// </summary>
    public static void Shuffle<T>(ref this PcgRandom rng, IList<T> list)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = rng.Range(0, n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    /// <summary>
    ///     Returns a random element from a list.
    /// </summary>
    public static T PickRandom<T>(ref this PcgRandom rng, IList<T> list)
    {
        if (list.Count == 0) throw new ArgumentException("List is empty", nameof(list));
        var index = rng.Range(0, list.Count);
        return list[index];
    }

    /// <summary>
    ///     Returns a random element from a span.
    /// </summary>
    public static T PickRandom<T>(ref this PcgRandom rng, ReadOnlySpan<T> span)
    {
        if (span.Length == 0) throw new ArgumentException("Span is empty", nameof(span));
        var index = rng.Range(0, span.Length);
        return span[index];
    }
}
