namespace Variable.Inventory.Tests;

public partial class InventoryLogicTests
{
    [Theory]
    [InlineData(10, 10, true)]
    [InlineData(9, 10, false)]
    [InlineData(11, 10, true)]
    public void IsFull_Int_Tests(int current, int max, bool expected)
    {
        Assert.Equal(expected, InventoryLogic.IsFull(current, max));
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(1, false)]
    [InlineData(-1, true)]
    public void IsEmpty_Int_Tests(int current, bool expected)
    {
        Assert.Equal(expected, InventoryLogic.IsEmpty(current));
    }

    [Theory]
    [InlineData(10, 5, true)]
    [InlineData(5, 5, true)]
    [InlineData(4, 5, false)]
    public void HasEnough_Int_Tests(int current, int required, bool expected)
    {
        Assert.Equal(expected, InventoryLogic.HasEnough(current, required));
    }

    [Theory]
    [InlineData(5, 10, 5, true)]
    [InlineData(5, 10, 4, true)]
    [InlineData(5, 10, 6, false)]
    public void CanAccept_Int_Tests(int current, int max, int amountToAdd, bool expected)
    {
        Assert.Equal(expected, InventoryLogic.CanAccept(current, max, amountToAdd));
    }

    [Theory]
    [InlineData(5, 10, 5)]
    [InlineData(10, 10, 0)]
    [InlineData(11, 10, 0)]
    public void GetRemainingSpace_Int_Tests(int current, int max, int expected)
    {
        InventoryLogic.GetRemainingSpace(current, max, out var result);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(5, 10, 5)]
    [InlineData(-5, 10, 0)]
    [InlineData(15, 10, 10)]
    public void Set_Int_Tests(int value, int max, int expected)
    {
        var current = 0;
        InventoryLogic.Set(ref current, value, max);
        Assert.Equal(expected, current);
    }

    [Fact]
    public void Clear_Int_Test()
    {
        var current = 10;
        InventoryLogic.Clear(ref current);
        Assert.Equal(0, current);
    }
}