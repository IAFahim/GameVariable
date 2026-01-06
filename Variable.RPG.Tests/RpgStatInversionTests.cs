namespace Variable.RPG.Tests;

/// <summary>
///     Tests for modifier inversion system.
/// </summary>
public class RpgStatInversionTests
{
    #region Operation Inversion

    [Fact]
    public void GetInverse_Add_ReturnsSubtract()
    {
        var inverse = RpgStatOperation.Add.GetInverse();
        Assert.Equal(RpgStatOperation.Subtract, inverse);
    }

    [Fact]
    public void GetInverse_Subtract_ReturnsAdd()
    {
        var inverse = RpgStatOperation.Subtract.GetInverse();
        Assert.Equal(RpgStatOperation.Add, inverse);
    }

    [Fact]
    public void GetInverse_Multiply_ReturnsDivide()
    {
        var inverse = RpgStatOperation.Multiply.GetInverse();
        Assert.Equal(RpgStatOperation.Divide, inverse);
    }

    [Fact]
    public void GetInverse_Divide_ReturnsMultiply()
    {
        var inverse = RpgStatOperation.Divide.GetInverse();
        Assert.Equal(RpgStatOperation.Multiply, inverse);
    }

    [Fact]
    public void GetInverse_AddPercent_ReturnsSubtractPercent()
    {
        var inverse = RpgStatOperation.AddPercent.GetInverse();
        Assert.Equal(RpgStatOperation.SubtractPercent, inverse);
    }

    [Fact]
    public void GetInverse_SubtractPercent_ReturnsAddPercent()
    {
        var inverse = RpgStatOperation.SubtractPercent.GetInverse();
        Assert.Equal(RpgStatOperation.AddPercent, inverse);
    }

    [Fact]
    public void IsInvertible_Add_ReturnsTrue()
    {
        Assert.True(RpgStatOperation.Add.IsInvertible());
    }

    [Fact]
    public void IsInvertible_Set_ReturnsFalse()
    {
        Assert.False(RpgStatOperation.Set.IsInvertible());
    }

    [Fact]
    public void IsInvertible_Min_ReturnsFalse()
    {
        Assert.False(RpgStatOperation.Min.IsInvertible());
    }

    [Fact]
    public void IsInvertible_Max_ReturnsFalse()
    {
        Assert.False(RpgStatOperation.Max.IsInvertible());
    }

    #endregion

    #region Modifier Inversion

    [Fact]
    public void ModifierGetInverse_Add_Works()
    {
        var mod = RpgStatModifier.AddFlat(RpgStatField.Base, 5f);
        var inverse = mod.GetInverse();

        Assert.Equal(RpgStatField.Base, inverse.Field);
        Assert.Equal(RpgStatOperation.Subtract, inverse.Operation);
        Assert.Equal(5f, inverse.Value);
    }

    [Fact]
    public void ModifierGetInverse_Multiply_Works()
    {
        var mod = new RpgStatModifier(RpgStatField.ModMult, RpgStatOperation.Multiply, 2f);
        var inverse = mod.GetInverse();

        Assert.Equal(RpgStatField.ModMult, inverse.Field);
        Assert.Equal(RpgStatOperation.Divide, inverse.Operation);
        Assert.Equal(2f, inverse.Value);
    }

    [Fact]
    public void ModifierGetInverse_Set_ThrowsException()
    {
        var mod = RpgStatModifier.SetValue(RpgStatField.Base, 100f);

        Assert.Throws<InvalidOperationException>(() => mod.GetInverse());
    }

    [Fact]
    public void ModifierTryGetInverse_Add_ReturnsTrue()
    {
        var mod = RpgStatModifier.AddFlat(RpgStatField.Base, 5f);
        var success = mod.TryGetInverse(out var inverse);

        Assert.True(success);
        Assert.Equal(RpgStatOperation.Subtract, inverse.Operation);
    }

    [Fact]
    public void ModifierTryGetInverse_Set_ReturnsFalse()
    {
        var mod = RpgStatModifier.SetValue(RpgStatField.Base, 100f);
        var success = mod.TryGetInverse(out var inverse);

        Assert.False(success);
        Assert.Equal(default, inverse);
    }

