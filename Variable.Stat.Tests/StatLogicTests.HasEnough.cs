namespace Variable.Stat.Tests;

public class StatLogicTests_HasEnough
{
    [Fact]
    public void HasEnough_ReturnsTrue_WhenEnough()
    {
        Assert.True(StatLogic.HasEnough(10f, 5f));
    }

    [Fact]
    public void HasEnough_ReturnsTrue_WhenExact()
    {
        Assert.True(StatLogic.HasEnough(10f, 10f));
    }

    [Fact]
    public void HasEnough_ReturnsFalse_WhenNotEnough()
    {
        Assert.False(StatLogic.HasEnough(5f, 10f));
    }
}