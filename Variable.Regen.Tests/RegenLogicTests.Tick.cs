using Variable.Bounded;

namespace Variable.Regen.Tests;

public class RegenLogicTests_Tick
{
    [Fact]
    public void Tick_Regenerates()
    {
        var current = 50f;
        RegenLogic.Tick(ref current, 100f, 10f, 1f);
        Assert.Equal(60f, current);
    }

    [Fact]
    public void Tick_ClampsToMax()
    {
        var current = 95f;
        RegenLogic.Tick(ref current, 100f, 10f, 1f);
        Assert.Equal(100f, current);
    }

    [Fact]
    public void Tick_Decays()
    {
        var current = 50f;
        RegenLogic.Tick(ref current, 100f, -10f, 1f);
        Assert.Equal(40f, current);
    }

    [Fact]
    public void Tick_ClampsToZero()
    {
        var current = 5f;
        RegenLogic.Tick(ref current, 100f, -10f, 1f);
        Assert.Equal(0f, current);
    }

    [Fact]
    public void Tick_BoundedFloat_Regenerates()
    {
        var bounded = new BoundedFloat(100f, 50f);
        // Use extension method which properly decomposes the struct
        bounded.Tick(10f, 1f);
        Assert.Equal(60f, bounded.Current);
    }
}