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
    /// <param name="result">The amount refilled.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Refill(ref int current, in int capacity, ref int reserve, out int result)
    {
        var missing = capacity - current;
        if (missing <= 0 || reserve <= 0)
        {
            result = 0;
            return;
        }

        var toRefill = reserve < missing ? reserve : missing;
        current += toRefill;
        reserve -= toRefill;

        result = toRefill;
    }
}