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
    public void GetValue_UsesCacheWhenClean()
    {
        var attr = new RpgStat(10f);
        attr.GetValue(); // Calculate once

        // Manually corrupt the modifiers without marking dirty
        attr.ModAdd = 999f;

        // Should return cached value
        var val = attr.GetValue();
        Assert.Equal(10f, val); // Cache still has old value
    }
}