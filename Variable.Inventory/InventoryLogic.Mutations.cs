namespace Variable.Inventory;

/// <summary>
///     Mutation operations for modifying inventory state.
///     These methods modify the current quantity via ref parameters.
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

    /// <summary>
    ///     Attempts to remove as much of the specified amount as possible from the inventory.
    /// </summary>
    /// <param name="current">The reference to the current quantity variable.</param>
    /// <param name="amount">The amount to attempt to remove.</param>
    /// <param name="actualRemoved">The actual amount removed from the inventory.</param>
    /// <param name="tolerance">The tolerance for floating point comparisons.</param>
    /// <returns>True if any amount was removed; otherwise, false.</returns>
    public static bool TryRemovePartial(ref float current, float amount, out float actualRemoved,
        float tolerance = MathConstants.Tolerance)
    {
        if (amount <= tolerance || current <= tolerance)
        {
            actualRemoved = 0f;
            return false;
        }

        var toRemove = current < amount ? current : amount;

        current -= toRemove;
        if (current < tolerance) current = 0f;

        actualRemoved = toRemove;
        return true;
    }

    /// <summary>
    ///     Removes as much as possible from inventory, returning the amount removed.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float RemovePartial(ref float current, float amount, float tolerance = MathConstants.Tolerance)
    {
        TryRemovePartial(ref current, amount, out var actualRemoved, tolerance);
        return actualRemoved;
    }

    /// <summary>
    ///     Sets the inventory quantity to a specific value, clamping it between 0 and the maximum capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref float current, float value, float max, float tolerance = MathConstants.Tolerance)
    {
        if (max - value < tolerance) value = max;
        else if (value < tolerance) value = 0f;

        current = value;
    }

    /// <summary>
    ///     Sets the inventory quantity to a specific value, clamping it between 0 and the maximum capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref int current, int value, int max)
    {
        if (value < 0) value = 0;
        if (value > max) value = max;
        current = value;
    }

    /// <summary>
    ///     Sets the inventory quantity to a specific value, clamping it between 0 and the maximum capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref long current, long value, long max)
    {
        if (value < 0) value = 0;
        if (value > max) value = max;
        current = value;
    }

    /// <summary>
    ///     Sets the inventory quantity to a specific value, clamping it between 0 and the maximum capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref byte current, byte value, byte max)
    {
        if (value > max) value = max;
        current = value;
    }

    /// <summary>
    ///     Clears the inventory, setting the current quantity to 0.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear(ref float current)
    {
        current = 0f;
    }

    /// <summary>
    ///     Clears the inventory, setting the current quantity to 0.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear(ref int current)
    {
        current = 0;
    }

    /// <summary>
    ///     Clears the inventory, setting the current quantity to 0.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear(ref long current)
    {
        current = 0;
    }

    /// <summary>
    ///     Clears the inventory, setting the current quantity to 0.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear(ref byte current)
    {
        current = 0;
    }

    /// <summary>
    ///     Attempts to add an exact amount of items to the inventory.
    /// </summary>
    /// <param name="current">The reference to the current quantity variable.</param>
    /// <param name="amount">The amount to add.</param>
    /// <param name="max">The maximum capacity.</param>
    /// <param name="tolerance">The tolerance for floating point comparisons.</param>
    /// <returns><c>true</c> if the amount was added successfully; otherwise, <c>false</c> if it would exceed capacity.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryAddExact(ref float current, float amount, float max,
        float tolerance = MathConstants.Tolerance)
    {
        if (amount <= tolerance) return true;
        if (current + amount > max + tolerance) return false;

        current += amount;
        if (max - current < tolerance) current = max;
        return true;
    }

    /// <summary>
    ///     Attempts to remove an exact amount of items from the inventory.
    /// </summary>
    /// <param name="current">The reference to the current quantity variable.</param>
    /// <param name="amount">The amount to remove.</param>
    /// <param name="tolerance">The tolerance for floating point comparisons.</param>
    /// <returns><c>true</c> if the amount was removed successfully; otherwise, <c>false</c> if there were insufficient items.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryRemoveExact(ref float current, float amount, float tolerance = MathConstants.Tolerance)
    {
        if (amount <= tolerance) return true;
        if (current < amount - tolerance) return false;

        current -= amount;
        if (current < tolerance) current = 0f;
        return true;
    }
}
