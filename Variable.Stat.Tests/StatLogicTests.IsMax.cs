namespace Variable.Stat.Tests;

public class StatLogicTests_IsMax
{
    [Fact]
    public void IsMax_ReturnsTrue_WhenAtMax()
    {
        Assert.True(StatLogic.IsMax(100f, 100f));
    }

    [Fact]
    public void IsMax_ReturnsTrue_WhenAboveMax()
    {
        Assert.True(StatLogic.IsMax(101f, 100f));
    }

    [Fact]
    public void IsMax_ReturnsFalse_WhenBelowMax()
    {
        Assert.False(StatLogic.IsMax(99f, 100f));
    }
}