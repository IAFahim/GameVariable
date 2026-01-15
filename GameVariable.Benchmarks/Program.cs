using BenchmarkDotNet.Running;
using GameVariable.Benchmarks;

// Run all benchmarks with console logger
BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, BenchmarkConfig.Default);

// Or run specific benchmark types:
// BenchmarkRunner.Run<CoreMathBenchmarks>(BenchmarkConfig.Default);
// BenchmarkRunner.Run<BoundedFloatBenchmarks>(BenchmarkConfig.Default);
