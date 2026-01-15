# Contributing to GameVariable

Thank you for your interest in contributing to GameVariable! We welcome contributions from the community.

## Getting Started

1.  **Fork the repository** on GitHub.
2.  **Clone your fork** locally.
3.  **Install dependencies**: Run `./dotnet-install.sh` (or `dotnet restore`).
4.  **Create a branch** for your feature or bug fix.

## Development Standards

### Code Style
*   **Zero Allocation**: This is a core philosophy. Avoid heap allocations (`new class`, closures, LINQ, boxing) in hot paths.
*   **Structs**: Prefer `struct` over `class` for data types.
*   **Modifiers**: Use `[MethodImpl(MethodImplOptions.AggressiveInlining)]` for small, frequently called methods.
*   **Serialization**: Ensure all data structs are `[Serializable]` and `[StructLayout(LayoutKind.Sequential)]`.

### Documentation
*   **XML Comments**: All public types and members **must** have XML documentation (`/// <summary>`).
*   **README**: If you add a new feature or package, update the relevant `README.md`.

### Testing
*   **Unit Tests**: All new logic must be covered by unit tests.
*   **Run Tests**: Run `dotnet test` to ensure all tests pass before submitting.

## Submitting a Pull Request

1.  Push your branch to your fork.
2.  Open a Pull Request against the `main` branch.
3.  Describe your changes clearly.
4.  Wait for review.

## License

By contributing, you agree that your contributions will be licensed under the MIT License.
