namespace Variable.Bounded;

/// <summary>
///     Extension methods for bounded types providing logic operations.
///     These methods bridge from structs to primitive-only logic.
/// </summary>
public static class BoundedExtensions
{
    // BoundedFloat Extensions
    
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
        return bounded.Current >= bounded.Max - tolerance;
    }

    /// <summary>
    ///     Determines whether the current value has reached its minimum bound.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(this BoundedFloat bounded, float tolerance = MathConstants.Tolerance)
    {
        return bounded.Current <= bounded.Min + tolerance;
    }

    /// <summary>
    ///     Gets the normalized ratio of the current value within the bounded range.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetRatio(this BoundedFloat bounded)
    {
        var range = bounded.Max - bounded.Min;
        return Math.Abs(range) < MathConstants.Tolerance ? 0.0 : (bounded.Current - bounded.Min) / range;
    }

    // BoundedInt Extensions
    
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
        return bounded.Current == bounded.Max;
    }

    /// <summary>
    ///     Determines whether the current value has reached its minimum bound.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(this BoundedInt bounded)
    {
        return bounded.Current == bounded.Min;
    }

    /// <summary>
    ///     Gets the normalized ratio of the current value within the bounded range.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetRatio(this BoundedInt bounded)
    {
        var range = bounded.Max - bounded.Min;
        return range == 0 ? 0.0 : (double)(bounded.Current - bounded.Min) / range;
    }

    // BoundedByte Extensions
    
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
        return bounded.Current == bounded.Max;
    }

    /// <summary>
    ///     Determines whether the current value has reached its minimum bound (zero).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(this BoundedByte bounded)
    {
        return bounded.Current == 0;
    }

    /// <summary>
    ///     Gets the normalized ratio of the current value within the bounded range.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetRatio(this BoundedByte bounded)
    {
        return bounded.Max == 0 ? 0.0 : (double)bounded.Current / bounded.Max;
    }
}
