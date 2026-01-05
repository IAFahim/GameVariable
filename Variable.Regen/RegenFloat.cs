namespace Variable.Regen;

/// <summary>
///     A bounded float value that automatically regenerates or decays over time.
///     Ideal for mana, stamina, shields, and other resources that recover passively.
/// </summary>
/// <remarks>
///     <para>Call <see cref="RegenExtensions.Tick(ref RegenFloat, float)" /> each frame with deltaTime to apply regeneration.</para>
///     <para>Positive <see cref="Rate" /> regenerates; negative <see cref="Rate" /> causes decay.</para>
///     <para>This struct is blittable and can be used in Unity ECS and Burst jobs.</para>
/// </remarks>
/// <example>
///     <code>
/// // Mana: Max 100, Current 50, regenerates 5/second
/// var mana = new RegenFloat(100f, 50f, 5f);
/// mana.Tick(Time.deltaTime);
/// 
/// // Poison decay: -10 per second
/// var poison = new RegenFloat(100f, 100f, -10f);
/// </code>
/// </example>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct RegenFloat : IBoundedInfo, IEquatable<RegenFloat>
{
    /// <summary>The underlying bounded value being regenerated.</summary>
    public BoundedFloat Value;

    /// <summary>The rate of regeneration/decay in units per second.</summary>
    public float Rate;

    /// <inheritdoc />
    float IBoundedInfo.Min
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => 0;
    }

    /// <inheritdoc />
    float IBoundedInfo.Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value.Current;
    }

    /// <inheritdoc />
    float IBoundedInfo.Max
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value.Max;
    }


    /// <summary>
    ///     Creates a new regenerating float with the specified bounds and rate.
    /// </summary>
    /// <param name="max">The maximum value.</param>
    /// <param name="current">The initial current value.</param>
    /// <param name="rate">The regeneration rate in units per second.</param>
    public RegenFloat(float max, float current, float rate)
    {
        Value = new BoundedFloat(max, current);
        Rate = rate;
    }

    /// <summary>
    ///     Creates a new regenerating float from an existing bounded float.
    /// </summary>
    /// <param name="value">The bounded float to wrap.</param>
    /// <param name="rate">The regeneration rate in units per second.</param>
    public RegenFloat(BoundedFloat value, float rate)
    {
        Value = value;
        Rate = rate;
    }

    /// <summary>Implicitly converts the regenerating float to its current value.</summary>
    public static implicit operator float(RegenFloat regen)
    {
        return regen.Value.Current;
    }

    /// <summary>Implicitly converts the regenerating float to its underlying bounded float.</summary>
    public static implicit operator BoundedFloat(RegenFloat regen)
    {
        return regen.Value;
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is RegenFloat other && Equals(other);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(RegenFloat other)
    {
        return Value.Equals(other.Value) && Rate.Equals(other.Rate);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return HashCode.Combine(Value, Rate);
    }

    /// <summary>Determines whether two regen floats are equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(RegenFloat left, RegenFloat right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two regen floats are not equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(RegenFloat left, RegenFloat right)
    {
        return !left.Equals(right);
    }
}