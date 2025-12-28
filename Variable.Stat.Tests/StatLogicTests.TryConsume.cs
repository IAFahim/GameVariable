namespace Variable.Stat.Tests;

public class StatLogicTests_TryConsume
{
    [Fact]
    public void TryConsume_ReturnsTrue_AndConsumes_WhenEnough()
    {
        var current = 10f;
        var success = StatLogic.TryConsume(ref current, 5f);

        Assert.True(success);
        Assert.Equal(5f, current);
    }

    [Fact]
    public void TryConsume_ReturnsFalse_AndDoesNotConsume_WhenNotEnough()
    {
        var current = 3f;
        var success = StatLogic.TryConsume(ref current, 5f);

        Assert.False(success);
        Assert.Equal(3f, current);
    }
}