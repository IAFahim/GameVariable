using Variable.Bounded;

namespace Variable.Reservoir.Tests;

public class ReservoirLogicTests_Float
{
    [Fact]
    public void Refill_FillsVolume_FromReserve()
    {
        var current = 5f;
        var reserve = 20f;
        var refilled = ReservoirLogic.Refill(ref current, 10f, ref reserve);

        Assert.Equal(10f, current);
        Assert.Equal(15f, reserve);
        Assert.Equal(5f, refilled);
    }

    [Fact]
    public void Refill_PartiallyFills_WhenReserveLow()
    {
        var current = 5f;
        var reserve = 3f;
        var refilled = ReservoirLogic.Refill(ref current, 10f, ref reserve);

        Assert.Equal(8f, current);
        Assert.Equal(0f, reserve);
        Assert.Equal(3f, refilled);
    }

    [Fact]
    public void Refill_BoundedFloat_Works()
    {
        var volume = new BoundedFloat(10f, 5f);
        var reserve = 20f;
        var refilled = ReservoirLogic.Refill(ref volume.Current, volume.Max, ref reserve);

        Assert.Equal(10f, volume.Current);
        Assert.Equal(15f, reserve);
        Assert.Equal(5f, refilled);
    }
}