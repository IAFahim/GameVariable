using Xunit;
using GameVariable.Synergy;
using Variable.Bounded;
using Variable.Timer;
using Variable.Regen;
using Variable.Experience;

namespace GameVariable.Synergy.Tests;

public class SynergyCharacterTests
{
    [Fact]
    public void Synergy_Integration_Scenario()
    {
        // 1. Setup
        var hero = new SynergyCharacter(
            maxHealth: 100f,
            maxMana: 50f,
            manaRegen: 5f, // 5 mana per second
            cooldown: 2f   // 2 second cooldown
        );

        // Initial check
        Assert.Equal(100f, hero.Health.Current);
        Assert.Equal(50f, hero.Mana.Value.Current);
        Assert.True(hero.AttackCooldown.IsReady());

        // 2. Cast Ability (Success)
        bool cast1 = hero.TryCastAbility(manaCost: 20f);
        Assert.True(cast1);

        // Mana should be reduced
        Assert.Equal(30f, hero.Mana.Value.Current);

        // Cooldown should be active (Reset sets Current to Duration)
        Assert.False(hero.AttackCooldown.IsReady());
        Assert.Equal(2f, hero.AttackCooldown.Current);

        // 3. Try Cast Again (Fail due to Cooldown)
        bool cast2 = hero.TryCastAbility(manaCost: 10f);
        Assert.False(cast2);
        Assert.Equal(30f, hero.Mana.Value.Current); // Mana not consumed

        // 4. Update Time (1 second)
        hero.Update(1f);

        // Cooldown should be 1s
        Assert.Equal(1f, hero.AttackCooldown.Current);

        // Mana should have regenerated (+5)
        Assert.Equal(35f, hero.Mana.Value.Current);

        // 5. Update Time (1.1 second) -> Total 2.1s
        hero.Update(1.1f);

        // Cooldown ready
        Assert.True(hero.AttackCooldown.IsReady());

        // Mana regenerated (+5.5) -> 40.5
        Assert.Equal(40.5f, hero.Mana.Value.Current);

        // 6. Cast Again (Success)
        bool cast3 = hero.TryCastAbility(manaCost: 40f);
        Assert.True(cast3);
        Assert.Equal(0.5f, hero.Mana.Value.Current);

        // 7. Try Cast Again (Fail due to Mana)
        // Wait 2 seconds for cooldown to clear
        hero.Update(2f);
        Assert.True(hero.AttackCooldown.IsReady());
        Assert.Equal(10.5f, hero.Mana.Value.Current); // 0.5 + 10 = 10.5

        bool cast4 = hero.TryCastAbility(manaCost: 20f);
        Assert.False(cast4); // Not enough mana
    }

    [Fact]
    public void Synergy_LevelUp_Scenario()
    {
        var hero = new SynergyCharacter(100f, 50f, 5f, 2f);
        // Reduce Health and Mana to verify refill on level up
        hero.Health -= 50f;
        hero.Mana.Value -= 40f;

        Assert.Equal(50f, hero.Health.Current);
        Assert.Equal(10f, hero.Mana.Value.Current);

        // Add XP (Need 100 for level 2)
        hero.AwardExperience(50, out bool leveledUp);
        Assert.False(leveledUp);
        Assert.Equal(50, hero.Experience.Current);

        // Add more XP to Level Up
        hero.AwardExperience(50, out leveledUp);
        Assert.True(leveledUp);

        // Verify Full Stats Refill
        Assert.Equal(100f, hero.Health.Current);
        Assert.Equal(50f, hero.Mana.Value.Current);

        // Verify Level and Next Target (Doubled)
        Assert.Equal(2, hero.Experience.Level);
        Assert.Equal(200, hero.Experience.Max);
        Assert.Equal(0, hero.Experience.Current); // We discarded overflow in logic
    }
}
