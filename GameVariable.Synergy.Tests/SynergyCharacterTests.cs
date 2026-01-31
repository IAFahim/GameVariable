using Xunit;
using Variable.Bounded;
using Variable.Regen;
using Variable.Timer;
using Variable.Experience;
using GameVariable.Synergy;

namespace GameVariable.Synergy.Tests;

public class SynergyCharacterTests
{
    [Fact]
    public void Should_Manage_Resources_And_Cooldowns_Correctly()
    {
        // 1. Setup
        // Health: 100, Mana: 50/50 (+10/sec), CD: 3s (starts ready), XP: 1000
        var character = new SynergyCharacter(100f, 50f, 10f, 3f, 1000);

        // Verify initial state
        Assert.Equal(50f, character.Mana.Value.Current);
        Assert.Equal(0f, character.FireballCd.Current); // Starts ready
        Assert.Equal(3f, character.FireballCd.Duration);

        // 2. Cast Fireball (Success)
        // Consumes 20 Mana, Resets CD to 3s
        bool cast1 = character.TryCastFireball(20f);
        Assert.True(cast1);
        Assert.Equal(30f, character.Mana.Value.Current); // 50 - 20
        Assert.Equal(3f, character.FireballCd.Current); // Reset to duration

        // 3. Try Cast Again (Fail - Cooldown)
        bool cast2 = character.TryCastFireball(20f);
        Assert.False(cast2);
        Assert.Equal(30f, character.Mana.Value.Current); // No change

        // 4. Update (1 sec)
        character.Update(1f);
        // Mana: 30 + 10 = 40
        // CD: 3 - 1 = 2
        Assert.Equal(40f, character.Mana.Value.Current);
        Assert.Equal(2f, character.FireballCd.Current);

        // 5. Try Cast Again (Fail - Cooldown)
        bool cast3 = character.TryCastFireball(20f);
        Assert.False(cast3);

        // 6. Update (2.1 sec) - Cooldown finishes, Mana fills to max
        character.Update(2.1f);
        // Mana: 40 + 21 = 61 -> Clamped to 50 (Max)
        // CD: 2 - 2.1 = -0.1 -> Clamped to 0
        Assert.Equal(50f, character.Mana.Value.Current);
        Assert.Equal(0f, character.FireballCd.Current);

        // 7. Cast Fireball (Success)
        bool cast4 = character.TryCastFireball(20f);
        Assert.True(cast4);
        Assert.Equal(30f, character.Mana.Value.Current);
        Assert.Equal(3f, character.FireballCd.Current);
    }
}
