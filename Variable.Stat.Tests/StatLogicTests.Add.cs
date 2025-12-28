using Xunit;
using Variable.Stat;

namespace Variable.Stat.Tests;

public class StatLogicTests_Add
{
    [Fact]
    public void Add_IncreasesValue()
    {
        float current = 10f;
        float added = StatLogic.Add(ref current, 5f, 20f);
        Assert.Equal(15f, current);
        Assert.Equal(5f, added);
    }

    [Fact]
    public void Add_ClampsToMax()
    {
        float current = 18f;
        float added = StatLogic.Add(ref current, 5f, 20f);
        Assert.Equal(20f, current);
        Assert.Equal(2f, added);
    }

    [Fact]
    public void Add_Zero_DoesNothing()
    {
        float current = 10f;
        float added = StatLogic.Add(ref current, 0f, 20f);
        Assert.Equal(10f, current);
        Assert.Equal(0f, added);
    }
}
