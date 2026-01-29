namespace GameVariable.Synergy;

/// <summary>
/// Extension methods for SynergyCharacter to enable fluent usage.
/// </summary>
public static class SynergyExtensions
{
    /// <summary>
    /// Updates the character's state.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Update(ref this SynergyCharacter character, float deltaTime)
    {
        SynergyLogic.Update(ref character, deltaTime);
    }

    /// <summary>
    /// Attempts to cast an ability.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryCastAbility(ref this SynergyCharacter character, float manaCost)
    {
        return SynergyLogic.TryCastAbility(ref character, manaCost);
    }

    /// <summary>
    /// Awards experience and checks for level up.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AwardExperience(ref this SynergyCharacter character, int amount, out bool leveledUp)
    {
        SynergyLogic.AwardExperience(ref character, amount, out leveledUp);
    }
}
