using Xunit;
using GameVariable.Synergy;
using Variable.Bounded;
using Variable.Regen;
using Variable.Timer;
using Variable.Experience;

namespace GameVariable.Synergy.Tests;

public class SynergyTests
{
    [Fact]
    public void FullFlow_Integration_Test()
    {
        // Arrange
        var character = new SynergyCharacter
        {
            Health = new BoundedFloat(100f),
            Mana = new RegenFloat(100f, 100f, 5f), // Max 100, Curr 100, Rate 5
            AbilityCooldown = new Cooldown(3f),    // Duration 3s
            XP = new ExperienceInt(1000)
        };

        // Act & Assert 1: Cast Ability (Success)
        bool cast1 = character.TryCastAbility(20f);
        Assert.True(cast1, "Should be able to cast initially");
        Assert.Equal(80f, character.Mana.Value.Current);
        Assert.Equal(3f, character.AbilityCooldown.Current); // Full cooldown
        Assert.False(character.AbilityCooldown.IsReady(), "Cooldown should be active");

        // Act & Assert 2: Tick 1s (Regen +5, Cooldown -1)
        character.Tick(1f);
        Assert.Equal(85f, character.Mana.Value.Current);
        Assert.Equal(2f, character.AbilityCooldown.Current);

        // Act & Assert 3: Try Cast (Fail due to Cooldown)
        bool cast2 = character.TryCastAbility(20f);
        Assert.False(cast2, "Should not cast while on cooldown");
        Assert.Equal(85f, character.Mana.Value.Current); // Mana should not be consumed

        // Act & Assert 4: Tick 2.1s (Regen +10.5, Cooldown -2.1 -> Ready)
        character.Tick(2.1f);
        // Mana: 85 + 10.5 = 95.5
        Assert.Equal(95.5f, character.Mana.Value.Current, 0.001f);
        Assert.True(character.AbilityCooldown.IsReady(), "Cooldown should be ready");

        // Act & Assert 5: Cast Ability (Success)
        bool cast3 = character.TryCastAbility(20f);
        Assert.True(cast3, "Should cast after cooldown");
        // Mana: 95.5 - 20 = 75.5
        Assert.Equal(75.5f, character.Mana.Value.Current, 0.001f);
        Assert.Equal(3f, character.AbilityCooldown.Current);
    }

    [Fact]
    public void Mana_Check_Test()
    {
        // Arrange
        var character = new SynergyCharacter
        {
            Mana = new RegenFloat(100f, 10f, 0f), // Low Mana
            AbilityCooldown = new Cooldown(1f)
        };

        // Act & Assert
        bool cast = character.TryCastAbility(20f);
        Assert.False(cast, "Should fail due to insufficient mana");
    }
}
