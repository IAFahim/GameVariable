import json
import sys
import os

def load_json(filepath):
    try:
        with open(filepath, 'r') as f:
            return json.load(f)
    except Exception as e:
        print(f"Error loading {filepath}: {e}", file=sys.stderr)
        return None

def get_benchmark_key(bm):
    # Create a unique key for the benchmark based on its name and parameters
    return bm.get('FullName', bm.get('DisplayInfo', 'Unknown'))

def format_time(ns):
    if ns < 1000:
        return f"{ns:.2f} ns"
    elif ns < 1000000:
        return f"{ns/1000:.2f} us"
    else:
        return f"{ns/1000000:.2f} ms"

def calculate_diff(current, baseline):
    if baseline == 0:
        return "N/A"
    diff = (current - baseline) / baseline * 100
    return diff

def main():
    if len(sys.argv) < 3:
        print("Usage: python3 compare_benchmarks.py <baseline_json> <current_json>")
        sys.exit(1)

    baseline_path = sys.argv[1]
    current_path = sys.argv[2]

    baseline_data = load_json(baseline_path)
    current_data = load_json(current_path)

    if not baseline_data or not current_data:
        print("Failed to load benchmark data.")
        sys.exit(1)

    baseline_map = {get_benchmark_key(bm): bm for bm in baseline_data.get('Benchmarks', [])}
    current_benchmarks = current_data.get('Benchmarks', [])

    print("# Benchmark Comparison Report\n")
    print(f"**Baseline:** {os.path.basename(baseline_path)}")
    print(f"**Current:** {os.path.basename(current_path)}\n")

    print("| Benchmark | Mean (Current) | Mean (Baseline) | Diff % | Alloc (Current) | Alloc (Baseline) |")
    print("|---|---|---|---|---|---|")

    for current_bm in current_benchmarks:
        key = get_benchmark_key(current_bm)
        baseline_bm = baseline_map.get(key)

        current_mean = current_bm.get('Statistics', {}).get('Mean', 0)
        current_alloc = current_bm.get('Memory', {}).get('BytesAllocatedPerOperation', 0)

        name = current_bm.get('Method', key)
        # Simplify name if it's too long, maybe strip namespace
        if '.' in name and '(' in name:
             # Try to keep just the method and params
             pass

        # Display simplified name
        display_name = key.split('.')[-1]

        if baseline_bm:
            baseline_mean = baseline_bm.get('Statistics', {}).get('Mean', 0)
            baseline_alloc = baseline_bm.get('Memory', {}).get('BytesAllocatedPerOperation', 0)

            time_diff = calculate_diff(current_mean, baseline_mean)

            # Formatting
            time_diff_str = f"{time_diff:.2f}%" if isinstance(time_diff, float) else time_diff

            # Add emoji for regression/improvement
            if isinstance(time_diff, float):
                if time_diff > 5.0:
                    time_diff_str += " 🔴" # Regression
                elif time_diff < -5.0:
                    time_diff_str += " 🟢" # Improvement

            print(f"| {display_name} | {format_time(current_mean)} | {format_time(baseline_mean)} | {time_diff_str} | {current_alloc} B | {baseline_alloc} B |")
        else:
            print(f"| {display_name} | {format_time(current_mean)} | N/A | New | {current_alloc} B | N/A |")

if __name__ == "__main__":
    main()
