namespace Game.Simulation.Core.Logic
{
    public static class DamageLogic
    {
        /// <summary>
        /// Calculates damage to health and updates provisional health based on game rules.
        /// </summary>
        /// <param name="incomingDamage">Raw incoming damage amount.</param>
        /// <param name="isArmored">Whether the entity is in an Armored state.</param>
        /// <param name="currentProvisionalHealth">The current accumulated provisional health.</param>
        /// <param name="damageToCurrentHealth">Output: Amount to subtract from current health.</param>
        /// <param name="newProvisionalHealthTotal">Output: New value for provisional health.</param>
        public static void CalculateDamage(
            float incomingDamage, 
            bool isArmored, 
            float currentProvisionalHealth,
            out float damageToCurrentHealth,
            out float newProvisionalHealthTotal)
        {
            float provisionalFromHit;
            float actualDamageFromHit;

            if (isArmored)
            {
                // "provisional damage is multiplied by 3 if he is in an armoured state."
                // "In Armored state, Provisional Health accounts for ALL damage taken from a single hit."
                // Logic: 0 actual, 3x provisional.
                provisionalFromHit = incomingDamage * 3.0f;
                actualDamageFromHit = 0f;
            }
            else
            {
                // "Provisional Health = on default, accounts for 50%... of the damage taken from single hits."
                provisionalFromHit = incomingDamage * 0.5f;
                actualDamageFromHit = incomingDamage * 0.5f;
            }

            // "Upon taking further damage, the previous hit is halved and added to the new provisional health.
            // Half of the previous damage is taken as actual damage on the 2nd hit."
            
            float previousConvertedToActual = 0f;
            float previousKept = 0f;

            if (currentProvisionalHealth > 0)
            {
                 previousConvertedToActual = currentProvisionalHealth * 0.5f;
                 previousKept = currentProvisionalHealth * 0.5f;
            }

            damageToCurrentHealth = actualDamageFromHit + previousConvertedToActual;
            newProvisionalHealthTotal = previousKept + provisionalFromHit;
        }
    }
}
