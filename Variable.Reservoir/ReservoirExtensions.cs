namespace Variable.Reservoir;

/// <summary>
///     Extension methods for <see cref="ReservoirFloat" /> and <see cref="ReservoirInt" /> providing logic operations.
///     These methods bridge from structs to primitive-only logic.
/// </summary>
public static class ReservoirExtensions
{
    /// <summary>
    ///     Refills the volume from the reserve.
    /// </summary>
    /// <param name="reservoir">The reservoir to refill.</param>
    /// <returns>The amount actually refilled.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Refill(ref this ReservoirFloat reservoir)
    {
        // Decompose struct into primitives for logic
        return ReservoirLogic.Refill(
            ref reservoir.Volume.Current,
            reservoir.Volume.Max,
            ref reservoir.Reserve
        );
    }

    /// <summary>
    ///     Refills the volume from the reserve.
    /// </summary>
    /// <param name="reservoir">The reservoir to refill.</param>
    /// <returns>The amount actually refilled.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Refill(ref this ReservoirInt reservoir)
    {
        // Decompose struct into primitives for logic
        return ReservoirLogic.Refill(
            ref reservoir.Volume.Current,
            reservoir.Volume.Max,
            ref reservoir.Reserve
        );
    }
}