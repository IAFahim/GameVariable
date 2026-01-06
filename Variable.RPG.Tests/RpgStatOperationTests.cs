namespace Variable.RPG.Tests;

/// <summary>
///     Tests for RpgStatOperation and modifier application system.
/// </summary>
public class RpgStatOperationTests
{
    #region Basic Operations

    [Fact]
    public void Operation_Set_Works()
    {
        var stat = new RpgStat(10f);
        var mod = new RpgStatModifier(RpgStatField.Base, RpgStatOperation.Set, 50f);

        stat.ApplyModifier(mod);

        Assert.Equal(50f, stat.Base);
    }

    [Fact]
    public void Operation_Add_Works()
    {
        var stat = new RpgStat(10f);
        var mod = RpgStatModifier.AddFlat(RpgStatField.Base, 5f);

        stat.ApplyModifier(mod);

        Assert.Equal(15f, stat.Base);
    }

    [Fact]
    public void Operation_Subtract_Works()
    {
        var stat = new RpgStat(100f);
        var mod = new RpgStatModifier(RpgStatField.Base, RpgStatOperation.Subtract, 30f);

        stat.ApplyModifier(mod);

        Assert.Equal(70f, stat.Base);
    }

    [Fact]
    public void Operation_Multiply_Works()
    {
        var stat = new RpgStat(10f);
        stat.ModMult = 1.5f;
        var mod = new RpgStatModifier(RpgStatField.ModMult, RpgStatOperation.Multiply, 2f);

        stat.ApplyModifier(mod);

        Assert.Equal(3f, stat.ModMult); // 1.5 * 2 = 3
    }

    [Fact]
    public void Operation_Divide_Works()
    {
        var stat = new RpgStat(100f);
        var mod = new RpgStatModifier(RpgStatField.Base, RpgStatOperation.Divide, 2f);

        stat.ApplyModifier(mod);

        Assert.Equal(50f, stat.Base);
    }

    [Fact]
    public void Operation_Divide_ByZero_DoesNothing()
    {
        var stat = new RpgStat(100f);
        var mod = new RpgStatModifier(RpgStatField.Base, RpgStatOperation.Divide, 0f);

        stat.ApplyModifier(mod);

        Assert.Equal(100f, stat.Base); // Unchanged
    }

    #endregion

    #region Percentage Operations

    [Fact]
    public void Operation_AddPercent_Works()
    {
        var stat = new RpgStat(100f); // Base 100
        stat.ModAdd = 10f; // Current ModAdd is 10

        // Add 20% of Base (100) to ModAdd
        var mod = RpgStatModifier.AddPercent(RpgStatField.ModAdd, 0.2f);
        stat.ApplyModifier(mod);

        Assert.Equal(30f, stat.ModAdd); // 10 + (100 * 0.2) = 30
    }

    [Fact]
    public void Operation_SubtractPercent_Works()
    {
        var stat = new RpgStat(100f); // Base 100
        stat.Max = 200f;

        // Subtract 10% of Base (100) from Max
        var mod = new RpgStatModifier(RpgStatField.Max, RpgStatOperation.SubtractPercent, 0.1f);
        stat.ApplyModifier(mod);

        Assert.Equal(190f, stat.Max); // 200 - (100 * 0.1) = 190
    }

    [Fact]
    public void Operation_AddPercentOfCurrent_Works()
    {
        var stat = new RpgStat(100f);

        // Add 50% of current value
        var mod = new RpgStatModifier(RpgStatField.Base, RpgStatOperation.AddPercentOfCurrent, 0.5f);
        stat.ApplyModifier(mod);

        Assert.Equal(150f, stat.Base); // 100 + (100 * 0.5) = 150
    }

    [Fact]
    public void Operation_SubtractPercentOfCurrent_Works()
    {
        var stat = new RpgStat(100f);

        // Subtract 25% of current value
        var mod = new RpgStatModifier(RpgStatField.Base, RpgStatOperation.SubtractPercentOfCurrent, 0.25f);
        stat.ApplyModifier(mod);

        Assert.Equal(75f, stat.Base); // 100 - (100 * 0.25) = 75
    }

    #endregion

    #region Min/Max Operations

