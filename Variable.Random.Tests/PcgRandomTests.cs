namespace Variable.Random.Tests;

public class PcgRandomTests
{
    [Fact]
    public void Determinism_SameSeed_ProducesSameSequence()
    {
        var rng1 = new PcgRandom(12345);
        var rng2 = new PcgRandom(12345);

        for (int i = 0; i < 100; i++)
        {
            Assert.Equal(rng1.Next(), rng2.Next());
        }
    }

    [Fact]
    public void Sequences_ProduceDifferentStreams()
    {
        var rng1 = new PcgRandom(12345, 1);
        var rng2 = new PcgRandom(12345, 2);

        // They might start similarly but should diverge immediately or very soon
        bool allSame = true;
        for (int i = 0; i < 100; i++)
        {
            if (rng1.Next() != rng2.Next())
            {
                allSame = false;
                break;
            }
        }
        Assert.False(allSame, "Different sequences should produce different random streams.");
    }

    [Fact]
    public void Range_Int_RespectsBounds()
    {
        var rng = new PcgRandom(42);
        for (int i = 0; i < 1000; i++)
        {
            var val = rng.Range(10, 20);
            Assert.True(val >= 10 && val < 20, $"Value {val} out of range [10, 20)");
        }
    }

    [Fact]
    public void Range_Float_RespectsBounds()
    {
        var rng = new PcgRandom(42);
        for (int i = 0; i < 1000; i++)
        {
            var val = rng.Range(10f, 20f);
            Assert.True(val >= 10f && val <= 20f, $"Value {val} out of range [10, 20]");
        }
    }

    [Fact]
    public void Chance_Works()
    {
         var rng = new PcgRandom(42);
         int heads = 0;
         int total = 10000;
         for (int i = 0; i < total; i++)
         {
             if (rng.Chance(0.5f)) heads++;
         }

         // 0.5 probability should be roughly 5000. Allow 5% margin (very loose).
         Assert.InRange(heads, 4500, 5500);
    }

    [Fact]
    public void Shuffle_PermutesList()
    {
        var rng = new PcgRandom(42);
        var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var original = new List<int>(list);

        rng.Shuffle(list);

        Assert.Equal(original.Count, list.Count);
        // It is theoretically possible to shuffle to the same order, but prob is 1/10! (1/3,628,800)
        Assert.NotEqual(original, list);

        foreach (var item in original)
        {
             Assert.Contains(item, list);
        }
    }

    [Fact]
    public void PickRandom_ReturnsElementFromList()
    {
        var rng = new PcgRandom(42);
        var list = new List<string> { "A", "B", "C" };

        for (int i = 0; i < 100; i++)
        {
            var item = rng.PickRandom(list);
            Assert.Contains(item, list);
        }
    }
}
