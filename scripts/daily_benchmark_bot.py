import os
import subprocess
import sys

SCRIPT_DIR = os.path.dirname(os.path.abspath(__file__))
REPO_ROOT = os.path.dirname(SCRIPT_DIR)
LAST_RUN_FILE = os.path.join(SCRIPT_DIR, "last_run_commit")
BENCHMARK_SCRIPT = os.path.join(SCRIPT_DIR, "run-daily-benchmarks.sh")

def run_command(command, cwd=None):
    try:
        result = subprocess.run(
            command,
            cwd=cwd,
            check=True,
            stdout=subprocess.PIPE,
            stderr=subprocess.PIPE,
            text=True
        )
        return result.stdout.strip()
    except subprocess.CalledProcessError as e:
        print(f"Error running command: {' '.join(command)}")
        print(f"Stdout: {e.stdout}")
        print(f"Stderr: {e.stderr}")
        raise

def get_current_commit():
    return run_command(["git", "rev-parse", "HEAD"], cwd=REPO_ROOT)

def get_last_run_commit():
    if os.path.exists(LAST_RUN_FILE):
        with open(LAST_RUN_FILE, "r") as f:
            return f.read().strip()
    return None

def check_for_changes(last_commit, current_commit):
    # Check for changes between last_commit and current_commit
    # excluding the scripts/ directory
    cmd = [
        "git", "diff", "--name-only",
        last_commit, current_commit,
        "--", ".", ":!scripts/"
    ]
    try:
        output = run_command(cmd, cwd=REPO_ROOT)
        return len(output) > 0
    except subprocess.CalledProcessError:
        # If the commit doesn't exist (e.g. forced push or shallow clone issue),
        # assume changes.
        print(f"Warning: Could not diff against {last_commit}. Assuming changes.")
        return True

def run_benchmarks(args=None):
    print("Running benchmarks...")
    # Ensure the script is executable
    subprocess.run(["chmod", "+x", BENCHMARK_SCRIPT], check=True)

    # Prepare command
    cmd = [BENCHMARK_SCRIPT]
    if args:
        cmd.extend(args)
    else:
        cmd.append("*")

    # Use --job short to avoid timeouts in CI/Sandbox environments
    cmd.extend(["--job", "short"])

    # Run the bash script
    proc = subprocess.run(cmd, cwd=SCRIPT_DIR)

    if proc.returncode != 0:
        print("Benchmark run failed!")
        sys.exit(1)

    print("Benchmarks completed successfully.")

def update_last_run_commit(commit_hash):
    with open(LAST_RUN_FILE, "w") as f:
        f.write(commit_hash)
    print(f"Updated {LAST_RUN_FILE} to {commit_hash}")

def main():
    print("--- Daily Benchmark Bot ---")

    current_commit = get_current_commit()
    print(f"Current commit: {current_commit}")

    last_commit = get_last_run_commit()

    should_run = False

    if not last_commit:
        print("No previous run recorded. Running benchmarks for the first time.")
        should_run = True
    elif last_commit == current_commit:
        print("No new commits since last run.")
        should_run = False
    else:
        print(f"Last run commit: {last_commit}")
        if check_for_changes(last_commit, current_commit):
            print("Changes detected in code.")
            should_run = True
        else:
            print("No relevant changes detected (only scripts/ modified or no changes).")
            should_run = False

    if should_run:
        # Pass command line arguments to run_benchmarks
        run_benchmarks(sys.argv[1:])
        update_last_run_commit(current_commit)
    else:
        print("Skipping benchmarks.")

if __name__ == "__main__":
    main()
