namespace Variable.Inventory.Tests;

public partial class InventoryLogicTests
{
    [Theory]
    [InlineData(10d, 10d, true)]
    [InlineData(9.999d, 10d, true)] // Within tolerance
    [InlineData(9d, 10d, false)]
    [InlineData(11d, 10d, true)]
    public void IsFull_Double_Tests(double current, double max, bool expected)
    {
        Assert.Equal(expected, InventoryLogic.IsFull(current, max));
    }

    [Theory]
    [InlineData(0d, true)]
    [InlineData(0.001d, true)] // Within tolerance
    [InlineData(1d, false)]
    [InlineData(-1d, true)]
    public void IsEmpty_Double_Tests(double current, bool expected)
    {
        Assert.Equal(expected, InventoryLogic.IsEmpty(current));
    }

    [Theory]
    [InlineData(10d, 5d, true)]
    [InlineData(5d, 5d, true)]
    [InlineData(4.999d, 5d, true)] // Within tolerance
    [InlineData(4d, 5d, false)]
    public void HasEnough_Double_Tests(double current, double required, bool expected)
    {
        Assert.Equal(expected, InventoryLogic.HasEnough(current, required));
    }

    [Theory]
    [InlineData(5d, 10d, 5d, true)]
    [InlineData(5d, 10d, 4d, true)]
    [InlineData(5d, 10d, 6d, false)]
    [InlineData(9.999d, 10d, 0.001d, true)]
    public void CanAccept_Double_Tests(double current, double max, double amountToAdd, bool expected)
    {
        Assert.Equal(expected, InventoryLogic.CanAccept(current, max, amountToAdd));
    }

    [Theory]
    [InlineData(5d, 10d, 5d)]
    [InlineData(10d, 10d, 0d)]
    [InlineData(11d, 10d, 0d)]
    public void GetRemainingSpace_Double_Tests(double current, double max, double expected)
    {
        Assert.Equal(expected, InventoryLogic.GetRemainingSpace(current, max));
    }

    [Theory]
    [InlineData(5d, 10d, 5d)]
    [InlineData(-5d, 10d, 0d)]
    [InlineData(15d, 10d, 10d)]
    [InlineData(0.0001d, 10d, 0d)] // Below tolerance -> 0
    [InlineData(9.9999d, 10d, 10d)] // Near max -> max
    public void Set_Double_Tests(double value, double max, double expected)
    {
        var current = 0d;
        InventoryLogic.Set(ref current, value, max);
        Assert.Equal(expected, current);
    }

    [Fact]
    public void Clear_Double_Test()
    {
        var current = 10d;
        InventoryLogic.Clear(ref current);
        Assert.Equal(0d, current);
    }
}