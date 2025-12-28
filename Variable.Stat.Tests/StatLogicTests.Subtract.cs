using Xunit;
using Variable.Stat;

namespace Variable.Stat.Tests;

public class StatLogicTests_Subtract
{
    [Fact]
    public void Subtract_DecreasesValue()
    {
        float current = 10f;
        float removed = StatLogic.Subtract(ref current, 5f);
        Assert.Equal(5f, current);
        Assert.Equal(5f, removed);
    }

    [Fact]
    public void Subtract_ClampsToMin()
    {
        float current = 2f;
        float removed = StatLogic.Subtract(ref current, 5f);
        Assert.Equal(0f, current);
        Assert.Equal(2f, removed);
    }

    [Fact]
    public void Subtract_WithCustomMin()
    {
        float current = 12f;
        float removed = StatLogic.Subtract(ref current, 5f, 10f);
        Assert.Equal(10f, current);
        Assert.Equal(2f, removed);
    }
}
