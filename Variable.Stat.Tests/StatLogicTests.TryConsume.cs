using Xunit;
using Variable.Stat;

namespace Variable.Stat.Tests;

public class StatLogicTests_TryConsume
{
    [Fact]
    public void TryConsume_ReturnsTrue_AndConsumes_WhenEnough()
    {
        float current = 10f;
        bool success = StatLogic.TryConsume(ref current, 5f);
        
        Assert.True(success);
        Assert.Equal(5f, current);
    }

    [Fact]
    public void TryConsume_ReturnsFalse_AndDoesNotConsume_WhenNotEnough()
    {
        float current = 3f;
        bool success = StatLogic.TryConsume(ref current, 5f);
        
        Assert.False(success);
        Assert.Equal(3f, current);
    }
}
