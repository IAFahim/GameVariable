namespace Variable.Random;

/// <summary>
///     A pure data struct representing the state of a random number generator.
///     Uses a 32-bit state suitable for Xorshift32.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct RngState : IEquatable<RngState>
{
    /// <summary>
    ///     The internal state of the generator.
    ///     Must be non-zero for Xorshift32.
    /// </summary>
    public uint State;

    /// <summary>
    ///     Initializes a new instance of the <see cref="RngState"/> struct.
    /// </summary>
    /// <param name="seed">The initial seed. If 0 is provided, it will be forced to 1.</param>
    public RngState(uint seed)
    {
        // Xorshift must not have a zero state.
        State = seed == 0 ? 1 : seed;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is RngState other && Equals(other);
    }

    /// <inheritdoc />
    public bool Equals(RngState other)
    {
        return State == other.State;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return (int)State;
    }

    /// <summary>
    ///     Determines whether two RngState instances are equal.
    /// </summary>
    public static bool operator ==(RngState left, RngState right)
    {
        return left.Equals(right);
    }

    /// <summary>
    ///     Determines whether two RngState instances are not equal.
    /// </summary>
    public static bool operator !=(RngState left, RngState right)
    {
        return !left.Equals(right);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return State.ToString();
    }
}
