namespace Variable.Reservoir;

/// <summary>
///     Provides static methods for reservoir refill and transfer operations (Int version).
///     All methods operate on primitives only - NO STRUCTS.
/// </summary>
public static partial class ReservoirLogic
{
    /// <summary>
    ///     Refills the current volume from the reserve.
    ///     Transfers as much as possible from reserve to current, up to capacity.
    /// </summary>
    /// <param name="current">Reference to current volume.</param>
    /// <param name="capacity">Maximum volume capacity.</param>
    /// <param name="reserve">Reference to reserve amount.</param>
    /// <returns>The amount refilled.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Refill(ref int current, int capacity, ref int reserve)
    {
        var missing = capacity - current;
        if (missing <= 0 || reserve <= 0) return 0;

        var toRefill = reserve < missing ? reserve : missing;
        current += toRefill;
        reserve -= toRefill;

        return toRefill;
    }
}