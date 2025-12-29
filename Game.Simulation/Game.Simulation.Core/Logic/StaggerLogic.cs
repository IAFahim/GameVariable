using Unity.Mathematics;

namespace Game.Simulation.Core.Logic
{
    public static class StaggerLogic
    {
        public static void UpdateStaggerDecay(
            ref float currentStagger, 
            float maxStagger, 
            float timeSinceLastHit, 
            float deltaTime)
        {
            // "If the enemy hasn’t been hit for 5 seconds, the stagger meter starts to reset to 0, 
            // fading at 20% at max stagger per second."
            
            if (timeSinceLastHit > 5.0f)
            {
                float decayAmount = maxStagger * 0.20f * deltaTime;
                currentStagger -= decayAmount;
                if (currentStagger < 0) currentStagger = 0;
            }
        }

        public static void AddStagger(
            ref float currentStagger, 
            float incomingStagger, 
            float maxStagger,
            float scalingCoefficient) 
        {
            // "If the enemies get consecutive hits, enemies get more stagger damage... Add a scaling coefficient"
            // Assuming scalingCoefficient is determined by the system based on combo count.
            
            float effectiveStagger = incomingStagger * scalingCoefficient;
            currentStagger += effectiveStagger;
            if (currentStagger > maxStagger) currentStagger = maxStagger;
        }
    }
}
