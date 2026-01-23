using Variable.Bounded;
using Variable.Experience;
using Variable.Regen;
using Variable.RPG;

namespace GameVariable.Synergy;

/// <summary>
///     Core logic for the Synergy module, handling the interaction between different game systems.
/// </summary>
public static class SynergyLogic
{
    /// <summary>
    ///     Updates the entire game state, processing experience, stats, and regeneration.
    /// </summary>
    public static void Update(ref SynergyState state, float deltaTime)
    {
        // 1. Check Level Up
        // Formula: NewMax = 100 * (Level + 1)
        int nextMax = 100 * (state.Experience.Level + 1);

        bool leveledUp = ExperienceLogic.TryApplyLevelUp(
            ref state.Experience.Current,
            ref state.Experience.Max,
            ref state.Experience.Level,
            nextMax
        );

        // 2. Sync Stats
        // We do this if leveled up OR if it's the first run (e.g. stats are 0).
        // For simplicity in this synergy demo, we enforce stats every frame based on level.
        // In a real optimized game, this would be event-driven.
        if (state.Stats.Count >= 2)
        {
            // Vitality = 10 + Level * 5
            float targetVitality = 10f + state.Experience.Level * 5f;
            state.Stats.SetBase(0, targetVitality);

            // Wisdom = 5 + Level * 2
            float targetWisdom = 5f + state.Experience.Level * 2f;
            state.Stats.SetBase(1, targetWisdom);
        }

        // 3. Sync Dependent Values (RPG -> Bounded/Regen)
        if (state.Stats.Count >= 2)
        {
            float vitality = state.Stats.Get(0);
            float wisdom = state.Stats.Get(1);

            // Health Max = Vitality * 10
            float newMaxHealth = vitality * 10f;
            state.Health.Max = newMaxHealth;
            // Clamp current health if it exceeds max
            if (state.Health.Current > state.Health.Max) state.Health.Current = state.Health.Max;

            // Mana Max = Wisdom * 10
            float newMaxMana = wisdom * 10f;
            state.Mana.Value.Max = newMaxMana;
            if (state.Mana.Value.Current > state.Mana.Value.Max) state.Mana.Value.Current = state.Mana.Value.Max;

            // Mana Regen Rate = Wisdom * 0.1
            state.Mana.Rate = wisdom * 0.1f;
        }

        // 4. Process Regeneration
        RegenLogic.Tick(
            ref state.Mana.Value.Current,
            state.Mana.Value.Min,
            state.Mana.Value.Max,
            state.Mana.Rate,
            deltaTime
        );
    }
}
