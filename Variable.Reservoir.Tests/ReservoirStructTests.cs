using Xunit;
using Variable.Reservoir;

namespace Variable.Reservoir.Tests;

public class ReservoirStructTests
{
    [Fact]
    public void ReservoirInt_Refill_Works()
    {
        var res = new ReservoirInt(10, 5, 20);
        int refilled = res.Refill();

        Assert.Equal(10, res.Volume.Current);
        Assert.Equal(15, res.Reserve);
        Assert.Equal(5, refilled);
    }

    [Fact]
    public void ReservoirFloat_Refill_Works()
    {
        var res = new ReservoirFloat(10f, 5f, 20f);
        float refilled = res.Refill();

        Assert.Equal(10f, res.Volume.Current);
        Assert.Equal(15f, res.Reserve);
        Assert.Equal(5f, refilled);
    }
}
