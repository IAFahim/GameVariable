namespace Variable.RPG;

/// <summary>
///     Type-safe wrapper for stat IDs to prevent accidental misuse of raw integers.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public readonly struct StatId : IEquatable<StatId>
{
    /// <summary>
    ///     The underlying integer value.
    /// </summary>
    public readonly int Value;

    /// <summary>
    ///     Creates a new StatId.
    /// </summary>
    /// <param name="value">The integer value.</param>
    public StatId(int value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public bool Equals(StatId other)
    {
        return Value == other.Value;
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is StatId other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Value;
    }

    /// <summary>
    ///     Implicit conversion to int for backward compatibility.
    /// </summary>
    public static implicit operator int(StatId id)
    {
        return id.Value;
    }

    /// <summary>
    ///     Implicit conversion from int for ease of use.
    /// </summary>
    public static implicit operator StatId(int value)
    {
        return new StatId(value);
    }

    /// <summary>
    ///     Equality operator.
    /// </summary>
    public static bool operator ==(StatId left, StatId right)
    {
        return left.Equals(right);
    }

    /// <summary>
    ///     Inequality operator.
    /// </summary>
    public static bool operator !=(StatId left, StatId right)
    {
        return !left.Equals(right);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"StatId({Value})";
    }
}