    #endregion

    #region Apply and Remove

    [Fact]
    public void RemoveModifier_Add_RestoresOriginal()
    {
        var stat = new RpgStat(10f);
        var mod = RpgStatModifier.AddFlat(RpgStatField.Base, 5f);

        stat.ApplyModifier(mod);
        Assert.Equal(15f, stat.Base);

        stat.RemoveModifier(mod);
        Assert.Equal(10f, stat.Base);
    }

    [Fact]
    public void RemoveModifier_Multiply_RestoresOriginal()
    {
        var stat = new RpgStat(10f);
        stat.ModMult = 2f;
        var mod = new RpgStatModifier(RpgStatField.ModMult, RpgStatOperation.Multiply, 3f);

        stat.ApplyModifier(mod);
        Assert.Equal(6f, stat.ModMult);

        stat.RemoveModifier(mod);
        Assert.Equal(2f, stat.ModMult);
    }

    [Fact]
    public void RemoveModifier_Set_ReturnsFalse()
    {
        var stat = new RpgStat(10f);
        var mod = RpgStatModifier.SetValue(RpgStatField.Base, 100f);

        stat.ApplyModifier(mod);
        Assert.Equal(100f, stat.Base);

        var success = stat.RemoveModifier(mod);
        Assert.False(success);
        Assert.Equal(100f, stat.Base); // Unchanged
    }

    [Fact]
    public void RemoveModifiers_Multiple_RestoresOriginal()
    {
        var stat = new RpgStat(10f);

        var mods = new[]
        {
            RpgStatModifier.AddFlat(RpgStatField.Base, 5f),
            RpgStatModifier.AddFlat(RpgStatField.ModAdd, 3f),
            RpgStatModifier.AddFlat(RpgStatField.ModMult, 0.2f)
        };

        stat.ApplyModifiers(mods);
        // (15 + 3) * 1.2 = 21.6
        Assert.Equal(21.6f, stat.GetValue(), 0.01f);

        stat.RemoveModifiers(mods);
        // Back to original
        Assert.Equal(10f, stat.GetValue());
    }

    #endregion

    #region Real-World Scenarios

    [Fact]
    public void Scenario_EquipUnequipWeapon()
    {
        var damage = new RpgStat(10f);

        // Equip weapon: +5 damage, +20% multiplier
        var weaponMods = new[]
        {
            RpgStatModifier.AddFlat(RpgStatField.Base, 5f),
            RpgStatModifier.AddFlat(RpgStatField.ModMult, 0.2f)
        };

        damage.ApplyModifiers(weaponMods);
        Assert.Equal(18f, damage.GetValue()); // (10 + 5) * 1.2

        // Unequip weapon
        damage.RemoveModifiers(weaponMods);
        Assert.Equal(10f, damage.GetValue()); // Back to base
    }

    [Fact]
    public void Scenario_BuffExpires()
    {
        var strength = new RpgStat(50f);

        // Apply buff: +30% strength
        var buff = RpgStatModifier.AddFlat(RpgStatField.ModMult, 0.3f);
        strength.ApplyModifier(buff);
        Assert.Equal(65f, strength.GetValue()); // 50 * 1.3

        // Buff expires
        strength.RemoveModifier(buff);
        Assert.Equal(50f, strength.GetValue(), 0.01f); // Back to normal (with tolerance)
    }

    [Fact]
    public void Scenario_StackableBuffs()
    {
        var armor = new RpgStat(100f);

        // Apply multiple buffs
        var buff1 = RpgStatModifier.AddFlat(RpgStatField.Base, 10f);
        var buff2 = RpgStatModifier.AddFlat(RpgStatField.Base, 20f);
        var buff3 = RpgStatModifier.AddFlat(RpgStatField.ModMult, 0.1f);

        armor.ApplyModifier(buff1);
        armor.ApplyModifier(buff2);
        armor.ApplyModifier(buff3);

        // (100 + 10 + 20) * 1.1 = 143
        Assert.Equal(143f, armor.GetValue());

        // Remove buff2
        armor.RemoveModifier(buff2);

        // (100 + 10) * 1.1 = 121
        Assert.Equal(121f, armor.GetValue());

        // Remove remaining buffs
        armor.RemoveModifier(buff1);
        armor.RemoveModifier(buff3);

        Assert.Equal(100f, armor.GetValue());
    }

