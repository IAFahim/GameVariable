namespace Variable.Stat.Tests;

public class StatLogicTests_GetMissing
{
    [Fact]
    public void GetMissing_ReturnsDifference()
    {
        StatLogic.GetMissing(60f, 100f, out var result);
        Assert.Equal(40f, result);
    }

    [Fact]
    public void GetMissing_ReturnsZero_WhenFull()
    {
        StatLogic.GetMissing(100f, 100f, out var result);
        Assert.Equal(0f, result);
    }

    [Fact]
    public void GetMissing_ReturnsZero_WhenOverFull()
    {
        StatLogic.GetMissing(110f, 100f, out var result);
        Assert.Equal(0f, result);
    }
}