namespace Variable.Inventory;

/// <summary>
///     Provides static logic for inventory management, including capacity checks,
///     adding/removing items, and transferring items between containers.
/// </summary>
public static partial class InventoryLogic
{
    /// <summary>
    ///     A small tolerance value used for floating-point comparisons to handle precision errors.
    /// </summary>
    public const float TOLERANCE = 0.001f;

    /// <summary>
    ///     A small tolerance value used for double-precision floating-point comparisons.
    /// </summary>
    public const double TOLERANCE_DOUBLE = 0.001;
}