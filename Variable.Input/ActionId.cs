namespace Variable.Input;

/// <summary>
///     Type-safe wrapper for action IDs to prevent accidental misuse of raw integers.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public readonly struct ActionId : IEquatable<ActionId>
{
    /// <summary>
    ///     The underlying integer value.
    /// </summary>
    public readonly int Value;

    /// <summary>
    ///     Creates a new ActionId.
    /// </summary>
    /// <param name="value">The integer value.</param>
    public ActionId(int value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public bool Equals(ActionId other)
    {
        return Value == other.Value;
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is ActionId other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Value;
    }

    /// <summary>
    ///     Implicit conversion to int for backward compatibility.
    /// </summary>
    public static implicit operator int(ActionId id)
    {
        return id.Value;
    }

    /// <summary>
    ///     Implicit conversion from int for ease of use.
    /// </summary>
    public static implicit operator ActionId(int value)
    {
        return new ActionId(value);
    }

    /// <summary>
    ///     Equality operator.
    /// </summary>
    public static bool operator ==(ActionId left, ActionId right)
    {
        return left.Equals(right);
    }

    /// <summary>
    ///     Inequality operator.
    /// </summary>
    public static bool operator !=(ActionId left, ActionId right)
    {
        return !left.Equals(right);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"ActionId({Value})";
    }
}