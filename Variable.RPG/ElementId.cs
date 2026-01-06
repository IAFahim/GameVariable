namespace Variable.RPG;

/// <summary>
///     Type-safe wrapper for element IDs to prevent accidental misuse of raw integers.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public readonly struct ElementId : IEquatable<ElementId>
{
    /// <summary>
    ///     The underlying integer value.
    /// </summary>
    public readonly int Value;

    /// <summary>
    ///     Creates a new ElementId.
    /// </summary>
    /// <param name="value">The integer value.</param>
    public ElementId(int value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public bool Equals(ElementId other)
    {
        return Value == other.Value;
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is ElementId other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Value;
    }

    /// <summary>
    ///     Implicit conversion to int for backward compatibility.
    /// </summary>
    public static implicit operator int(ElementId id)
    {
        return id.Value;
    }

    /// <summary>
    ///     Implicit conversion from int for ease of use.
    /// </summary>
    public static implicit operator ElementId(int value)
    {
        return new ElementId(value);
    }

    /// <summary>
    ///     Equality operator.
    /// </summary>
    public static bool operator ==(ElementId left, ElementId right)
    {
        return left.Equals(right);
    }

    /// <summary>
    ///     Inequality operator.
    /// </summary>
    public static bool operator !=(ElementId left, ElementId right)
    {
        return !left.Equals(right);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"ElementId({Value})";
    }
}