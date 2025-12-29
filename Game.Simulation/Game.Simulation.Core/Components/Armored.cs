using Unity.Entities;

namespace Game.Simulation.Core.Components
{
    /// <summary>
    /// Tag component indicating the entity is in an Armored state.
    /// In Armored state, provisional damage is multiplied by 3.
    /// </summary>
    public struct Armored : IComponentData, IEnableableComponent
    {
    }
}
