namespace GameVariable.Synergy;

public static class SynergyExtensions
{
    /// <summary>
    /// Initializes the SynergyState with default values and allocates unmanaged resources.
    /// </summary>
    public static void Initialize(ref this SynergyState state, int startLevel = 1)
    {
        // Init Stats (Unmanaged)
        state.Stats = new RpgStatSheet(StatIds.Count);

        // Init Experience (Max 1000 for level 1)
        state.Level = new ExperienceInt(1000, 0, startLevel);

        // Init Health/Mana (RegenFloat wraps BoundedFloat)
        // Default: 100 max, full, 1.0 regen (will be overwritten by Update)
        state.Health = new RegenFloat(100f, 100f, 1f);
        state.Mana = new RegenFloat(50f, 50f, 0.5f);

        // Initial Update to sync stats with the starting level
        state.Update(0f);

        // Refill Health/Mana to new max after stats are calculated
        state.Health.Value.Current = state.Health.Value.Max;
        state.Mana.Value.Current = state.Mana.Value.Max;
    }

    /// <summary>
    /// The main "Conductor" loop.
    /// Updates Stats based on Level, Max Values based on Stats, and Regen based on Stats.
    /// Then ticks the regen simulation.
    /// </summary>
    public static void Update(ref this SynergyState state, float deltaTime)
    {
        // 1. Stats Calculation based on Level
        // In a real game, this might be event-driven or cached, but for this demo/conductor,
        // we recalculate to show the flow "Level -> Stats".
        int currentLevel = state.Level.Level;

        // Vitality: 10 + (Level * 2)
        SynergyLogic.CalculateBaseStat(in currentLevel, 2f, 10f, out float baseVit);
        state.Stats.SetBase(StatIds.Vitality, baseVit);

        // Intellect: 5 + (Level * 1)
        SynergyLogic.CalculateBaseStat(in currentLevel, 1f, 5f, out float baseInt);
        state.Stats.SetBase(StatIds.Intellect, baseInt);

        // Spirit: 2 + (Level * 0.5)
        SynergyLogic.CalculateBaseStat(in currentLevel, 0.5f, 2f, out float baseSpi);
        state.Stats.SetBase(StatIds.Spirit, baseSpi);

        // 2. Resolve Derived Stats
        // We get the resolved value (Base + Modifiers)
        float vitVal = state.Stats.Get(StatIds.Vitality);
        float intVal = state.Stats.Get(StatIds.Intellect);
        float spiVal = state.Stats.Get(StatIds.Spirit);

        // 3. Update Max Values (Health/Mana)
        // Health (Derived from Vitality)
        SynergyLogic.CalculateMaxHealth(in vitVal, out float newMaxHealth);
        state.Health.Value.Max = newMaxHealth;
        state.Health.Value.Normalize(); // Clamp current if max reduced below current

        // Mana (Derived from Intellect)
        SynergyLogic.CalculateMaxMana(in intVal, out float newMaxMana);
        state.Mana.Value.Max = newMaxMana;
        state.Mana.Value.Normalize();

        // 4. Update Regen Rates
        // Regen (Derived from Spirit)
        SynergyLogic.CalculateRegen(in spiVal, out float newRegen);
        state.Health.Rate = newRegen;
        state.Mana.Rate = newRegen;

        // 5. Tick Regen
        state.Health.Tick(deltaTime);
        state.Mana.Tick(deltaTime);
    }
}
