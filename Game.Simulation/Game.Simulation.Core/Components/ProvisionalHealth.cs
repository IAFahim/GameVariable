using Unity.Entities;

namespace Game.Simulation.Core.Components
{
    /// <summary>
    /// Represents the "white portion" of health (provisional health).
    /// </summary>
    public struct ProvisionalHealth : IComponentData
    {
        public float Value;
    }
}