    [Fact]
    public void Operation_Min_CapsValue()
    {
        var stat = new RpgStat(100f);

        // Cap at 50
        var mod = new RpgStatModifier(RpgStatField.Base, RpgStatOperation.Min, 50f);
        stat.ApplyModifier(mod);

        Assert.Equal(50f, stat.Base);
    }

    [Fact]
    public void Operation_Min_PreservesLowerValue()
    {
        var stat = new RpgStat(30f);

        // Try to cap at 50, but current is lower
        var mod = new RpgStatModifier(RpgStatField.Base, RpgStatOperation.Min, 50f);
        stat.ApplyModifier(mod);

        Assert.Equal(30f, stat.Base); // Unchanged
    }

    [Fact]
    public void Operation_Max_EnsuresMinimum()
    {
        var stat = new RpgStat(5f);

        // Ensure at least 10
        var mod = new RpgStatModifier(RpgStatField.Base, RpgStatOperation.Max, 10f);
        stat.ApplyModifier(mod);

        Assert.Equal(10f, stat.Base);
    }

    [Fact]
    public void Operation_Max_PreservesHigherValue()
    {
        var stat = new RpgStat(50f);

        // Try to ensure at least 10, but current is higher
        var mod = new RpgStatModifier(RpgStatField.Base, RpgStatOperation.Max, 10f);
        stat.ApplyModifier(mod);

        Assert.Equal(50f, stat.Base); // Unchanged
    }

    #endregion

    #region Multiple Modifiers

    [Fact]
    public void ApplyModifiers_Multiple_Works()
    {
        var stat = new RpgStat(100f, 0f, 500f);

        var modifiers = new[]
        {
            RpgStatModifier.AddFlat(RpgStatField.Base, 20f), // 100 + 20 = 120
            RpgStatModifier.AddFlat(RpgStatField.ModAdd, 10f), // 0 + 10 = 10
            RpgStatModifier.AddFlat(RpgStatField.ModMult, 0.5f) // 1 + 0.5 = 1.5
        };

        var count = stat.ApplyModifiers(modifiers);

        Assert.Equal(3, count);
        Assert.Equal(120f, stat.Base);
        Assert.Equal(10f, stat.ModAdd);
        // (120 + 10) * 1.5 = 195
        Assert.Equal(195f, stat.GetValue());
    }

    [Fact]
    public void ApplyModifiers_RecalculatesOnce()
    {
        var stat = new RpgStat(10f);

        var modifiers = new[]
        {
            RpgStatModifier.AddFlat(RpgStatField.Base, 5f),
            RpgStatModifier.AddFlat(RpgStatField.ModAdd, 3f),
            RpgStatModifier.AddFlat(RpgStatField.ModMult, 0.2f) // Add to multiplier
        };

        stat.ApplyModifiers(modifiers);

        // (15 + 3) * 1.2 = 21.6
        Assert.Equal(21.6f, stat.GetValue(), 0.01f);
    }

    #endregion

    #region RPG Scenarios

    [Fact]
    public void Scenario_EquipWeapon_AddsDamage()
    {
        var damage = new RpgStat(10f); // Base damage

        // Weapon: +5 damage, +20% damage multiplier
        var weaponMods = new[]
        {
            RpgStatModifier.AddFlat(RpgStatField.Base, 5f),
            RpgStatModifier.AddFlat(RpgStatField.ModMult, 0.2f) // Add 0.2 to multiplier
        };

        damage.ApplyModifiers(weaponMods);

        // (10 + 5) * 1.2 = 18
        Assert.Equal(18f, damage.GetValue());
    }

    [Fact]
    public void Scenario_LevelUp_IncreasesMaxHealth()
    {
        var health = new RpgStat(100f, 0f, 100f);

        // Level up: +20 max health
        var levelUpMod = RpgStatModifier.AddFlat(RpgStatField.Max, 20f);
        health.ApplyModifier(levelUpMod);

        Assert.Equal(120f, health.Max);
    }

    [Fact]
    public void Scenario_Buff_IncreasesStrength()
    {
        var strength = new RpgStat(50f);

        // Buff: +30% strength multiplier for 10 seconds
        var buffMod = RpgStatModifier.AddFlat(RpgStatField.ModMult, 0.3f);
        strength.ApplyModifier(buffMod);

        // 50 * 1.3 = 65
        Assert.Equal(65f, strength.GetValue());
    }

