namespace Variable.Random;

/// <summary>
///     Extension methods for <see cref="RngState"/> to provide a convenient API.
/// </summary>
public static class RngExtensions
{
    /// <summary>
    ///     Returns the next random unsigned integer and updates the state.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Next(ref this RngState rng)
    {
        RngLogic.Next(in rng.State, out rng.State, out uint result);
        return result;
    }

    /// <summary>
    ///     Returns the next random float in range [0, 1) and updates the state.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float NextFloat(ref this RngState rng)
    {
        RngLogic.NextFloat(in rng.State, out rng.State, out float result);
        return result;
    }

    /// <summary>
    ///     Returns a random integer in range [min, max) and updates the state.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Range(ref this RngState rng, int min, int max)
    {
        RngLogic.Range(in rng.State, in min, in max, out rng.State, out int result);
        return result;
    }
}
