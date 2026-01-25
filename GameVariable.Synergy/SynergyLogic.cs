namespace GameVariable.Synergy;

/// <summary>
///     Pure logic layer for the Synergy integration.
///     <para>Orchestrates updates between Experience, Stats, and Regen components.</para>
/// </summary>
public static class SynergyLogic
{
    /// <summary>
    ///     Updates the regeneration state based on time delta.
    /// </summary>
    /// <param name="state">The synergy state.</param>
    /// <param name="deltaTime">Time elapsed in seconds.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Update(ref SynergyState state, float deltaTime)
    {
        // Decompose Health
        RegenLogic.Tick(
            ref state.Health.Value.Current,
            state.Health.Value.Min,
            state.Health.Value.Max,
            state.Health.Rate,
            deltaTime
        );

        // Decompose Mana
        RegenLogic.Tick(
            ref state.Mana.Value.Current,
            state.Mana.Value.Min,
            state.Mana.Value.Max,
            state.Mana.Rate,
            deltaTime
        );
    }

    /// <summary>
    ///     Adds experience, processes level ups, and synchronizes stats.
    /// </summary>
    /// <param name="state">The synergy state.</param>
    /// <param name="amount">Amount of XP to add.</param>
    /// <param name="xpCurve">Function defining XP required for next level.</param>
    /// <param name="maxHpId">Stat ID for Max Health.</param>
    /// <param name="maxMpId">Stat ID for Max Mana.</param>
    /// <param name="hpRegenId">Stat ID for Health Regen.</param>
    /// <param name="mpRegenId">Stat ID for Mana Regen.</param>
    public static void AddExperience(
        ref SynergyState state,
        int amount,
        Func<int, int> xpCurve,
        int maxHpId,
        int maxMpId,
        int hpRegenId,
        int mpRegenId)
    {
        // 1. Add XP
        // Using operator+ which creates a new struct, so we reassign
        state.Experience += amount;

        // 2. Level Up Loop
        // Check if we have enough XP to level up
        while (state.Experience.Current >= state.Experience.Max)
        {
            // Calculate requirements for NEXT level
            int nextLevel = state.Experience.Level + 1;
            int nextMax = xpCurve(nextLevel);

            // Apply level up using ExperienceLogic
            // Modifies fields directly via ref
            ExperienceLogic.TryApplyLevelUp(
                ref state.Experience.Current,
                ref state.Experience.Max,
                ref state.Experience.Level,
                nextMax
            );

            // Apply Level Up Bonuses (Simple fixed growth for demo)
            // +10 Max HP, +5 Max Mana
            // We modify the Base value of the RPG stats
            float currentHpBase = state.Stats.GetRef(maxHpId).Base;
            float currentMpBase = state.Stats.GetRef(maxMpId).Base;

            state.Stats.SetBase(maxHpId, currentHpBase + 10f);
            state.Stats.SetBase(maxMpId, currentMpBase + 5f);
        }

        // 3. Sync Regen components with updated Stats
        RecalculateStats(ref state, maxHpId, maxMpId, hpRegenId, mpRegenId);
    }

    /// <summary>
    ///     Synchronizes RegenFloat components (Bounds/Rates) with values from RpgStatSheet.
    /// </summary>
    /// <param name="state">The synergy state.</param>
    /// <param name="maxHpId">Stat ID for Max Health.</param>
    /// <param name="maxMpId">Stat ID for Max Mana.</param>
    /// <param name="hpRegenId">Stat ID for Health Regen.</param>
    /// <param name="mpRegenId">Stat ID for Mana Regen.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void RecalculateStats(
        ref SynergyState state,
        int maxHpId,
        int maxMpId,
        int hpRegenId,
        int mpRegenId)
    {
        // 1. Read calculated stats from the RPG sheet (includes modifiers)
        float maxHp = state.Stats.Get(maxHpId);
        float maxMp = state.Stats.Get(maxMpId);
        float hpRegen = state.Stats.Get(hpRegenId);
        float mpRegen = state.Stats.Get(mpRegenId);

        // 2. Update the Regen/Bounded components
        // We preserve current value but clamp to new max
        // RegenFloat constructor takes (max, current, rate) or (BoundedFloat, rate)

        state.Health = new RegenFloat(
            new BoundedFloat(maxHp, state.Health.Value.Current),
            hpRegen
        );

        state.Mana = new RegenFloat(
            new BoundedFloat(maxMp, state.Mana.Value.Current),
            mpRegen
        );
    }

    /// <summary>
    ///     Applies damage to Health.
    /// </summary>
    /// <param name="state">The synergy state.</param>
    /// <param name="amount">Amount of damage.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void TakeDamage(ref SynergyState state, float amount)
    {
        // Apply damage to Health
        // Using operator- on BoundedFloat guarantees clamping
        state.Health.Value = state.Health.Value - amount;
    }
}
