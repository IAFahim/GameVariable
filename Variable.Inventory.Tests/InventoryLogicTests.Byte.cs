namespace Variable.Inventory.Tests;

public partial class InventoryLogicTests
{
    [Theory]
    [InlineData(10, 10, true)]
    [InlineData(9, 10, false)]
    [InlineData(11, 10, true)]
    public void IsFull_Byte_Tests(byte current, byte max, bool expected)
    {
        Assert.Equal(expected, InventoryLogic.IsFull(current, max));
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(1, false)]
    public void IsEmpty_Byte_Tests(byte current, bool expected)
    {
        Assert.Equal(expected, InventoryLogic.IsEmpty(current));
    }

    [Theory]
    [InlineData(10, 5, true)]
    [InlineData(5, 5, true)]
    [InlineData(4, 5, false)]
    public void HasEnough_Byte_Tests(byte current, byte required, bool expected)
    {
        Assert.Equal(expected, InventoryLogic.HasEnough(current, required));
    }

    [Theory]
    [InlineData(5, 10, 5, true)]
    [InlineData(5, 10, 4, true)]
    [InlineData(5, 10, 6, false)]
    public void CanAccept_Byte_Tests(byte current, byte max, byte amountToAdd, bool expected)
    {
        Assert.Equal(expected, InventoryLogic.CanAccept(current, max, amountToAdd));
    }

    [Theory]
    [InlineData(5, 10, 5)]
    [InlineData(10, 10, 0)]
    [InlineData(11, 10, 0)]
    public void GetRemainingSpace_Byte_Tests(byte current, byte max, byte expected)
    {
        Assert.Equal(expected, InventoryLogic.GetRemainingSpace(current, max));
    }

    [Theory]
    [InlineData(5, 10, 5)]
    [InlineData(15, 10, 10)]
    public void Set_Byte_Tests(byte value, byte max, byte expected)
    {
        byte current = 0;
        InventoryLogic.Set(ref current, value, max);
        Assert.Equal(expected, current);
    }

    [Fact]
    public void Clear_Byte_Test()
    {
        byte current = 10;
        InventoryLogic.Clear(ref current);
        Assert.Equal(0, current);
    }
}