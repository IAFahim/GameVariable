using Xunit;
using Variable.Stat;

namespace Variable.Stat.Tests;

public class StatLogicTests_Set
{
    [Fact]
    public void Set_UpdatesValue()
    {
        float current = 10f;
        StatLogic.Set(ref current, 15f, 20f);
        Assert.Equal(15f, current);
    }

    [Fact]
    public void Set_ClampsToMax()
    {
        float current = 10f;
        StatLogic.Set(ref current, 25f, 20f);
        Assert.Equal(20f, current);
    }

    [Fact]
    public void Set_ClampsToMin()
    {
        float current = 10f;
        StatLogic.Set(ref current, -5f, 20f);
        Assert.Equal(0f, current);
    }
}
