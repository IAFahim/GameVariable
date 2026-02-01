namespace GameVariable.Synergy;

/// <summary>
///     Stateless logic for verifying interactions between different variable types.
///     Adheres to "Primitives Only" and "Layer B" constraints.
/// </summary>
public static class SynergyLogic
{
    /// <summary>
    ///     Checks if an action can be performed based on resource availability and cooldown state.
    /// </summary>
    /// <param name="resourceCurrent">Current resource amount (e.g., Mana).</param>
    /// <param name="resourceCost">Cost of the action.</param>
    /// <param name="cooldownCurrent">Current cooldown time remaining.</param>
    /// <param name="isReady">Outputs true if resource is sufficient and cooldown is finished.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CheckAbilityReadiness(in float resourceCurrent, in float resourceCost, in float cooldownCurrent,
        out bool isReady)
    {
        // 1. Check Cooldown (must be 0 or less, effectively empty)
        // We use a small epsilon or direct comparison. Since Cooldown clamps to 0, <= 0 is safe.
        // Bounded types usually consider "Empty" as <= Min (0).
        bool isCooldownReady = cooldownCurrent <= 0f;

        // 2. Check Resource (must be >= Cost)
        bool hasResource = resourceCurrent >= resourceCost;

        isReady = isCooldownReady && hasResource;
    }
}
