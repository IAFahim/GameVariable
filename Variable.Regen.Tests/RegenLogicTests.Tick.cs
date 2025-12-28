using Xunit;
using Variable.Regen;
using Variable.Bounded;

namespace Variable.Regen.Tests;

public class RegenLogicTests_Tick
{
    [Fact]
    public void Tick_Regenerates()
    {
        float current = 50f;
        RegenLogic.Tick(ref current, 100f, 10f, 1f);
        Assert.Equal(60f, current);
    }

    [Fact]
    public void Tick_ClampsToMax()
    {
        float current = 95f;
        RegenLogic.Tick(ref current, 100f, 10f, 1f);
        Assert.Equal(100f, current);
    }

    [Fact]
    public void Tick_Decays()
    {
        float current = 50f;
        RegenLogic.Tick(ref current, 100f, -10f, 1f);
        Assert.Equal(40f, current);
    }

    [Fact]
    public void Tick_ClampsToZero()
    {
        float current = 5f;
        RegenLogic.Tick(ref current, 100f, -10f, 1f);
        Assert.Equal(0f, current);
    }

    [Fact]
    public void Tick_BoundedFloat_Regenerates()
    {
        var bounded = new BoundedFloat(100f, 50f);
        RegenLogic.Tick(ref bounded, 10f, 1f);
        Assert.Equal(60f, bounded.Current);
    }
}
