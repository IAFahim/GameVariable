using GameVariable.Synergy;
using Variable.Bounded;
using Variable.Regen;
using Variable.Timer;
using Variable.Experience;

namespace GameVariable.Synergy.Tests;

public class SynergyTests
{
    [Fact]
    public void Update_TicksComponents()
    {
        // Arrange
        // Mana: 0/100, Regen +10/sec
        // Cooldown: 10s duration, starts at 10s
        var charState = new SynergyCharacter
        {
            Mana = new RegenFloat(100f, 0f, 10f),
            AbilityCooldown = new Cooldown(10f, 10f)
        };

        // Act
        // Tick 1 second
        charState.Update(1.0f);

        // Assert
        Assert.Equal(10f, charState.Mana.Value.Current, 0.001f);
        Assert.Equal(9f, charState.AbilityCooldown.Current, 0.001f);
    }

    [Fact]
    public void TryCastAbility_Succeeds_WhenReady()
    {
        // Arrange
        // Mana: 50/100
        // Cooldown: Ready (0)
        var charState = new SynergyCharacter
        {
            Mana = new RegenFloat(100f, 50f, 0f),
            AbilityCooldown = new Cooldown(5f, 0f)
        };

        float cost = 20f;

        // Act
        charState.TryCastAbility(cost, out bool success);

        // Assert
        Assert.True(success);
        Assert.Equal(30f, charState.Mana.Value.Current, 0.001f); // 50 - 20
        Assert.Equal(5f, charState.AbilityCooldown.Current, 0.001f); // Reset to max
    }

    [Fact]
    public void TryCastAbility_Fails_WhenNotEnoughMana()
    {
        // Arrange
        var charState = new SynergyCharacter
        {
            Mana = new RegenFloat(100f, 10f, 0f),
            AbilityCooldown = new Cooldown(5f, 0f)
        };

        float cost = 20f;

        // Act
        charState.TryCastAbility(cost, out bool success);

        // Assert
        Assert.False(success);
        Assert.Equal(10f, charState.Mana.Value.Current); // Unchanged
        Assert.Equal(0f, charState.AbilityCooldown.Current); // Still ready
    }

    [Fact]
    public void TryCastAbility_Fails_WhenOnCooldown()
    {
        // Arrange
        var charState = new SynergyCharacter
        {
            Mana = new RegenFloat(100f, 100f, 0f),
            AbilityCooldown = new Cooldown(5f, 2f) // 2s remaining
        };

        float cost = 20f;

        // Act
        charState.TryCastAbility(cost, out bool success);

        // Assert
        Assert.False(success);
        Assert.Equal(100f, charState.Mana.Value.Current); // Unchanged
        Assert.Equal(2f, charState.AbilityCooldown.Current); // Unchanged
    }
}
