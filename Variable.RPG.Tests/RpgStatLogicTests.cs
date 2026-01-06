namespace Variable.RPG.Tests;

public class RpgStatLogicTests
{
    [Fact]
    public void Calculate_DiamondPattern_Works()
    {
        // 10 Base + 5 Flat = 15
        // 15 * 2.0 Mult (1.0 + 1.0) = 30

        var attr = new RpgStat(10f);
        attr.AddModifier(5f, 1.0f); // +5 Flat, +100%

        var val = attr.GetValue();

        Assert.Equal(30f, val);
    }

    [Fact]
    public void Calculate_ClampsMinMax()
    {
        var attr = new RpgStat(100f, 0f, 50f); // Max 50
        var val = attr.GetValue();
        Assert.Equal(50f, val);
    }

    [Fact]
    public void Calculate_ClampsToMin()
    {
        var attr = new RpgStat(10f, 20f, 100f); // Min 20, but base is 10
        var val = attr.GetValue();
        Assert.Equal(20f, val); // Should clamp to min
    }

    [Fact]
    public void ClearModifiers_ResetsToBase()
    {
        var attr = new RpgStat(10f);
        attr.AddModifier(20f, 2.0f);

        var valBefore = attr.GetValue();
        Assert.Equal(90f, valBefore); // (10+20)*3.0

        attr.ClearModifiers();
        var valAfter = attr.GetValue();
        Assert.Equal(10f, valAfter); // Back to base
    }

    [Fact]
    public void GetValue_AlwaysRecalculates()
    {
        var attr = new RpgStat(10f);
        var val1 = attr.GetValue(); // First calculation
        Assert.Equal(10f, val1);

        // Modify the modifiers directly
        attr.ModAdd = 999f;

        // GetValue always recalculates from primitives
        var val2 = attr.GetValue();
        Assert.Equal(1009f, val2); // (10 + 999) * 1.0
    }

    [Fact]
    public void TrySetField_Max_Works()
    {
        var stat = new RpgStat(100f, 0f, 200f);

        // Set max to 150
        var success = stat.TrySetField(RpgStatField.Max, 150f);

        Assert.True(success);
        Assert.Equal(150f, stat.Max);
        // Value should be recalculated and clamped to new max
        Assert.Equal(100f, stat.GetValue());
    }

    [Fact]
    public void TrySetField_Min_Works()
    {
        var stat = new RpgStat(50f, 0f, 200f);

        // Set min to 60
        var success = stat.TrySetField(RpgStatField.Min, 60f);

        Assert.True(success);
        Assert.Equal(60f, stat.Min);
        // Value should be recalculated and clamped to new min
        Assert.Equal(60f, stat.GetValue());
    }

    [Fact]
    public void TrySetField_Base_Works()
    {
        var stat = new RpgStat(10f);

        var success = stat.TrySetField(RpgStatField.Base, 50f);

        Assert.True(success);
        Assert.Equal(50f, stat.Base);
        Assert.Equal(50f, stat.GetValue());
    }

    [Fact]
    public void TrySetField_ModAdd_Works()
    {
        var stat = new RpgStat(10f);

        var success = stat.TrySetField(RpgStatField.ModAdd, 5f);

        Assert.True(success);
        Assert.Equal(5f, stat.ModAdd);
        Assert.Equal(15f, stat.GetValue()); // (10 + 5) * 1.0
    }

    [Fact]
    public void TrySetField_ModMult_Works()
    {
        var stat = new RpgStat(10f);

        var success = stat.TrySetField(RpgStatField.ModMult, 2.0f);

        Assert.True(success);
        Assert.Equal(2.0f, stat.ModMult);
        Assert.Equal(20f, stat.GetValue()); // (10 + 0) * 2.0
    }

    [Fact]
    public void TrySetField_Value_NoAutoRecalculate()
    {
        var stat = new RpgStat(10f);
        stat.AddModifier(5f, 0f); // Real value should be 15

        // Force set value to 999 (bypassing calculation)
        var success = stat.TrySetField(RpgStatField.Value, 999f, false);

        Assert.True(success);
        Assert.Equal(999f, stat.Value);
    }

    [Fact]
    public void TrySetField_InvalidField_ReturnsFalse()
    {
        var stat = new RpgStat(10f);

        var success = stat.TrySetField(RpgStatField.None, 50f);

        Assert.False(success);
    }

    [Fact]
    public void TryGetField_Base_Works()
    {
        var stat = new RpgStat(42f);

        var success = stat.TryGetField(RpgStatField.Base, out var value);

        Assert.True(success);
        Assert.Equal(42f, value);
    }

    [Fact]
    public void TryGetField_Max_Works()
    {
        var stat = new RpgStat(10f, 0f, 500f);

        var success = stat.TryGetField(RpgStatField.Max, out var value);

        Assert.True(success);
        Assert.Equal(500f, value);
    }

    [Fact]
    public void TryGetField_InvalidField_ReturnsFalse()
    {
        var stat = new RpgStat(10f);

        var success = stat.TryGetField(RpgStatField.None, out var value);

        Assert.False(success);
        Assert.Equal(0f, value);
    }

    [Fact]
    public void TrySetField_MaxClampingScenario()
    {
        // Start with a stat that has high value
        var stat = new RpgStat(100f);
        stat.AddModifier(50f, 0.5f); // (100 + 50) * 1.5 = 225

        Assert.Equal(225f, stat.GetValue());

        // Now reduce the max to 150
        stat.TrySetField(RpgStatField.Max, 150f);

        // Value should be clamped to new max
        Assert.Equal(150f, stat.GetValue());
    }

    [Fact]
    public void TrySetField_CombinedModifications()
    {
        var stat = new RpgStat(10f);

        // Set multiple fields
        stat.TrySetField(RpgStatField.Base, 50f);
        stat.TrySetField(RpgStatField.ModAdd, 10f);
        stat.TrySetField(RpgStatField.ModMult, 2f);
        stat.TrySetField(RpgStatField.Max, 100f);

        // (50 + 10) * 2 = 120, clamped to 100
        Assert.Equal(100f, stat.GetValue());
    }
}