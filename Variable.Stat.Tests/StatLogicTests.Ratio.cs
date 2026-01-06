namespace Variable.Stat.Tests;

public class StatLogicTests_Ratio
{
    [Fact]
    public void GetRatio_CalculatesCorrectly()
    {
        StatLogic.GetRatio(50f, 100f, out var result);
        Assert.Equal(0.5f, result);
    }

    [Fact]
    public void GetRatio_ClampsToZero()
    {
        StatLogic.GetRatio(-10f, 100f, out var result);
        Assert.Equal(0f, result);
    }

    [Fact]
    public void GetRatio_ClampsToOne()
    {
        StatLogic.GetRatio(150f, 100f, out var result);
        Assert.Equal(1f, result);
    }

    [Fact]
    public void GetPercent_CalculatesCorrectly()
    {
        StatLogic.GetPercent(50f, 100f, out var result);
        Assert.Equal(50f, result);
    }
}