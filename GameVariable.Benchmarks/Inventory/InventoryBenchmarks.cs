using BenchmarkDotNet.Attributes;
using Variable.Inventory;

namespace GameVariable.Benchmarks.Inventory;

[MemoryDiagnoser]
[ShortRunJob]
public class InventoryBenchmarks
{
    private const float MaxCapacity = 100f;

    [Benchmark]
    public bool TryAddPartial_Float()
    {
        float current = 50f;
        return InventoryLogic.TryAddPartial(ref current, 30f, MaxCapacity, out _, out _);
    }

    [Benchmark]
    public bool TryRemovePartial_Float()
    {
        float current = 50f;
        return InventoryLogic.TryRemovePartial(ref current, 30f, out _);
    }

    [Benchmark]
    public void Set_Float()
    {
        float current = 50f;
        InventoryLogic.Set(ref current, 75f, MaxCapacity);
    }

    [Benchmark]
    public void Clear_Float()
    {
        float current = 50f;
        InventoryLogic.Clear(ref current);
    }

    [Benchmark]
    public bool TryAddExact_Float()
    {
        float current = 50f;
        return InventoryLogic.TryAddExact(ref current, 30f, MaxCapacity);
    }

    [Benchmark]
    public bool TryRemoveExact_Float()
    {
        float current = 50f;
        return InventoryLogic.TryRemoveExact(ref current, 30f);
    }
}
