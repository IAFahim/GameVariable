namespace Variable.Bounded;

/// <summary>
///     Extension methods for bounded types providing logic operations.
///     These methods bridge from structs to primitive-only logic.
/// </summary>
public static class BoundedExtensions
{
    /// <summary>
    ///     Sets the current value, clamping it to bounds.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref this BoundedFloat b, float value)
    {
        BoundedLogic.Set(ref b.Current, b.Min, b.Max, value);
    }

    /// <summary>
    ///     Normalizes (clamps) the current value to bounds.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Normalize(ref this BoundedFloat b)
    {
        BoundedLogic.Normalize(ref b.Current, b.Min, b.Max);
    }

    /// <summary>
    ///     Determines whether the current value has reached its maximum bound.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(this BoundedFloat bounded, float tolerance = MathConstants.Tolerance)
    {
        return BoundedLogic.IsFull(bounded.Current, bounded.Max, tolerance);
    }

    /// <summary>
    ///     Determines whether the current value has reached its minimum bound.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(this BoundedFloat bounded, float tolerance = MathConstants.Tolerance)
    {
        return BoundedLogic.IsEmpty(bounded.Current, bounded.Min, tolerance);
    }

    /// <summary>
    ///     Gets the normalized ratio of the current value within the bounded range.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetRatio(this BoundedFloat bounded)
    {
        return BoundedLogic.GetRatio(bounded.Current, bounded.Min, bounded.Max);
    }

    /// <summary>
    ///     Gets the range between min and max.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetRange(this BoundedFloat bounded)
    {
        return BoundedLogic.GetRange(bounded.Min, bounded.Max);
    }

    /// <summary>
    ///     Gets the amount remaining until max.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetRemaining(this BoundedFloat bounded)
    {
        return BoundedLogic.GetRemaining(bounded.Current, bounded.Max);
    }

    /// <summary>
    ///     Sets the current value, clamping it to bounds.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref this BoundedInt b, int value)
    {
        BoundedLogic.Set(ref b.Current, b.Min, b.Max, value);
    }

    /// <summary>
    ///     Normalizes (clamps) the current value to bounds.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Normalize(ref this BoundedInt b)
    {
        BoundedLogic.Normalize(ref b.Current, b.Min, b.Max);
    }

    /// <summary>
    ///     Determines whether the current value has reached its maximum bound.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(this BoundedInt bounded)
    {
        return BoundedLogic.IsFull(bounded.Current, bounded.Max);
    }

    /// <summary>
    ///     Determines whether the current value has reached its minimum bound.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(this BoundedInt bounded)
    {
        return BoundedLogic.IsEmpty(bounded.Current, bounded.Min);
    }

    /// <summary>
    ///     Gets the normalized ratio of the current value within the bounded range.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetRatio(this BoundedInt bounded)
    {
        return BoundedLogic.GetRatio(bounded.Current, bounded.Min, bounded.Max);
    }

    /// <summary>
    ///     Gets the range between min and max.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetRange(this BoundedInt bounded)
    {
        return BoundedLogic.GetRange(bounded.Min, bounded.Max);
    }

    /// <summary>
    ///     Gets the amount remaining until max.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetRemaining(this BoundedInt bounded)
    {
        return BoundedLogic.GetRemaining(bounded.Current, bounded.Max);
    }

    /// <summary>
    ///     Sets the current value, clamping it to max.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref this BoundedByte b, byte value)
    {
        BoundedLogic.Set(ref b.Current, b.Max, value);
    }

    /// <summary>
    ///     Normalizes (clamps) the current value to max.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Normalize(ref this BoundedByte b)
    {
        BoundedLogic.Normalize(ref b.Current, b.Max);
    }

    /// <summary>
    ///     Determines whether the current value has reached its maximum bound.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(this BoundedByte bounded)
    {
        return BoundedLogic.IsFull(bounded.Current, bounded.Max);
    }

    /// <summary>
    ///     Determines whether the current value has reached its minimum bound (zero).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(this BoundedByte bounded)
    {
        return BoundedLogic.IsEmpty(bounded.Current);
    }

    /// <summary>
    ///     Gets the normalized ratio of the current value within the bounded range.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetRatio(this BoundedByte bounded)
    {
        return BoundedLogic.GetRatio(bounded.Current, bounded.Max);
    }

    /// <summary>
    ///     Gets the range (which is just max since min is always 0).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte GetRange(this BoundedByte bounded)
    {
        return BoundedLogic.GetRange(bounded.Max);
    }

    /// <summary>
    ///     Gets the amount remaining until max.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte GetRemaining(this BoundedByte bounded)
    {
        return BoundedLogic.GetRemaining(bounded.Current, bounded.Max);
    }

    /// <summary>
    ///     Applies regeneration to a BoundedFloat.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Tick(ref this BoundedFloat bounded, float rate, float deltaTime)
    {
        var current = bounded.Current;
        bounded.Current = current + rate * deltaTime;
        bounded.Normalize();
    }

    /// <summary>
    ///     Attempts to consume (subtract) an amount from the bounded float.
    ///     Returns false if there's not enough available.
    /// </summary>
    /// <param name="bounded">The bounded float to consume from.</param>
    /// <param name="amount">The amount to consume.</param>
    /// <returns>True if successful; false if insufficient.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryConsume(ref this BoundedFloat bounded, float amount)
    {
        if (bounded.Current < amount) return false;
        bounded.Current -= amount;
        return true;
    }

    /// <summary>
    ///     Attempts to consume (subtract) an amount from the bounded int.
    ///     Returns false if there's not enough available.
    /// </summary>
    /// <param name="bounded">The bounded int to consume from.</param>
    /// <param name="amount">The amount to consume.</param>
    /// <returns>True if successful; false if insufficient.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryConsume(ref this BoundedInt bounded, int amount)
    {
        if (bounded.Current < amount) return false;
        bounded.Current -= amount;
        return true;
    }

    /// <summary>
    ///     Attempts to consume (subtract) an amount from the bounded byte.
    ///     Returns false if there's not enough available.
    /// </summary>
    /// <param name="bounded">The bounded byte to consume from.</param>
    /// <param name="amount">The amount to consume.</param>
    /// <returns>True if successful; false if insufficient.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryConsume(ref this BoundedByte bounded, byte amount)
    {
        if (bounded.Current < amount) return false;
        bounded.Current -= amount;
        return true;
    }
}