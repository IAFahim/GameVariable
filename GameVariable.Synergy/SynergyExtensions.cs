using System.Runtime.CompilerServices;
using Variable.Regen;
using Variable.Experience;
using Variable.Bounded;

namespace GameVariable.Synergy
{
    /// <summary>
    /// The "Conductor" layer that orchestrates the data flow between components.
    /// </summary>
    public static class SynergyExtensions
    {
        /// <summary>
        /// Initializes the state with Level 1 stats.
        /// </summary>
        public static void Initialize(ref this SynergyState state)
        {
             // 1. Calculate Base Stats for Level 1
             SynergyLogic.CalculateBaseStats(state.Experience.Level, out var vit, out var spi, out var str, out var agi);

             // 2. Set Stats
             state.Stats.SetBase(SynergyState.STAT_VITALITY, vit);
             state.Stats.SetBase(SynergyState.STAT_SPIRIT, spi);
             state.Stats.SetBase(SynergyState.STAT_STRENGTH, str);
             state.Stats.SetBase(SynergyState.STAT_AGILITY, agi);

             // 3. Update Derived Values
             var finalVit = state.Stats.Get(SynergyState.STAT_VITALITY);
             var finalSpi = state.Stats.Get(SynergyState.STAT_SPIRIT);

             SynergyLogic.CalculateMaxHealth(finalVit, out var maxHealth);
             SynergyLogic.CalculateMaxMana(finalSpi, out var maxMana);

             state.Health.Value.Max = maxHealth;
             state.Health.Value.Current = maxHealth; // Start full

             state.Mana.Value.Max = maxMana;
             state.Mana.Value.Current = maxMana; // Start full

             SynergyLogic.CalculateHealthRegen(finalVit, out state.Health.Rate);
             SynergyLogic.CalculateManaRegen(finalSpi, out state.Mana.Rate);
        }

        /// <summary>
        /// Updates the synergy state: Checks Level Up -> Updates Stats -> Updates Regen.
        /// </summary>
        /// <param name="state">The state to update.</param>
        /// <param name="deltaTime">Time elapsed.</param>
        public static void Update(ref this SynergyState state, float deltaTime)
        {
            // 1. Check Level Up
            SynergyLogic.CheckLevelUp(state.Experience.Current, state.Experience.Max, out var isReady);
            if (isReady)
            {
                // Process Level Up
                // Simple carry over logic
                state.Experience.Current -= state.Experience.Max;
                state.Experience.Level++;

                // Calculate next level requirements
                SynergyLogic.CalculateNextLevelXp(state.Experience.Level, out state.Experience.Max);

                // Update Stats based on new level
                SynergyLogic.CalculateBaseStats(state.Experience.Level, out var vit, out var spi, out var str, out var agi);
                state.Stats.SetBase(SynergyState.STAT_VITALITY, vit);
                state.Stats.SetBase(SynergyState.STAT_SPIRIT, spi);
                state.Stats.SetBase(SynergyState.STAT_STRENGTH, str);
                state.Stats.SetBase(SynergyState.STAT_AGILITY, agi);

                // Update Derived Values
                var finalVit = state.Stats.Get(SynergyState.STAT_VITALITY);
                var finalSpi = state.Stats.Get(SynergyState.STAT_SPIRIT);

                SynergyLogic.CalculateMaxHealth(finalVit, out var maxHealth);
                SynergyLogic.CalculateMaxMana(finalSpi, out var maxMana);

                // Update BoundedFloats Max
                state.Health.Value.Max = maxHealth;
                state.Mana.Value.Max = maxMana;

                // Update Regen Rates
                SynergyLogic.CalculateHealthRegen(finalVit, out state.Health.Rate);
                SynergyLogic.CalculateManaRegen(finalSpi, out state.Mana.Rate);

                // Full Heal on Level Up (Design Choice)
                state.Health.Value.Current = state.Health.Value.Max;
                state.Mana.Value.Current = state.Mana.Value.Max;
            }

            // 2. Regen Tick
            state.Health.Tick(deltaTime);
            state.Mana.Tick(deltaTime);
        }

        /// <summary>
        /// Adds experience to the state.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddExperience(ref this SynergyState state, int amount)
        {
             state.Experience.Current += amount;
        }
    }
}
