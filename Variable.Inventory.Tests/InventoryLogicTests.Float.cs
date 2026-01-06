namespace Variable.Inventory.Tests;

public partial class InventoryLogicTests
{
    [Theory]
    [InlineData(10f, 10f, true)]
    [InlineData(9.999f, 10f, true)] // Within tolerance
    [InlineData(9f, 10f, false)]
    [InlineData(11f, 10f, true)]
    public void IsFull_Tests(float current, float max, bool expected)
    {
        Assert.Equal(expected, InventoryLogic.IsFull(current, max));
    }

    [Theory]
    [InlineData(0f, true)]
    [InlineData(0.001f, true)] // Within tolerance
    [InlineData(1f, false)]
    [InlineData(-1f, true)]
    public void IsEmpty_Tests(float current, bool expected)
    {
        Assert.Equal(expected, InventoryLogic.IsEmpty(current));
    }

    [Theory]
    [InlineData(10f, 5f, true)]
    [InlineData(5f, 5f, true)]
    [InlineData(4.999f, 5f, true)] // Within tolerance
    [InlineData(4f, 5f, false)]
    public void HasEnough_Tests(float current, float required, bool expected)
    {
        Assert.Equal(expected, InventoryLogic.HasEnough(current, required));
    }

    [Theory]
    [InlineData(5f, 10f, 5f, true)]
    [InlineData(5f, 10f, 4f, true)]
    [InlineData(5f, 10f, 6f, false)]
    [InlineData(9.999f, 10f, 0.001f, true)]
    public void CanAccept_Tests(float current, float max, float amountToAdd, bool expected)
    {
        Assert.Equal(expected, InventoryLogic.CanAccept(current, max, amountToAdd));
    }

    [Theory]
    [InlineData(5f, 10f, 5f)]
    [InlineData(10f, 10f, 0f)]
    [InlineData(11f, 10f, 0f)]
    public void GetRemainingSpace_Tests(float current, float max, float expected)
    {
        InventoryLogic.GetRemainingSpace(current, max, out var result);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(5f, 10f, 50f, 100f, 10f, 5f)] // Limited by Qty (5 space) vs Weight (50 space / 10 = 5) -> 5
    [InlineData(5f, 10f, 90f, 100f, 10f, 1f)] // Limited by Weight (10 space / 10 = 1) vs Qty (5) -> 1
    [InlineData(5f, 10f, 50f, 100f, 0f, 5f)] // Zero weight unit -> Limited by Qty
    [InlineData(10f, 10f, 50f, 100f, 10f, 0f)] // Full Qty
    [InlineData(5f, 10f, 100f, 100f, 10f, 0f)] // Full Weight
    public void GetMaxAcceptable_Tests(float currentQty, float maxQty, float currentWeight, float maxWeight,
        float unitWeight, float expected)
    {
        InventoryLogic.GetMaxAcceptable(currentQty, maxQty, currentWeight, maxWeight, unitWeight, out var result);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(5f, 10f, 5f)]
    [InlineData(-5f, 10f, 0f)]
    [InlineData(15f, 10f, 10f)]
    [InlineData(0.0001f, 10f, 0f)] // Below tolerance -> 0
    [InlineData(9.9999f, 10f, 10f)] // Near max -> max
    public void Set_Tests(float value, float max, float expected)
    {
        var current = 0f;
        InventoryLogic.Set(ref current, value, max);
        Assert.Equal(expected, current);
    }

    [Fact]
    public void Clear_Test()
    {
        var current = 10f;
        InventoryLogic.Clear(ref current);
        Assert.Equal(0f, current);
    }

    [Theory]
    [InlineData(5f, 5f, 10f, true, 10f)]
    [InlineData(5f, 6f, 10f, false, 5f)]
    [InlineData(5f, 0.0001f, 10f, true, 5f)] // Small amount ignored
    public void TryAddExact_Tests(float initial, float amount, float max, bool expectedResult, float expectedCurrent)
    {
        var current = initial;
        var result = InventoryLogic.TryAddExact(ref current, amount, max);
        Assert.Equal(expectedResult, result);
        Assert.Equal(expectedCurrent, current);
    }

    [Theory]
    [InlineData(5f, 5f, 10f, 5f, 0f, 10f)]
    [InlineData(5f, 10f, 10f, 5f, 5f, 10f)] // Add 10, space 5 -> add 5, overflow 5
    [InlineData(5f, 0.0001f, 10f, 0f, 0f, 5f)] // Small amount ignored
    public void AddPartial_Tests(float initial, float amount, float max, float expectedAdded, float expectedOverflow,
        float expectedCurrent)
    {
        var current = initial;
        InventoryLogic.TryAddPartial(ref current, amount, max, out var added, out var overflow);
        Assert.Equal(expectedAdded, added);
        Assert.Equal(expectedOverflow, overflow);
        Assert.Equal(expectedCurrent, current);
    }

