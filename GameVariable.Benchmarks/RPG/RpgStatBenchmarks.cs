using BenchmarkDotNet.Attributes;
using Variable.RPG;

namespace GameVariable.Benchmarks.RPG;

[MemoryDiagnoser]
[ShortRunJob]
public class RpgStatBenchmarks
{
    private RpgStat _stat = new(100f);
    private RpgStat _statWithModifiers = CreateStatWithModifiers();

    private static RpgStat CreateStatWithModifiers()
    {
        var stat = new RpgStat(100f);
        stat.AddModifier(50f, 0.1f);
        stat.Recalculate();
        return stat;
    }

    [Benchmark]
    public RpgStat Construction()
    {
        return new RpgStat(100f);
    }

    [Benchmark]
    public void Recalculate()
    {
        var s = _statWithModifiers;
        s.Recalculate();
    }

    [Benchmark]
    public void AddModifier()
    {
        var s = _stat;
        s.AddModifier(10f, 0.05f);
    }

    [Benchmark]
    public void ClearModifiers()
    {
        var s = _statWithModifiers;
        s.ClearModifiers();
    }

    [Benchmark]
    public float GetValue()
    {
        return _statWithModifiers.GetValue();
    }

    [Benchmark]
    public string ToStringCompact()
    {
        var s = _statWithModifiers;
        return s.ToStringCompact();
    }

    [Benchmark]
    public string ToStringVerbose()
    {
        var s = _statWithModifiers;
        return s.ToStringVerbose();
    }

    [Benchmark]
    public bool TryGetField()
    {
        return _statWithModifiers.TryGetField(RpgStatField.Base, out _);
    }

    [Benchmark]
    public bool TrySetField()
    {
        var s = _stat;
        return s.TrySetField(RpgStatField.Base, 150f);
    }

    [Benchmark]
    [Arguments(RpgStatField.Base, RpgStatOperation.Add, 10f)]
    [Arguments(RpgStatField.ModAdd, RpgStatOperation.Add, 10f)]
    [Arguments(RpgStatField.ModMult, RpgStatOperation.AddPercent, 0.1f)]
    public bool ApplyModifier(RpgStatField field, RpgStatOperation operation, float value)
    {
        var s = _stat;
        var modifier = new RpgStatModifier(field, operation, value);
        return s.ApplyModifier(modifier);
    }

    [Benchmark]
    public void ApplyModifiers_Multiple()
    {
        var s = _stat;
        var modifiers = new RpgStatModifier[]
        {
            new(RpgStatField.Base, RpgStatOperation.Add, 10f),
            new(RpgStatField.ModAdd, RpgStatOperation.Add, 5f),
            new(RpgStatField.ModMult, RpgStatOperation.AddPercent, 0.1f)
        };
        s.ApplyModifiers(modifiers);
    }
}

[MemoryDiagnoser]
[ShortRunJob]
public class DamageLogicBenchmarks
{
    private const float Damage = 100f;
    private const float FlatMitigation = 30f;
    private const float PercentMitigation = 0.25f;

    [Benchmark]
    public void ApplyMitigation_Flat()
    {
        DamageLogic.ApplyMitigation(Damage, FlatMitigation, true, out _);
    }

    [Benchmark]
    public void ApplyMitigation_Percent()
    {
        DamageLogic.ApplyMitigation(Damage, PercentMitigation, false, out _);
    }

    [Benchmark]
    [Arguments(100f, 30f, true)]
    [Arguments(100f, 0.25f, false)]
    [Arguments(50f, 10f, true)]
    [Arguments(200f, 0.5f, false)]
    public void ApplyMitigation_Parameters(float damage, float mitigation, bool isFlat)
    {
        DamageLogic.ApplyMitigation(damage, mitigation, isFlat, out _);
    }
}