    [Fact]
    public void Scenario_Debuff_ReducesArmor()
    {
        var armor = new RpgStat(100f);

        // Debuff: -50% armor
        var debuffMod = new RpgStatModifier(RpgStatField.Base, RpgStatOperation.SubtractPercentOfCurrent, 0.5f);
        armor.ApplyModifier(debuffMod);

        Assert.Equal(50f, armor.Base);
    }

    [Fact]
    public void Scenario_EnchantedArmor_MultipleStats()
    {
        var armor = new RpgStat(50f);
        var health = new RpgStat(100f, 0f, 200f);

        // Enchanted Armor: +10 armor, +20% max health (of base)
        var armorMod = RpgStatModifier.AddFlat(RpgStatField.Base, 10f);
        var healthMod = RpgStatModifier.AddPercent(RpgStatField.Max, 0.2f);

        armor.ApplyModifier(armorMod);
        health.ApplyModifier(healthMod);

        Assert.Equal(60f, armor.GetValue());
        Assert.Equal(220f, health.Max); // 200 + (100 * 0.2)
    }

    [Fact]
    public void Scenario_PassiveAbility_IncreasesAllDamage()
    {
        var physicalDamage = new RpgStat(20f);
        var magicDamage = new RpgStat(15f);

        // Passive: +15% all damage (add to multiplier)
        var passiveMod = RpgStatModifier.AddFlat(RpgStatField.ModMult, 0.15f);

        physicalDamage.ApplyModifier(passiveMod);
        magicDamage.ApplyModifier(passiveMod);

        Assert.Equal(23f, physicalDamage.GetValue()); // 20 * 1.15
        Assert.Equal(17.25f, magicDamage.GetValue()); // 15 * 1.15
    }

    [Fact]
    public void Scenario_PercentHealthPotion_Heals()
    {
        var health = new RpgStat(50f, 0f, 100f);

        // Health Potion: Heal 50% of current health
        var potionMod = new RpgStatModifier(RpgStatField.Base, RpgStatOperation.AddPercentOfCurrent, 0.5f);
        health.ApplyModifier(potionMod);

        Assert.Equal(75f, health.Base); // 50 + (50 * 0.5) = 75
    }

    [Fact]
    public void Scenario_ArmorBreak_SetToZero()
    {
        var armor = new RpgStat(100f);

        // Armor Break skill: Set armor to 0
        var breakMod = RpgStatModifier.SetValue(RpgStatField.Base, 0f);
        armor.ApplyModifier(breakMod);

        Assert.Equal(0f, armor.Base);
    }

    [Fact]
    public void Scenario_DamageCapDebuff_MaxDamage()
    {
        var damage = new RpgStat(100f);

        // Debuff: Cap damage at 20
        var capMod = new RpgStatModifier(RpgStatField.Max, RpgStatOperation.Set, 20f);
        damage.ApplyModifier(capMod);

        Assert.Equal(20f, damage.GetValue()); // Clamped
    }

    #endregion

    #region Factory Methods

    [Fact]
    public void Factory_AddFlat_CreatesCorrectModifier()
    {
        var mod = RpgStatModifier.AddFlat(RpgStatField.Base, 5f);

        Assert.Equal(RpgStatField.Base, mod.Field);
        Assert.Equal(RpgStatOperation.Add, mod.Operation);
        Assert.Equal(5f, mod.Value);
    }

    [Fact]
    public void Factory_AddPercent_CreatesCorrectModifier()
    {
        var mod = RpgStatModifier.AddPercent(RpgStatField.ModMult, 0.2f);

        Assert.Equal(RpgStatField.ModMult, mod.Field);
        Assert.Equal(RpgStatOperation.AddPercent, mod.Operation);
        Assert.Equal(0.2f, mod.Value);
    }

    [Fact]
    public void Factory_SetValue_CreatesCorrectModifier()
    {
        var mod = RpgStatModifier.SetValue(RpgStatField.Max, 200f);

        Assert.Equal(RpgStatField.Max, mod.Field);
        Assert.Equal(RpgStatOperation.Set, mod.Operation);
        Assert.Equal(200f, mod.Value);
    }

    #endregion
}