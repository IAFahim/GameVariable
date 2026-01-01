namespace Variable.RPG.Tests;

public class AttributeLogicTests
{
    [Fact]
    public void Calculate_DiamondPattern_Works()
    {
        // 10 Base + 5 Flat = 15
        // 15 * 2.0 Mult (1.0 + 1.0) = 30

        var attr = new Attribute(10f);
        AttributeLogic.AddModifier(ref attr, 5f, 1.0f); // +5 Flat, +100%

        var val = AttributeLogic.GetValue(ref attr);

        Assert.Equal(30f, val);
        Assert.Equal(0, attr.IsDirty);
    }

    [Fact]
    public void Calculate_ClampsMinMax()
    {
        var attr = new Attribute(100f, 0f, 50f); // Max 50
        var val = AttributeLogic.GetValue(ref attr);
        Assert.Equal(50f, val);
    }

    [Fact]
    public void Calculate_ClampsToMin()
    {
        var attr = new Attribute(10f, 20f, 100f); // Min 20, but base is 10
        var val = AttributeLogic.GetValue(ref attr);
        Assert.Equal(20f, val); // Should clamp to min
    }

    [Fact]
    public void AddModifier_MarksDirty()
    {
        var attr = new Attribute(10f);
        AttributeLogic.GetValue(ref attr); // Clean it
        Assert.Equal(0, attr.IsDirty);

        AttributeLogic.AddModifier(ref attr, 5f, 0f);
        Assert.Equal(1, attr.IsDirty);
    }

    [Fact]
    public void ClearModifiers_ResetsToBase()
    {
        var attr = new Attribute(10f);
        AttributeLogic.AddModifier(ref attr, 20f, 2.0f);

        var valBefore = AttributeLogic.GetValue(ref attr);
        Assert.Equal(90f, valBefore); // (10+20)*3.0

        AttributeLogic.ClearModifiers(ref attr);
        var valAfter = AttributeLogic.GetValue(ref attr);
        Assert.Equal(10f, valAfter); // Back to base
    }

    [Fact]
    public void GetValue_RecalculatesWhenDirty()
    {
        var attr = new Attribute(10f);
        attr.ModAdd = 5f;
        attr.IsDirty = 1;

        var val = AttributeLogic.GetValue(ref attr);
        Assert.Equal(15f, val);
        Assert.Equal(0, attr.IsDirty);
    }

    [Fact]
    public void GetValue_UsesCacheWhenClean()
    {
        var attr = new Attribute(10f);
        AttributeLogic.GetValue(ref attr); // Calculate once

        // Manually corrupt the modifiers without marking dirty
        attr.ModAdd = 999f;

        // Should return cached value
        var val = AttributeLogic.GetValue(ref attr);
        Assert.Equal(10f, val); // Cache still has old value
    }
}
