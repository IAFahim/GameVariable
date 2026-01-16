#!/usr/bin/env python3
import json
import sys
import os

def load_benchmarks(filepath):
    try:
        with open(filepath, 'r') as f:
            data = json.load(f)

        benchmarks = {}
        # BenchmarkDotNet JSON structure usually has a 'Benchmarks' list
        if 'Benchmarks' in data:
            for b in data['Benchmarks']:
                # Key by DisplayInfo or FullName
                key = b.get('DisplayInfo', b.get('FullName', 'Unknown'))
                benchmarks[key] = b
        return benchmarks
    except Exception as e:
        print(f"Error loading {filepath}: {e}")
        return {}

def format_time(ns):
    if ns < 1000:
        return f"{ns:.2f} ns"
    elif ns < 1_000_000:
        return f"{ns/1000:.2f} us"
    elif ns < 1_000_000_000:
        return f"{ns/1_000_000:.2f} ms"
    else:
        return f"{ns/1_000_000_000:.2f} s"

def shorten_name(name):
    # Attempt to clean up the name for the table
    # Example: "Variable.Core.Tests.Benchmarks.CoreMathBenchmarks.IsPowerOfTwo(n: 10)"
    # -> "CoreMathBenchmarks.IsPowerOfTwo(n: 10)"
    parts = name.split('.')
    if len(parts) > 2:
        return parts[-1]
    return name

def compare(old_file, new_file, output_file):
    old_data = load_benchmarks(old_file)
    new_data = load_benchmarks(new_file)

    if not old_data and not new_data:
        print("No benchmark data found in either file.")
        return

    lines = []
    lines.append(f"### Comparison: {os.path.basename(new_file)}")
    lines.append("")
    lines.append("| Benchmark | Old | New | Diff | Status |")
    lines.append("|---|---|---|---|---|")

    all_keys = set(old_data.keys()) | set(new_data.keys())
    sorted_keys = sorted(list(all_keys))

    for key in sorted_keys:
        old_b = old_data.get(key)
        new_b = new_data.get(key)

        display_name = shorten_name(key)

        old_mean = old_b['Statistics']['Mean'] if old_b and 'Statistics' in old_b else None
        new_mean = new_b['Statistics']['Mean'] if new_b and 'Statistics' in new_b else None

        old_str = format_time(old_mean) if old_mean is not None else "-"
        new_str = format_time(new_mean) if new_mean is not None else "-"

        diff_str = ""
        status = ""

        if old_mean and new_mean:
            if old_mean == 0:
                 diff_str = "0%"
                 status = "⚪ Same"
            else:
                diff_ns = new_mean - old_mean
                diff_percent = (diff_ns / old_mean) * 100
                diff_str = f"{diff_percent:+.1f}%"

                if diff_percent > 5.0:
                    status = "🔴 Slower"
                elif diff_percent < -5.0:
                    status = "🟢 Faster"
                else:
                    status = "⚪ Same"
        elif old_mean and not new_mean:
            status = "❌ Removed"
        elif not old_mean and new_mean:
            status = "✨ New"

        lines.append(f"| {display_name} | {old_str} | {new_str} | {diff_str} | {status} |")

    lines.append("")

    with open(output_file, 'w') as f:
        f.write("\n".join(lines))

    print(f"Comparison report generated: {output_file}")

if __name__ == "__main__":
    if len(sys.argv) != 4:
        print("Usage: compare_benchmarks.py <old_json> <new_json> <output_md>")
        sys.exit(1)

    compare(sys.argv[1], sys.argv[2], sys.argv[3])
