using System.Runtime.CompilerServices;

namespace GameVariable.Synergy;

/// <summary>
///     Pure logic for the Synergy system.
///     Operates on primitives only.
/// </summary>
public static class SynergyLogic
{
    /// <summary>
    ///     Determines if a skill can be cast based on mana and cooldown status.
    /// </summary>
    /// <param name="currentMana">Current mana value.</param>
    /// <param name="manaCost">Mana cost of the skill.</param>
    /// <param name="isCooldownReady">Whether the cooldown is ready.</param>
    /// <param name="canCast">Output result.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CanCast(in float currentMana, in float manaCost, in bool isCooldownReady, out bool canCast)
    {
        canCast = currentMana >= manaCost && isCooldownReady;
    }

    /// <summary>
    ///     Calculates the maximum gold capacity based on stats.
    /// </summary>
    /// <param name="baseCapacity">The base capacity.</param>
    /// <param name="strength">The strength stat value.</param>
    /// <param name="capacity">Output calculated capacity.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CalculateGoldCapacity(in float baseCapacity, in float strength, out float capacity)
    {
        // Example rule: Each point of strength adds 10 capacity
        capacity = baseCapacity + (strength * 10f);
    }

    /// <summary>
    ///     Checks if a level up occurred and determines if stats should be restored.
    /// </summary>
    /// <param name="oldLevel">The previous level.</param>
    /// <param name="newLevel">The current level.</param>
    /// <param name="shouldRestore">Output true if stats should be restored.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CheckLevelUpRestore(in int oldLevel, in int newLevel, out bool shouldRestore)
    {
        shouldRestore = newLevel > oldLevel;
    }
}
