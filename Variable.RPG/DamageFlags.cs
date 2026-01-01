namespace Variable.RPG;

/// <summary>
///     Flags for DamagePacket to mark special damage types.
///     Use bitwise operations for combinations.
/// </summary>
public static class DamageFlags
{
    public const int None = 0;
    public const int Critical = 1 << 0;      // 1
    public const int TrueDamage = 1 << 1;    // 2 - Ignores armor/resist
    public const int Lifesteal = 1 << 2;     // 4 - Heals attacker
    public const int CanDodge = 1 << 3;      // 8 - Can be dodged
    public const int CanBlock = 1 << 4;      // 16 - Can be blocked
    public const int CanReflect = 1 << 5;    // 32 - Can be reflected
}
