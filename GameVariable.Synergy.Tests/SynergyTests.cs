namespace GameVariable.Synergy.Tests;

public class SynergyTests
{
    [Fact]
    public void LevelUp_ShouldIncreaseStats_And_ReflectInRegen()
    {
        // 1. Initialize State
        // 2 Stats: 0=Vitality, 1=Wisdom
        var state = new SynergyState(2);

        try
        {
            // Initial Update to set base stats
            state.Update(0f);

            // Initial Check (Level 1)
            Assert.Equal(1, state.Experience.Level);

            // Vitality = 10 + 1*5 = 15
            // Wisdom = 5 + 1*2 = 7
            Assert.Equal(15f, state.Stats.Get(0));
            Assert.Equal(7f, state.Stats.Get(1));

            // Max Health = Vit * 10 = 150
            Assert.Equal(150f, state.Health.Max);

            // Max Mana = Wis * 10 = 70
            Assert.Equal(70f, state.Mana.Value.Max);

            // Regen Rate = Wis * 0.1 = 0.7
            Assert.Equal(0.7f, state.Mana.Rate, 0.001f);

            // 2. Level Up
            // Level 1 MaxXP is 100
            state.AddExperience(100);

            // Current Mana starts at 50 (from constructor)
            // But after first Update(0f), Max became 70. Current is still 50 (since Max increased, Current stays).
            float initialMana = state.Mana.Value.Current;
            Assert.Equal(50f, initialMana);

            // Update with 1 second delta to process level up and regen
            state.Update(1f);

            // Verify Level Up
            Assert.Equal(2, state.Experience.Level);

            // Verify Stats
            // Vitality = 10 + 2*5 = 20
            // Wisdom = 5 + 2*2 = 9
            Assert.Equal(20f, state.Stats.Get(0));
            Assert.Equal(9f, state.Stats.Get(1));

            // Verify Max Values
            Assert.Equal(200f, state.Health.Max);
            Assert.Equal(90f, state.Mana.Value.Max);

            // Verify Regen Rate
            // Rate = 9 * 0.1 = 0.9
            Assert.Equal(0.9f, state.Mana.Rate, 0.001f);

            // Verify Regen Applied
            // Rate was updated to 0.9 before Tick.
            // Expected Mana = 50 + 0.9 * 1.0 = 50.9.
            Assert.Equal(initialMana + 0.9f, state.Mana.Value.Current, 0.001f);
        }
        finally
        {
            state.Dispose();
        }
    }
}
