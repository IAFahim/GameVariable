namespace Variable.Input;

/// <summary>
///     Type-safe wrapper for input trigger IDs to prevent accidental misuse of raw integers.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public readonly struct InputTriggerId : IEquatable<InputTriggerId>
{
    /// <summary>
    ///     The underlying integer value.
    /// </summary>
    public readonly int Value;

    /// <summary>
    ///     Creates a new InputTriggerId.
    /// </summary>
    /// <param name="value">The integer value.</param>
    public InputTriggerId(int value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public bool Equals(InputTriggerId other)
    {
        return Value == other.Value;
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is InputTriggerId other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Value;
    }

    /// <summary>
    ///     Implicit conversion to int for backward compatibility.
    /// </summary>
    public static implicit operator int(InputTriggerId id)
    {
        return id.Value;
    }

    /// <summary>
    ///     Implicit conversion from int for ease of use.
    /// </summary>
    public static implicit operator InputTriggerId(int value)
    {
        return new InputTriggerId(value);
    }

    /// <summary>
    ///     Equality operator.
    /// </summary>
    public static bool operator ==(InputTriggerId left, InputTriggerId right)
    {
        return left.Equals(right);
    }

    /// <summary>
    ///     Inequality operator.
    /// </summary>
    public static bool operator !=(InputTriggerId left, InputTriggerId right)
    {
        return !left.Equals(right);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"InputTriggerId({Value})";
    }
}