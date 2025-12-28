using System.Runtime.CompilerServices;

namespace Variable.Core.Tests;

public class IBoundedInfoTests
{
    #region Polymorphism Tests

    [Fact]
    public void PolymorphicUsage_WorksWithDifferentImplementations()
    {
        var implementations = new IBoundedInfo[]
        {
            new TestBounded(0f, 100f, 50f),
            new TestBounded(-50f, 50f, 0f),
            new TestBounded(0f, 1f, 0.5f)
        };

        foreach (var impl in implementations)
        {
            // All should have valid ratio between 0 and 1
            var ratio = impl.GetRatio();
            Assert.InRange(ratio, 0.0, 1.0);
        }
    }

    #endregion

    /// <summary>
    ///     A test implementation of IBoundedInfo for testing the interface contract.
    /// </summary>
    private readonly struct TestBounded : IBoundedInfo
    {
        public readonly float Current;
        public readonly float Min;
        public readonly float Max;
        
        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetMin() => 0;

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetCurrent() => Current;

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetMax() => Max;


        public TestBounded(float min, float max, float current)
        {
            Min = min;
            Max = max;
            Current = Math.Clamp(current, min, max);
        }

        public bool IsFull()
        {
            return Math.Abs(Current - Max) < float.Epsilon;
        }

        public bool IsEmpty()
        {
            return Math.Abs(Current - Min) < float.Epsilon;
        }

        public double GetRatio()
        {
            return Math.Abs(Max - Min) < float.Epsilon ? 0.0 : (Current - Min) / (Max - Min);
        }
    }

    #region Interface Contract Tests

    [Fact]
    public void IsFull_ContractHonored_WhenAtMax()
    {
        IBoundedInfo bounded = new TestBounded(0f, 100f, 100f);
        Assert.True(bounded.IsFull());
    }

    [Fact]
    public void IsFull_ContractHonored_WhenNotAtMax()
    {
        IBoundedInfo bounded = new TestBounded(0f, 100f, 50f);
        Assert.False(bounded.IsFull());
    }

    [Fact]
    public void IsEmpty_ContractHonored_WhenAtMin()
    {
        IBoundedInfo bounded = new TestBounded(0f, 100f, 0f);
        Assert.True(bounded.IsEmpty());
    }

    [Fact]
    public void IsEmpty_ContractHonored_WhenNotAtMin()
    {
        IBoundedInfo bounded = new TestBounded(0f, 100f, 50f);
        Assert.False(bounded.IsEmpty());
    }

    [Fact]
    public void GetRatio_ReturnsNormalizedValue()
    {
        IBoundedInfo bounded = new TestBounded(0f, 100f, 50f);
        Assert.Equal(0.5, bounded.GetRatio(), 5);
    }

    [Fact]
    public void GetRatio_ReturnsZero_WhenRangeIsZero()
    {
        IBoundedInfo bounded = new TestBounded(50f, 50f, 50f);
        Assert.Equal(0.0, bounded.GetRatio());
    }

    [Fact]
    public void GetRatio_WorksWithNegativeMin()
    {
        IBoundedInfo bounded = new TestBounded(-50f, 50f, 0f);
        Assert.Equal(0.5, bounded.GetRatio(), 5);
    }

    #endregion
}