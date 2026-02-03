namespace GameVariable.Synergy;

/// <summary>
///     A composite character structure demonstrating the synergy between multiple Variable packages.
///     Integrates Health (Bounded), Mana (Regen), Cooldown (Timer), and XP (Experience).
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{ToString()}")]
public struct SynergyCharacter : IEquatable<SynergyCharacter>
{
    /// <summary>The character's health points.</summary>
    public BoundedFloat Health;

    /// <summary>The character's mana points, which regenerate over time.</summary>
    public RegenFloat Mana;

    /// <summary>The cooldown for the character's primary ability.</summary>
    public Cooldown AbilityCooldown;

    /// <summary>The character's experience and level progression.</summary>
    public ExperienceInt XP;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SynergyCharacter" /> struct.
    /// </summary>
    /// <param name="healthMax">Maximum health.</param>
    /// <param name="manaMax">Maximum mana.</param>
    /// <param name="manaRegenRate">Mana regeneration rate per second.</param>
    /// <param name="cooldownDuration">Duration of the ability cooldown.</param>
    /// <param name="xpMax">XP required for the first level up.</param>
    public SynergyCharacter(float healthMax, float manaMax, float manaRegenRate, float cooldownDuration, int xpMax)
    {
        Health = new BoundedFloat(healthMax);
        Mana = new RegenFloat(manaMax, manaMax, manaRegenRate);
        AbilityCooldown = new Cooldown(cooldownDuration);
        XP = new ExperienceInt(xpMax);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"HP: {Health} | MP: {Mana} | CD: {AbilityCooldown} | XP: {XP}";
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is SynergyCharacter other && Equals(other);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(SynergyCharacter other)
    {
        return Health.Equals(other.Health) &&
               Mana.Equals(other.Mana) &&
               AbilityCooldown.Equals(other.AbilityCooldown) &&
               XP.Equals(other.XP);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return HashCode.Combine(Health, Mana, AbilityCooldown, XP);
    }

    /// <summary>Determines whether two characters are equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(SynergyCharacter left, SynergyCharacter right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two characters are not equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(SynergyCharacter left, SynergyCharacter right)
    {
        return !left.Equals(right);
    }
}
