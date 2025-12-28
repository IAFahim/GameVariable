using Xunit;
using Variable.Stat;

namespace Variable.Stat.Tests;

public class StatLogicTests_GetMissing
{
    [Fact]
    public void GetMissing_ReturnsDifference()
    {
        Assert.Equal(40f, StatLogic.GetMissing(60f, 100f));
    }

    [Fact]
    public void GetMissing_ReturnsZero_WhenFull()
    {
        Assert.Equal(0f, StatLogic.GetMissing(100f, 100f));
    }

    [Fact]
    public void GetMissing_ReturnsZero_WhenOverFull()
    {
        Assert.Equal(0f, StatLogic.GetMissing(110f, 100f));
    }
}
