using Xunit;
using Variable.Regen;

namespace Variable.Regen.Tests;

public class RegenFloatTests
{
    [Fact]
    public void Tick_UpdatesValue()
    {
        var regen = new RegenFloat(100f, 50f, 10f);
        regen.Tick(1f);
        Assert.Equal(60f, regen.Value.Current);
    }

    [Fact]
    public void Tick_DecaysValue()
    {
        var regen = new RegenFloat(100f, 50f, -10f);
        regen.Tick(1f);
        Assert.Equal(40f, regen.Value.Current);
    }
}
