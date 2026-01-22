namespace Variable.Random.Tests;

public class RandomTests
{
    [Fact]
    public void Initialize_ProduceDeterministicResults()
    {
        var state1 = new RandomState();
        var state2 = new RandomState();
        int seed = 12345;

        RandomLogic.Initialize(ref state1, seed);
        RandomLogic.Initialize(ref state2, seed);

        RandomLogic.NextUInt(ref state1, out uint u1);
        RandomLogic.NextUInt(ref state2, out uint u2);

        Assert.Equal(u1, u2);

        RandomLogic.NextFloat(ref state1, out float f1);
        RandomLogic.NextFloat(ref state2, out float f2);

        Assert.Equal(f1, f2);
    }

    [Fact]
    public void NextInt_RespectsRange()
    {
        var state = new RandomState();
        RandomLogic.Initialize(ref state, 42);

        for (int i = 0; i < 1000; i++)
        {
            RandomLogic.NextInt(ref state, 10, 20, out int val);
            Assert.True(val >= 10);
            Assert.True(val < 20);
        }
    }

    [Fact]
    public void NextFloat_RespectsRange()
    {
        var state = new RandomState();
        RandomLogic.Initialize(ref state, 999);

        for (int i = 0; i < 1000; i++)
        {
            RandomLogic.NextFloat(ref state, out float val);
            Assert.True(val >= 0.0f);
            Assert.True(val < 1.0f);
        }
    }

    [Fact]
    public void DifferentSeeds_ProduceDifferentResults()
    {
        var state1 = new RandomState();
        var state2 = new RandomState();

        RandomLogic.Initialize(ref state1, 111);
        RandomLogic.Initialize(ref state2, 222);

        RandomLogic.NextUInt(ref state1, out uint u1);
        RandomLogic.NextUInt(ref state2, out uint u2);

        // Extremely unlikely to be equal
        Assert.NotEqual(u1, u2);
    }

    [Fact]
    public void Initialize_SetsStateNonZero()
    {
        var state = new RandomState();
        RandomLogic.Initialize(ref state, 0);

        Assert.True(state.S0 != 0 || state.S1 != 0 || state.S2 != 0 || state.S3 != 0);
    }
}
