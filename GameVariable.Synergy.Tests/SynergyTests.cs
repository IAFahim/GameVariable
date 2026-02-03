namespace GameVariable.Synergy.Tests;

public class SynergyTests
{
    [Fact]
    public void Tick_ShouldUpdateRegenAndCooldown()
    {
        // Arrange
        // HP: 100, MP: 50/100 (5 regen/s), CD: 3s
        var hero = new SynergyCharacter(100f, 100f, 5f, 3f, 1000);

        // Manually set state for testing
        // Note: Direct field access works because everything is a struct and we have public fields.
        hero.Mana.Value.Current = 50f; // Start with half mana
        hero.AbilityCooldown.Current = 3f; // Start on cooldown

        // Act
        // Tick 1 second
        hero.Tick(1f);

        // Assert
        Assert.Equal(55f, hero.Mana.Value.Current, 0.001f); // 50 + 5
        Assert.Equal(2f, hero.AbilityCooldown.Current, 0.001f); // 3 - 1
    }

    [Fact]
    public void TryCast_ShouldSucceed_WhenResourcesAvailable()
    {
        // Arrange
        var hero = new SynergyCharacter(100f, 100f, 5f, 3f, 1000);
        hero.Mana.Value.Current = 50f;
        hero.AbilityCooldown.Current = 0f; // Ready

        // Act
        var success = hero.TryCastAbility(20f);

        // Assert
        Assert.True(success);
        Assert.Equal(30f, hero.Mana.Value.Current, 0.001f); // 50 - 20
        Assert.Equal(3f, hero.AbilityCooldown.Current, 0.001f); // Reset to full duration
    }

    [Fact]
    public void TryCast_ShouldFail_WhenOnCooldown()
    {
        // Arrange
        var hero = new SynergyCharacter(100f, 100f, 5f, 3f, 1000);
        hero.Mana.Value.Current = 50f;
        hero.AbilityCooldown.Current = 1f; // On Cooldown

        // Act
        var success = hero.TryCastAbility(20f);

        // Assert
        Assert.False(success);
        Assert.Equal(50f, hero.Mana.Value.Current, 0.001f); // No change
        Assert.Equal(1f, hero.AbilityCooldown.Current, 0.001f); // No change
    }

    [Fact]
    public void TryCast_ShouldFail_WhenNotEnoughMana()
    {
        // Arrange
        var hero = new SynergyCharacter(100f, 100f, 5f, 3f, 1000);
        hero.Mana.Value.Current = 10f; // Low mana
        hero.AbilityCooldown.Current = 0f;

        // Act
        var success = hero.TryCastAbility(20f);

        // Assert
        Assert.False(success);
        Assert.Equal(10f, hero.Mana.Value.Current, 0.001f); // No change
    }

    [Fact]
    public void Integration_Scenario()
    {
        // A full scenario: Cast -> Wait -> Regen -> Cast again
        var hero = new SynergyCharacter(100f, 100f, 10f, 2f, 1000);
        // Start: 100 MP, 0 CD

        // 1. First Cast (Cost 50)
        Assert.True(hero.TryCastAbility(50f));
        Assert.Equal(50f, hero.Mana.Value.Current);
        Assert.Equal(2f, hero.AbilityCooldown.Current);

        // 2. Tick 1.5 seconds
        hero.Tick(1.5f);
        // Mana: 50 + (10 * 1.5) = 65
        // CD: 2 - 1.5 = 0.5
        Assert.Equal(65f, hero.Mana.Value.Current, 0.001f);
        Assert.Equal(0.5f, hero.AbilityCooldown.Current, 0.001f);

        // 3. Try Cast (Fail due to Cooldown)
        Assert.False(hero.TryCastAbility(50f));

        // 4. Tick 1.0 second
        hero.Tick(1.0f);
        // Mana: 65 + 10 = 75
        // CD: 0.5 - 1.0 = -0.5 -> 0 (Ready)
        Assert.Equal(75f, hero.Mana.Value.Current, 0.001f);
        Assert.Equal(0f, hero.AbilityCooldown.Current, 0.001f);

        // 5. Try Cast (Succeed)
        Assert.True(hero.TryCastAbility(50f));
        Assert.Equal(25f, hero.Mana.Value.Current, 0.001f); // 75 - 50
        Assert.Equal(2f, hero.AbilityCooldown.Current, 0.001f);
    }
}
