using Xunit;
using Variable.Reservoir;
using Variable.Bounded;

namespace Variable.Reservoir.Tests;

public class ReservoirLogicTests_Float
{
    [Fact]
    public void Refill_FillsVolume_FromReserve()
    {
        float current = 5f;
        float reserve = 20f;
        float refilled = ReservoirLogic.Refill(ref current, 10f, ref reserve);

        Assert.Equal(10f, current);
        Assert.Equal(15f, reserve);
        Assert.Equal(5f, refilled);
    }

    [Fact]
    public void Refill_PartiallyFills_WhenReserveLow()
    {
        float current = 5f;
        float reserve = 3f;
        float refilled = ReservoirLogic.Refill(ref current, 10f, ref reserve);

        Assert.Equal(8f, current);
        Assert.Equal(0f, reserve);
        Assert.Equal(3f, refilled);
    }

    [Fact]
    public void Refill_BoundedFloat_Works()
    {
        var volume = new BoundedFloat(10f, 5f);
        float reserve = 20f;
        float refilled = ReservoirLogic.Refill(ref volume, ref reserve);

        Assert.Equal(10f, volume.Current);
        Assert.Equal(15f, reserve);
        Assert.Equal(5f, refilled);
    }
}
