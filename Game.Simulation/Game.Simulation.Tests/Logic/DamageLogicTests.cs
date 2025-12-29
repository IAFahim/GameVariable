using Xunit;
using Game.Simulation.Core.Logic;

namespace Game.Simulation.Tests.Logic
{
    public class DamageLogicTests
    {
        [Fact]
        public void CalculateDamage_Standard_FirstHit_CorrectSplit()
        {
            float incoming = 10f;
            bool armored = false;
            float currentProv = 0f;

            DamageLogic.CalculateDamage(incoming, armored, currentProv, out float dmgToHealth, out float newProv);

            Assert.Equal(5f, dmgToHealth);
            Assert.Equal(5f, newProv);
        }

        [Fact]
        public void CalculateDamage_Standard_SecondHit_AccumulatesAndConverts()
        {
            float incoming = 10f;
            bool armored = false;
            float currentProv = 5f; // From previous hit

            DamageLogic.CalculateDamage(incoming, armored, currentProv, out float dmgToHealth, out float newProv);

            // New Actual: 5. Old Converted: 2.5. Total: 7.5
            Assert.Equal(7.5f, dmgToHealth);
            // New Prov: 5. Old Kept: 2.5. Total: 7.5
            Assert.Equal(7.5f, newProv);
        }

        [Fact]
        public void CalculateDamage_Armored_FirstHit_TriplesProvisional()
        {
            float incoming = 10f;
            bool armored = true;
            float currentProv = 0f;

            DamageLogic.CalculateDamage(incoming, armored, currentProv, out float dmgToHealth, out float newProv);

            Assert.Equal(0f, dmgToHealth);
            Assert.Equal(30f, newProv);
        }

        [Fact]
        public void CalculateDamage_Armored_SecondHit_ConvertsPrevious()
        {
            float incoming = 10f;
            bool armored = true;
            float currentProv = 30f;

            DamageLogic.CalculateDamage(incoming, armored, currentProv, out float dmgToHealth, out float newProv);

            // New Actual: 0. Old Converted: 15. Total: 15.
            Assert.Equal(15f, dmgToHealth);
            // New Prov: 30. Old Kept: 15. Total: 45.
            Assert.Equal(45f, newProv);
        }
    }
}
