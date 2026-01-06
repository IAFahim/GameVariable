namespace Variable.RPG.Tests;

/// <summary>
///     Examples demonstrating the use of RpgStatField for selective field modification.
/// </summary>
public class RpgStatFieldExamples
{
    [Fact]
    public void Example_ChangeOnlyMaxValue()
    {
        // Create a health stat with 100 base, range 0-200
        var health = new RpgStat(100f, 0f, 200f);

        // Player leveled up, increase max health to 250
        health.TrySetField(RpgStatField.Max, 250f);

        Assert.Equal(250f, health.Max);
        Assert.Equal(100f, health.Base); // Base unchanged
    }

    [Fact]
    public void Example_DynamicMaxHealthFromEndurance()
    {
        // Base health is 100
        var health = new RpgStat(100f, 0f, 100f);

        // Player increases Endurance stat
        var endurance = 50f; // Each point of endurance adds 2 max health
        var newMaxHealth = 100f + endurance * 2f; // 100 + 100 = 200

        // Update max health based on endurance
        health.TrySetField(RpgStatField.Max, newMaxHealth);

        Assert.Equal(200f, health.Max);
    }

    [Fact]
    public void Example_TemporaryMaxHealthBuff()
    {
        var health = new RpgStat(100f, 0f, 100f);

        // Store original max
        health.TryGetField(RpgStatField.Max, out var originalMax);

        // Apply temporary +50 max health buff
        health.TrySetField(RpgStatField.Max, originalMax + 50f);
        Assert.Equal(150f, health.Max);

        // Buff expires, restore original max
        health.TrySetField(RpgStatField.Max, originalMax);
        Assert.Equal(100f, health.Max);
    }

    [Fact]
    public void Example_ReadAndModifyMultipleFields()
    {
        var strength = new RpgStat(10f);

        // Read current values
        strength.TryGetField(RpgStatField.Base, out var currentBase);
        strength.TryGetField(RpgStatField.ModAdd, out var currentModAdd);

        // Increase both by 5
        strength.TrySetField(RpgStatField.Base, currentBase + 5f);
        strength.TrySetField(RpgStatField.ModAdd, currentModAdd + 5f);

        // (15 + 5) * 1.0 = 20
        Assert.Equal(20f, strength.GetValue());
    }

    [Fact]
    public void Example_ClampingAfterMaxReduction()
    {
        // Character at full health
        var health = new RpgStat(200f, 0f, 200f);
        health.TrySetField(RpgStatField.Base, 200f); // Full HP
        Assert.Equal(200f, health.GetValue());

        // Apply debuff that reduces max health
        health.TrySetField(RpgStatField.Max, 100f);

        // Current health is automatically clamped to new max
        Assert.Equal(100f, health.GetValue());
    }

    [Fact]
    public void Example_PercentageHealthCalculation()
    {
        var health = new RpgStat(75f, 0f, 100f);

        // Calculate percentage
        health.TryGetField(RpgStatField.Base, out var current);
        health.TryGetField(RpgStatField.Max, out var max);

        var percentage = current / max * 100f;
        Assert.Equal(75f, percentage);
    }

    [Fact]
    public void Example_LevelUpSystem()
    {
        // Starting stats at level 1
        var health = new RpgStat(100f, 0f, 100f);
        var strength = new RpgStat(10f);

        // Level up: Increase base stats
        for (var level = 2; level <= 5; level++)
        {
            // +20 max health per level
            health.TryGetField(RpgStatField.Max, out var currentMax);
            health.TrySetField(RpgStatField.Max, currentMax + 20f);

            // +2 strength per level
            strength.TryGetField(RpgStatField.Base, out var currentStr);
            strength.TrySetField(RpgStatField.Base, currentStr + 2f);
        }

        // At level 5:
        Assert.Equal(180f, health.Max); // 100 + (4 * 20)
        Assert.Equal(18f, strength.Base); // 10 + (4 * 2)
    }

    [Fact]
    public void Example_MinimumDamageThreshold()
    {
        // Armor stat that can't go below 0
        var armor = new RpgStat(50f, 0f, 999f);

        // Apply armor debuff
        armor.TrySetField(RpgStatField.Base, -10f);

        // Value is clamped to min (0)
        Assert.Equal(0f, armor.GetValue());
    }

    [Fact]
    public void Example_ResistanceSystem()
    {
        // Resistance: -1.0 (take double damage) to 1.0 (immune)
        var fireResist = new RpgStat(0.5f, -1f, 1f); // 50% resist

        // Apply resistance debuff
        fireResist.TrySetField(RpgStatField.ModAdd, -0.7f);

        // (0.5 + (-0.7)) * 1.0 = -0.2 = take 20% extra damage
        Assert.Equal(-0.2f, fireResist.GetValue(), 0.001f); // Use tolerance for float comparison
    }

    [Fact]
    public void Example_EquipmentSystemWithFieldSelection()
    {
        var damage = new RpgStat(10f);

        // Equip weapon: +5 base damage
        damage.TryGetField(RpgStatField.Base, out var baseVal);
        var weaponDamage = 5f;
        damage.TrySetField(RpgStatField.Base, baseVal + weaponDamage);

        // Equip ring: +20% damage
        damage.TryGetField(RpgStatField.ModMult, out var multVal);
        var ringBonus = 0.2f;
        damage.TrySetField(RpgStatField.ModMult, multVal + ringBonus);

        // (15 + 0) * 1.2 = 18
        Assert.Equal(18f, damage.GetValue());

        // Unequip weapon
        damage.TrySetField(RpgStatField.Base, baseVal);

        // (10 + 0) * 1.2 = 12
        Assert.Equal(12f, damage.GetValue());
    }
}