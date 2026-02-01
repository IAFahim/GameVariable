using Xunit;
using GameVariable.Synergy;
using Variable.Bounded;
using Variable.Regen;
using Variable.Timer;
using Variable.Experience;

namespace GameVariable.Synergy.Tests
{
    public class SynergyTests
    {
        [Fact]
        public void SynergyCharacter_Tick_UpdatesRegenAndCooldown()
        {
            // Arrange
            // Mana: 100 max, 0 current, 10 rate -> full in 10s
            // Cooldown: 5s duration, 5s current (not ready)
            var character = new SynergyCharacter
            {
                Health = new BoundedFloat(100f),
                Mana = new RegenFloat(100f, 0f, 10f),
                AbilityCooldown = new Cooldown(5f, 5f),
                XP = new ExperienceInt(1000)
            };

            // Act
            // Tick 1 second
            character.Tick(1f);

            // Assert
            // Mana should be 10
            Assert.Equal(10f, character.Mana.Value.Current, 1);

            // Cooldown should be 4
            Assert.Equal(4f, character.AbilityCooldown.Current, 1);
        }

        [Fact]
        public void SynergyCharacter_TryCast_Success_ConsumesManaAndResetsCooldown()
        {
            // Arrange
            // Mana: 50 current
            // Cooldown: 0 current (Ready)
            var character = new SynergyCharacter
            {
                Mana = new RegenFloat(100f, 50f, 0f),
                AbilityCooldown = new Cooldown(5f, 0f)
            };

            // Act
            bool success = character.TryCast(20f);

            // Assert
            Assert.True(success);
            // Mana: 50 - 20 = 30
            Assert.Equal(30f, character.Mana.Value.Current);
            // Cooldown: Should reset to Duration (5)
            Assert.Equal(5f, character.AbilityCooldown.Current);
        }

        [Fact]
        public void SynergyCharacter_TryCast_Fail_NotEnoughMana()
        {
            // Arrange
            // Mana: 10 current
            // Cooldown: 0 current (Ready)
            var character = new SynergyCharacter
            {
                Mana = new RegenFloat(100f, 10f, 0f),
                AbilityCooldown = new Cooldown(5f, 0f)
            };

            // Act
            bool success = character.TryCast(20f);

            // Assert
            Assert.False(success);
            Assert.Equal(10f, character.Mana.Value.Current);
            Assert.Equal(0f, character.AbilityCooldown.Current);
        }

        [Fact]
        public void SynergyCharacter_TryCast_Fail_CooldownNotReady()
        {
            // Arrange
            // Mana: 100 current
            // Cooldown: 2 current (Not Ready)
            var character = new SynergyCharacter
            {
                Mana = new RegenFloat(100f, 100f, 0f),
                AbilityCooldown = new Cooldown(5f, 2f)
            };

            // Act
            bool success = character.TryCast(20f);

            // Assert
            Assert.False(success);
            Assert.Equal(100f, character.Mana.Value.Current);
            Assert.Equal(2f, character.AbilityCooldown.Current);
        }
    }
}