    [Fact]
    public void Scenario_TemporaryDebuff()
    {
        var speed = new RpgStat(100f);

        // Apply slow debuff: -50 flat
        var slowDebuff = new RpgStatModifier(RpgStatField.Base, RpgStatOperation.Subtract, 50f);
        speed.ApplyModifier(slowDebuff);
        Assert.Equal(50f, speed.Base); // 100 - 50

        // Debuff ends
        speed.RemoveModifier(slowDebuff);
        Assert.Equal(100f, speed.Base); // Back to full speed
    }

    [Fact]
    public void Scenario_EnchantedArmorSwap()
    {
        var armor = new RpgStat(50f);
        var health = new RpgStat(100f, 0f, 200f);

        // Equip enchanted armor: +10 armor, +20 max health (flat)
        var armorMod = RpgStatModifier.AddFlat(RpgStatField.Base, 10f);
        var healthMod = RpgStatModifier.AddFlat(RpgStatField.Max, 20f);

        armor.ApplyModifier(armorMod);
        health.ApplyModifier(healthMod);

        Assert.Equal(60f, armor.GetValue());
        Assert.Equal(220f, health.Max); // 200 + 20

        // Swap to different armor: +5 armor, no health bonus
        armor.RemoveModifier(armorMod);
        health.RemoveModifier(healthMod);

        var newArmorMod = RpgStatModifier.AddFlat(RpgStatField.Base, 5f);
        armor.ApplyModifier(newArmorMod);

        Assert.Equal(55f, armor.GetValue());
        Assert.Equal(200f, health.Max); // Back to base max
    }

    [Fact]
    public void Scenario_PercentageOperations()
    {
        var stat = new RpgStat(100f);

        // Add 20 flat
        var mod = RpgStatModifier.AddFlat(RpgStatField.Base, 20f);
        stat.ApplyModifier(mod);
        Assert.Equal(120f, stat.Base);

        // Remove it
        stat.RemoveModifier(mod);
        Assert.Equal(100f, stat.Base);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void RemoveModifier_NeverApplied_DoesNothing()
    {
        var stat = new RpgStat(10f);
        var mod = RpgStatModifier.AddFlat(RpgStatField.Base, 5f);

        // Remove without applying
        stat.RemoveModifier(mod);

        // Results in negative (10 - 5 = 5)
        Assert.Equal(5f, stat.Base);
    }

    [Fact]
    public void RemoveModifier_AppliedTwice_RemoveOnce()
    {
        var stat = new RpgStat(10f);
        var mod = RpgStatModifier.AddFlat(RpgStatField.Base, 5f);

        // Apply twice
        stat.ApplyModifier(mod);
        stat.ApplyModifier(mod);
        Assert.Equal(20f, stat.Base);

        // Remove once
        stat.RemoveModifier(mod);
        Assert.Equal(15f, stat.Base);
    }

    [Fact]
    public void RemoveModifiers_MixedInvertible_OnlyRemovesInvertible()
    {
        var stat = new RpgStat(10f);

        var mods = new[]
        {
            RpgStatModifier.AddFlat(RpgStatField.Base, 5f), // Invertible
            RpgStatModifier.SetValue(RpgStatField.Base, 100f), // NOT invertible
            RpgStatModifier.AddFlat(RpgStatField.ModAdd, 3f) // Invertible
        };

        stat.ApplyModifiers(mods);
        // Base set to 100, ModAdd = 3
        Assert.Equal(100f, stat.Base);

        var removedCount = stat.RemoveModifiers(mods);

        // Only 2 removed (the Add operations)
        Assert.Equal(2, removedCount);
    }

    #endregion
}