using Variable.Bounded;

namespace Variable.Reservoir.Tests;

public class ReservoirLogicTests_Int
{
    [Fact]
    public void Refill_FillsVolume_FromReserve()
    {
        var current = 5;
        var reserve = 20;
        ReservoirLogic.Refill(ref current, 10, ref reserve, out var refilled);

        Assert.Equal(10, current);
        Assert.Equal(15, reserve);
        Assert.Equal(5, refilled);
    }

    [Fact]
    public void Refill_PartiallyFills_WhenReserveLow()
    {
        var current = 5;
        var reserve = 3;
        ReservoirLogic.Refill(ref current, 10, ref reserve, out var refilled);

        Assert.Equal(8, current);
        Assert.Equal(0, reserve);
        Assert.Equal(3, refilled);
    }

    [Fact]
    public void Refill_DoesNothing_WhenFull()
    {
        var current = 10;
        var reserve = 20;
        ReservoirLogic.Refill(ref current, 10, ref reserve, out var refilled);

        Assert.Equal(10, current);
        Assert.Equal(20, reserve);
        Assert.Equal(0, refilled);
    }

    [Fact]
    public void Refill_BoundedInt_Works()
    {
        var volume = new BoundedInt(10, 5);
        var reserve = 20;
        ReservoirLogic.Refill(ref volume.Current, volume.Max, ref reserve, out var refilled);

        Assert.Equal(10, volume.Current);
        Assert.Equal(15, reserve);
        Assert.Equal(5, refilled);
    }
}