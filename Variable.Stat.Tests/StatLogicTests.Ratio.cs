using Xunit;
using Variable.Stat;

namespace Variable.Stat.Tests;

public class StatLogicTests_Ratio
{
    [Fact]
    public void GetRatio_CalculatesCorrectly()
    {
        Assert.Equal(0.5f, StatLogic.GetRatio(50f, 100f));
    }

    [Fact]
    public void GetRatio_ClampsToZero()
    {
        Assert.Equal(0f, StatLogic.GetRatio(-10f, 100f));
    }

    [Fact]
    public void GetRatio_ClampsToOne()
    {
        Assert.Equal(1f, StatLogic.GetRatio(150f, 100f));
    }

    [Fact]
    public void GetPercent_CalculatesCorrectly()
    {
        Assert.Equal(50f, StatLogic.GetPercent(50f, 100f));
    }
}