    [Theory]
    [InlineData(10f, 5f, true, 5f)]
    [InlineData(5f, 10f, false, 5f)]
    [InlineData(10f, 0.0001f, true, 10f)] // Small amount ignored
    public void TryRemoveExact_Tests(float initial, float amount, bool expectedResult, float expectedCurrent)
    {
        var current = initial;
        var result = InventoryLogic.TryRemoveExact(ref current, amount);
        Assert.Equal(expectedResult, result);
        Assert.Equal(expectedCurrent, current);
    }

    [Theory]
    [InlineData(10f, 5f, 5f, 5f)]
    [InlineData(5f, 10f, 5f, 0f)]
    [InlineData(10f, 0.0001f, 0f, 10f)] // Small amount ignored
    public void RemovePartial_Tests(float initial, float amount, float expectedRemoved, float expectedCurrent)
    {
        var current = initial;
        InventoryLogic.TryRemovePartial(ref current, amount, out var removed);
        Assert.Equal(expectedRemoved, removed);
        Assert.Equal(expectedCurrent, current);
    }

    [Theory]
    [InlineData(10f, 0f, 10f, 5f, true, 5f, 5f)]
    [InlineData(4f, 0f, 10f, 5f, false, 4f, 0f)] // Not enough src
    [InlineData(10f, 6f, 10f, 5f, false, 10f, 6f)] // Not enough dst space
    public void TryTransferExact_Tests(float srcInitial, float dstInitial, float dstMax, float amount,
        bool expectedResult, float expectedSrc, float expectedDst)
    {
        var src = srcInitial;
        var dst = dstInitial;
        var result = InventoryLogic.TryTransferExact(ref src, ref dst, dstMax, amount);
        Assert.Equal(expectedResult, result);
        Assert.Equal(expectedSrc, src);
        Assert.Equal(expectedDst, dst);
    }

    [Theory]
    [InlineData(10f, 0f, 0f, 100f, 10f, 10f, 5f, true, 5f, 5f, 50f)]
    [InlineData(10f, 0f, 90f, 100f, 10f, 10f, 2f, false, 10f, 0f,
        90f)] // Weight limit exceeded (2 * 10 = 20, 90+20 > 100)
    public void TryTransferExactWithWeight_Tests(
        float srcInitial, float dstInitial, float dstWeightInitial,
        float dstMaxWeight, float dstMaxQty, float unitWeight, float amount,
        bool expectedResult, float expectedSrc, float expectedDst, float expectedDstWeight)
    {
        var src = srcInitial;
        var dst = dstInitial;
        var dstWeight = dstWeightInitial;

        var result = InventoryLogic.TryTransferExactWithWeight(
            ref src, ref dst, ref dstWeight,
            dstMaxWeight, dstMaxQty, unitWeight, amount);

        Assert.Equal(expectedResult, result);
        Assert.Equal(expectedSrc, src);
        Assert.Equal(expectedDst, dst);
        Assert.Equal(expectedDstWeight, dstWeight);
    }

    [Theory]
    [InlineData(10f, 0f, 10f, 5f, 5f, 5f, 5f)]
    [InlineData(10f, 8f, 10f, 5f, 2f, 8f, 10f)] // Limited by dst space
    [InlineData(3f, 0f, 10f, 5f, 3f, 0f, 3f)] // Limited by src amount
    public void TransferPartial_Tests(float srcInitial, float dstInitial, float dstMax, float amount,
        float expectedTransferred, float expectedSrc, float expectedDst)
    {
        var src = srcInitial;
        var dst = dstInitial;
        InventoryLogic.TryTransferPartial(ref src, ref dst, dstMax, amount, out var transferred);
        Assert.Equal(expectedTransferred, transferred);
        Assert.Equal(expectedSrc, src);
        Assert.Equal(expectedDst, dst);
    }

    [Theory]
    [InlineData(10f, 0f, 0f, 100f, 10f, 10f, 5f, 5f, 5f, 5f, 50f)]
    [InlineData(10f, 0f, 90f, 100f, 10f, 10f, 5f, 1f, 9f, 1f, 100f)] // Limited by weight (10 space / 10 = 1)
    public void TransferPartialWithWeight_Tests(
        float srcInitial, float dstInitial, float dstWeightInitial,
        float dstMaxWeight, float dstMaxQty, float unitWeight, float amount,
        float expectedTransferred, float expectedSrc, float expectedDst, float expectedDstWeight)
    {
        var src = srcInitial;
        var dst = dstInitial;
        var dstWeight = dstWeightInitial;

        InventoryLogic.TryTransferPartialWithWeight(
            ref src, ref dst, ref dstWeight,
            dstMaxWeight, dstMaxQty, unitWeight, amount, out var transferred);

        Assert.Equal(expectedTransferred, transferred);
        Assert.Equal(expectedSrc, src);
        Assert.Equal(expectedDst, dst);
        Assert.Equal(expectedDstWeight, dstWeight);
    }
}