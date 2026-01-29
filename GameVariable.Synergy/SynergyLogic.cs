namespace GameVariable.Synergy;

/// <summary>
/// Pure stateless logic for manipulating SynergyCharacter.
/// </summary>
public static class SynergyLogic
{
    /// <summary>
    /// Updates the character's time-based states (Mana regen, Cooldowns).
    /// </summary>
    /// <param name="character">The character to update.</param>
    /// <param name="deltaTime">Time elapsed since last frame.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Update(ref SynergyCharacter character, float deltaTime)
    {
        // Update Mana (Regen)
        character.Mana.Tick(deltaTime);

        // Update Cooldown (Timer)
        character.AttackCooldown.Tick(deltaTime);
    }

    /// <summary>
    /// Attempts to cast an ability if cooldown is ready and mana is sufficient.
    /// </summary>
    /// <param name="character">The character attempting to cast.</param>
    /// <param name="manaCost">The mana cost of the ability.</param>
    /// <returns>True if cast was successful; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryCastAbility(ref SynergyCharacter character, float manaCost)
    {
        // Check 1: Cooldown
        if (!character.AttackCooldown.IsReady())
            return false;

        // Check 2: Mana
        if (character.Mana.Value < manaCost)
            return false;

        // Execute: Consume Mana
        character.Mana.Value -= manaCost;

        // Execute: Reset Cooldown
        character.AttackCooldown.Reset();

        return true;
    }

    /// <summary>
    /// Awards experience and checks for level up.
    /// </summary>
    /// <param name="character">The character gaining experience.</param>
    /// <param name="amount">The amount of XP to gain.</param>
    /// <param name="leveledUp">Outputs true if the character leveled up.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AwardExperience(ref SynergyCharacter character, int amount, out bool leveledUp)
    {
        leveledUp = false;
        character.Experience += amount;

        // Simple level up logic: If full, increment level and double requirement
        if (character.Experience.IsFull())
        {
            leveledUp = true;
            int nextMax = character.Experience.Max * 2;
            int currentLevel = character.Experience.Level;

            // Handle overflow simply for this example (discard it or wrap it)
            // For a perfect "Synergy" we might want to carry it over, but let's keep it clean:
            // Just reset to 0/NextMax at Level+1.
            // Or better: Preserve the overflow.
            int overflow = character.Experience.Current - character.Experience.Max;
            if (overflow < 0) overflow = 0; // Should not happen if IsFull is true

            character.Experience = new ExperienceInt(nextMax, overflow, currentLevel + 1);

            // Synergy Bonus: Full Heal/Mana on Level Up!
            character.Health.Current = character.Health.Max;
            character.Mana.Value.Current = character.Mana.Value.Max;
        }
    }
}
