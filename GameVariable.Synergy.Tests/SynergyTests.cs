using GameVariable.Synergy;
using Variable.Bounded;
using Variable.Experience;
using Variable.Regen;
using Variable.Timer;

namespace GameVariable.Synergy.Tests;

public class SynergyTests
{
    [Fact]
    public void Tick_UpdatesManaAndCooldown()
    {
        // Arrange
        var health = new BoundedFloat(100f, 100f);
        var mana = new RegenFloat(100f, 50f, 10f); // 50 start, +10/sec
        var cooldown = new Cooldown(5f, 2f); // 5s duration, 2s remaining
        var xp = new ExperienceInt(1000, 0, 1);

        var character = new SynergyCharacter(health, mana, cooldown, xp);

        // Act
        character.Tick(0.5f); // Advance 0.5 seconds

        // Assert
        Assert.Equal(55f, character.Mana.Value.Current, 0.001f); // 50 + (10 * 0.5) = 55
        Assert.Equal(1.5f, character.AbilityCooldown.Current, 0.001f); // 2 - 0.5 = 1.5
    }

    [Fact]
    public void Tick_ClampsValues()
    {
        // Arrange
        var health = new BoundedFloat(100f, 100f);
        var mana = new RegenFloat(100f, 95f, 10f); // 95 start, +10/sec
        var cooldown = new Cooldown(5f, 0.2f); // 0.2s remaining
        var xp = new ExperienceInt(1000);

        var character = new SynergyCharacter(health, mana, cooldown, xp);

        // Act
        character.Tick(1.0f); // Advance 1 second

        // Assert
        Assert.Equal(100f, character.Mana.Value.Current); // Clamped to Max 100
        Assert.Equal(0f, character.AbilityCooldown.Current); // Clamped to 0
    }
}
