namespace GameVariable.Synergy.Tests;

public class SynergyCharacterTests
{
    [Fact]
    public void Synergy_RageMode_DoublesRegen()
    {
        // Arrange
        // Max 100, Rate 10.
        var stamina = new RegenFloat(100f, 0f, 10f);

        var character = new SynergyCharacter
        {
            Health = new BoundedFloat(100f),
            Stamina = stamina,
            Level = new ExperienceInt(100)
        };

        // Set Health to 20% (Low Health -> Rage Mode)
        character.Health.Set(20f);

        // Act
        character.Update(1.0f); // 1 Second

        // Assert
        // Normal regen is 10. Rage multiplier is 2. So should be 20.
        Assert.Equal(20f, character.Stamina.Value.Current);
    }

    [Fact]
    public void Synergy_NormalMode_NormalRegen()
    {
        // Arrange
        var stamina = new RegenFloat(100f, 0f, 10f);

        var character = new SynergyCharacter
        {
            Health = new BoundedFloat(100f),
            Stamina = stamina,
            Level = new ExperienceInt(100)
        };

        // Set Health to 100% (High Health -> Normal Mode)
        character.Health.Set(100f);

        // Act
        character.Update(1.0f); // 1 Second

        // Assert
        // Normal regen is 10. Multiplier is 1. So should be 10.
        Assert.Equal(10f, character.Stamina.Value.Current);
    }
}
