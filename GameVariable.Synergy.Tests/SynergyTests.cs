using NUnit.Framework;
using GameVariable.Synergy;
using Variable.Experience;
using Variable.RPG;
using Variable.Bounded; // For IsFull if needed, but Experience has it too

namespace GameVariable.Synergy.Tests;

public class SynergyTests
{
    [Test]
    public void Synergy_FullFlow_Works()
    {
        // 1. Initialize
        var state = new SynergyState();
        state.Initialize(startLevel: 1);

        try
        {
            // Verify Initial Stats (Level 1)
            // Vit: 10 + (1 * 2) = 12
            // HP: 12 * 10 = 120
            Assert.That(state.Level.Level, Is.EqualTo(1));
            Assert.That(state.Stats.Get(StatIds.Vitality), Is.EqualTo(12f).Within(0.01f));
            Assert.That(state.Health.Value.Max, Is.EqualTo(120f).Within(0.01f));
            Assert.That(state.Health.Value.Current, Is.EqualTo(120f).Within(0.01f));

            // 2. Take Damage
            state.Health.Value -= 50f;
            Assert.That(state.Health.Value.Current, Is.EqualTo(70f).Within(0.01f));

            // 3. Level Up (Add XP)
            // Manually simulate level up by setting the struct directly
            // because we don't have a concrete INextMaxFormula here easily without defining one.
            state.Level = new ExperienceInt(2000, 0, 2);

            // 4. Update Flow
            state.Update(1.0f); // 1 sec delta

            // Verify Stats Increased (Level 2)
            // Vit: 10 + (2*2) = 14
            Assert.That(state.Stats.Get(StatIds.Vitality), Is.EqualTo(14f).Within(0.01f));

            // Verify Max HP Increased
            // HP: 14 * 10 = 140
            Assert.That(state.Health.Value.Max, Is.EqualTo(140f).Within(0.01f));

            // Verify Regen
            // Spi: 2 + (2*0.5) = 3
            // Rate: 3 * 0.5 = 1.5
            Assert.That(state.Health.Rate, Is.EqualTo(1.5f).Within(0.01f));

            // Verify Health Regenerated
            // Start: 70
            // Regen: 1.5 * 1.0s = 1.5
            // Expected: 71.5
            Assert.That(state.Health.Value.Current, Is.EqualTo(71.5f).Within(0.01f));
        }
        finally
        {
            state.Dispose();
        }
    }
}
