namespace Variable.Inventory;

/// <summary>
///     Provides static logic for inventory management operations.
/// </summary>
public static partial class InventoryLogic
{
    /// <summary>
    ///     Attempts to add as much of the specified amount as possible to the inventory, up to the maximum capacity.
    /// </summary>
    /// <param name="current">The reference to the current quantity variable.</param>
    /// <param name="amount">The amount to attempt to add.</param>
    /// <param name="max">The maximum capacity.</param>
    /// <param name="actualAdded">The actual amount added to the inventory.</param>
    /// <param name="overflow">The amount that could not be added.</param>
    /// <param name="tolerance">The tolerance for floating point comparisons.</param>
    /// <returns>True if any amount was added; otherwise, false.</returns>
    public static bool TryAddPartial(ref float current, float amount, float max, out float actualAdded,
        out float overflow, float tolerance = MathConstants.Tolerance)
    {
        if (amount <= tolerance)
        {
            actualAdded = 0f;
            overflow = 0f;
            return false;
        }

        var space = max - current;
        if (space < 0f) space = 0f;

        var toAdd = amount < space ? amount : space;
        overflow = amount - toAdd;

        current += toAdd;
        if (max - current < tolerance) current = max;

        actualAdded = toAdd;
        return toAdd > tolerance;
    }

    /// <summary>
    ///     Adds as much as possible to inventory, returning the amount added and outputting overflow.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float AddPartial(ref float current, float amount, float max, out float overflow,
        float tolerance = MathConstants.Tolerance)
    {
        TryAddPartial(ref current, amount, max, out var actualAdded, out overflow, tolerance);
        return actualAdded;
    }
}