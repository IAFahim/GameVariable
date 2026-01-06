namespace Variable.Inventory.Tests;

public partial class InventoryLogicTests
{
    [Theory]
    [InlineData(10L, 10L, true)]
    [InlineData(9L, 10L, false)]
    [InlineData(11L, 10L, true)]
    public void IsFull_Long_Tests(long current, long max, bool expected)
    {
        Assert.Equal(expected, InventoryLogic.IsFull(current, max));
    }

    [Theory]
    [InlineData(0L, true)]
    [InlineData(1L, false)]
    [InlineData(-1L, true)]
    public void IsEmpty_Long_Tests(long current, bool expected)
    {
        Assert.Equal(expected, InventoryLogic.IsEmpty(current));
    }

    [Theory]
    [InlineData(10L, 5L, true)]
    [InlineData(5L, 5L, true)]
    [InlineData(4L, 5L, false)]
    public void HasEnough_Long_Tests(long current, long required, bool expected)
    {
        Assert.Equal(expected, InventoryLogic.HasEnough(current, required));
    }

    [Theory]
    [InlineData(5L, 10L, 5L, true)]
    [InlineData(5L, 10L, 4L, true)]
    [InlineData(5L, 10L, 6L, false)]
    public void CanAccept_Long_Tests(long current, long max, long amountToAdd, bool expected)
    {
        Assert.Equal(expected, InventoryLogic.CanAccept(current, max, amountToAdd));
    }

    [Theory]
    [InlineData(5L, 10L, 5L)]
    [InlineData(10L, 10L, 0L)]
    [InlineData(11L, 10L, 0L)]
    public void GetRemainingSpace_Long_Tests(long current, long max, long expected)
    {
        InventoryLogic.GetRemainingSpace(current, max, out var result);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(5L, 10L, 5L)]
    [InlineData(-5L, 10L, 0L)]
    [InlineData(15L, 10L, 10L)]
    public void Set_Long_Tests(long value, long max, long expected)
    {
        var current = 0L;
        InventoryLogic.Set(ref current, value, max);
        Assert.Equal(expected, current);
    }

    [Fact]
    public void Clear_Long_Test()
    {
        var current = 10L;
        InventoryLogic.Clear(ref current);
        Assert.Equal(0L, current);
    }
}