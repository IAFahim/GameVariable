namespace GameVariable.Synergy;

/// <summary>
///     Extension methods for <see cref="SynergyState" />.
/// </summary>
public static class SynergyExtensions
{
    /// <summary>
    ///     Updates the synergy state (Regen ticks).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Tick(ref this SynergyState state, float deltaTime)
    {
        SynergyLogic.Update(ref state, deltaTime);
    }

    /// <summary>
    ///     Adds experience and handles level-ups and stat synchronization.
    /// </summary>
    /// <param name="state">The synergy state.</param>
    /// <param name="amount">XP amount to add.</param>
    /// <param name="maxHpId">Stat ID for Max Health.</param>
    /// <param name="maxMpId">Stat ID for Max Mana.</param>
    /// <param name="hpRegenId">Stat ID for Health Regen.</param>
    /// <param name="mpRegenId">Stat ID for Mana Regen.</param>
    /// <param name="xpCurve">Optional custom XP curve (default: Level * 1000).</param>
    public static void AddXp(
        ref this SynergyState state,
        int amount,
        int maxHpId,
        int maxMpId,
        int hpRegenId,
        int mpRegenId,
        Func<int, int>? xpCurve = null)
    {
        xpCurve ??= DefaultCurve;

        SynergyLogic.AddExperience(
            ref state,
            amount,
            xpCurve,
            maxHpId,
            maxMpId,
            hpRegenId,
            mpRegenId
        );
    }

    /// <summary>
    ///     Synchronizes Regen/Bounded components with the current RPG stats.
    ///     Call this after modifying stats manually (e.g. equipping items).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SyncStats(
        ref this SynergyState state,
        int maxHpId,
        int maxMpId,
        int hpRegenId,
        int mpRegenId)
    {
        SynergyLogic.RecalculateStats(ref state, maxHpId, maxMpId, hpRegenId, mpRegenId);
    }

    /// <summary>
    ///     Applies damage to the Health component.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ApplyDamage(ref this SynergyState state, float amount)
    {
        SynergyLogic.TakeDamage(ref state, amount);
    }

    private static int DefaultCurve(int level)
    {
        return level * 1000;
    }
}
