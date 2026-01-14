namespace Variable.Inventory;

/// <summary>
///     Query operations for inventory state inspection.
///     All methods are pure functions that do not mutate state.
/// </summary>
public static partial class InventoryLogic
{
    /// <summary>
    ///     Determines whether the inventory is full based on the current quantity and maximum capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(in float current, in float max, in float tolerance = MathConstants.DefaultTolerance)
    {
        return current >= max - tolerance;
    }

    /// <summary>
    ///     Determines whether the inventory is full based on the current quantity and maximum capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(in int current, in int max)
    {
        return current >= max;
    }

    /// <summary>
    ///     Determines whether the inventory is full based on the current quantity and maximum capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(in long current, in long max)
    {
        return current >= max;
    }

    /// <summary>
    ///     Determines whether the inventory is full based on the current quantity and maximum capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(in byte current, in byte max)
    {
        return current >= max;
    }

    /// <summary>
    ///     Determines whether the inventory is empty.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(in float current, in float tolerance = MathConstants.DefaultTolerance)
    {
        return current <= tolerance;
    }

    /// <summary>
    ///     Determines whether the inventory is empty.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(in int current)
    {
        return current <= 0;
    }

    /// <summary>
    ///     Determines whether the inventory is empty.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(in long current)
    {
        return current <= 0;
    }

    /// <summary>
    ///     Determines whether the inventory is empty.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(in byte current)
    {
        return current <= 0;
    }

    /// <summary>
    ///     Checks if the inventory has enough items to satisfy a requirement.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasEnough(in float current, in float required, in float tolerance = MathConstants.DefaultTolerance)
    {
        return current >= required - tolerance;
    }

    /// <summary>
    ///     Checks if the inventory has enough items to satisfy a requirement.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasEnough(in int current, in int required)
    {
        return current >= required;
    }

    /// <summary>
    ///     Checks if the inventory has enough items to satisfy a requirement.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasEnough(in long current, in long required)
    {
        return current >= required;
    }

    /// <summary>
    ///     Checks if the inventory has enough items to satisfy a requirement.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasEnough(in byte current, in byte required)
    {
        return current >= required;
    }

    /// <summary>
    ///     Checks if the inventory can accept a specific amount of items without exceeding its capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CanAccept(in float current, in float max, in float amountToAdd,
        in float tolerance = MathConstants.DefaultTolerance)
    {
        return current + amountToAdd <= max + tolerance;
    }

    /// <summary>
    ///     Checks if the inventory can accept a specific amount of items without exceeding its capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CanAccept(in int current, in int max, in int amountToAdd)
    {
        return current + amountToAdd <= max;
    }

    /// <summary>
    ///     Checks if the inventory can accept a specific amount of items without exceeding its capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CanAccept(in long current, in long max, in long amountToAdd)
    {
        return current + amountToAdd <= max;
    }

    /// <summary>
    ///     Checks if the inventory can accept a specific amount of items without exceeding its capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CanAccept(in byte current, in byte max, in byte amountToAdd)
    {
        return current + amountToAdd <= max;
    }

    /// <summary>
    ///     Calculates the remaining space in the inventory.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetRemainingSpace(in float current, in float max, out float result)
    {
        result = current >= max ? 0f : max - current;
    }


    /// <summary>
    ///     Calculates the remaining space in the inventory.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetRemainingSpace(in int current, in int max, out int result)
    {
        result = current >= max ? 0 : max - current;
    }

    /// <summary>
    ///     Calculates the remaining space in the inventory.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetRemainingSpace(in long current, in long max, out long result)
    {
        result = current >= max ? 0 : max - current;
    }

    /// <summary>
    ///     Calculates the remaining space in the inventory.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetRemainingSpace(in byte current, in byte max, out byte result)
    {
        result = (byte)(current >= max ? 0 : max - current);
    }

    /// <summary>
    ///     Calculates the maximum amount of an item that can be accepted, considering both quantity and weight limits.
    /// </summary>
    /// <param name="currentQty">The current quantity of items.</param>
    /// <param name="maxQty">The maximum quantity capacity.</param>
    /// <param name="currentWeight">The current total weight.</param>
    /// <param name="maxWeight">The maximum weight capacity.</param>
    /// <param name="unitWeight">The weight of a single unit of the item.</param>
    /// <param name="result">The maximum amount that can be accepted.</param>
    /// <param name="tolerance">The tolerance for floating point comparisons.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetMaxAcceptable(
        in float currentQty,
        in float maxQty,
        in float currentWeight,
        in float maxWeight,
        in float unitWeight,
        out float result,
        in float tolerance = MathConstants.DefaultTolerance)
    {
        var spaceByQty = currentQty >= maxQty ? 0f : maxQty - currentQty;

        if (unitWeight <= tolerance)
        {
            result = spaceByQty;
            return;
        }

        var remainingWeight = currentWeight >= maxWeight ? 0f : maxWeight - currentWeight;
        var spaceByWeight = remainingWeight / unitWeight;

        result = spaceByQty < spaceByWeight ? spaceByQty : spaceByWeight;
    }
}