using Xunit;
using GameVariable.Synergy;
using Variable.Experience;
using Variable.Regen;
using Variable.Bounded;

namespace GameVariable.Synergy.Tests
{
    public class SynergyTests
    {
        [Fact]
        public void Conductor_Flow_IntegrationTest()
        {
            // 1. Initialize
            // MaxXP for Level 1 = 1000
            var state = new SynergyState(100, 50, 1000);
            try
            {
                state.Initialize();

                // Verify Level 1
                Assert.Equal(1, state.Experience.Level);
                Assert.Equal(1000, state.Experience.Max);

                // Verify Stats (Level 1: Vit=10, Spi=5)
                // Formulas:
                // Vit = 10 + (1-1)*2 = 10
                // Spi = 5 + (1-1)*1 = 5
                Assert.Equal(10f, state.Stats.Get(SynergyState.STAT_VITALITY));
                Assert.Equal(5f, state.Stats.Get(SynergyState.STAT_SPIRIT));

                // Verify Derived (MaxHealth = 100 + 10*10 = 200, MaxMana = 50 + 5*10 = 100)
                Assert.Equal(200f, state.Health.Value.Max);
                Assert.Equal(100f, state.Mana.Value.Max);

                // Verify Regen Rates (HealthRate = 1 + 10*0.5 = 6, ManaRate = 2 + 5*1 = 7)
                Assert.Equal(6f, state.Health.Rate);
                Assert.Equal(7f, state.Mana.Rate);

                // 2. Add Experience (Not enough to level up)
                state.AddExperience(500);
                state.Update(1.0f);

                Assert.Equal(1, state.Experience.Level);
                Assert.Equal(500, state.Experience.Current);

                // 3. Level Up
                state.AddExperience(500); // Now 1000/1000 -> Level Up!
                state.Update(0.0f); // Zero delta time to isolate level up logic

                // Verify Level 2
                Assert.Equal(2, state.Experience.Level);
                Assert.Equal(0, state.Experience.Current); // Should consume 1000
                Assert.Equal(2000, state.Experience.Max); // New max for Level 2

                // Verify Stats (Level 2: Vit=12, Spi=6)
                // Vit = 10 + (2-1)*2 = 12
                // Spi = 5 + (2-1)*1 = 6
                Assert.Equal(12f, state.Stats.Get(SynergyState.STAT_VITALITY));
                Assert.Equal(6f, state.Stats.Get(SynergyState.STAT_SPIRIT));

                // Verify Derived
                // MaxHealth = 100 + 12*10 = 220
                // MaxMana = 50 + 6*10 = 110
                Assert.Equal(220f, state.Health.Value.Max);
                Assert.Equal(110f, state.Mana.Value.Max);

                // Verify Regen Rates
                // HealthRate = 1 + 12*0.5 = 7
                // ManaRate = 2 + 6*1 = 8
                Assert.Equal(7f, state.Health.Rate);
                Assert.Equal(8f, state.Mana.Rate);

                // Verify Full Heal on Level Up
                Assert.Equal(220f, state.Health.Value.Current);
                Assert.Equal(110f, state.Mana.Value.Current);

                // 4. Regen Tick
                // Damage the player to test regen
                state.Health.Value.Current = 100f;
                state.Update(1.0f); // 1 second

                // Should regen 7 health
                Assert.Equal(107f, state.Health.Value.Current);
            }
            finally
            {
                state.Dispose();
            }
        }
    }
}
