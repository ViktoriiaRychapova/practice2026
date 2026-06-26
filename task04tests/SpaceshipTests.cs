using Xunit;
using Task04;

namespace Task04Tests
{
    public class SpaceshipTests
    {
        [Fact]
        public void Cruiser_ShouldHaveCorrectStats()
        {
            ISpaceship cruiser = new Cruiser();
            Assert.Equal(50, cruiser.Speed);
            Assert.Equal(100, cruiser.FirePower);
        }

        [Fact]
        public void Fighter_ShouldBeFasterThanCruiser()
        {
            var fighter = new Fighter();
            var cruiser = new Cruiser();
            Assert.True(fighter.Speed > cruiser.Speed);
        }

        [Fact]
        public void Fighter_ShouldHaveCorrectStats()
        {
            ISpaceship fighter = new Fighter();
            Assert.Equal(100, fighter.Speed);
            Assert.Equal(20, fighter.FirePower);
        }

        [Theory]
        [InlineData(typeof(Fighter), 20)]
        [InlineData(typeof(Cruiser), 100)]
        public void FirePower_ShouldMatchShipType(Type shipType, int expectedPower)
        {
            var instance = Activator.CreateInstance(shipType);
            Assert.NotNull(instance);
            ISpaceship ship = (ISpaceship)instance;
            Assert.Equal(expectedPower, ship.FirePower);
        }

        [Fact]
        public void Ship_ShouldBeAbleToPerformActions()
        {
            ISpaceship ship = new Fighter();
            
            var exception = Record.Exception(() =>
            {
                ship.MoveForward();
                ship.Rotate(45);
                ship.Fire();
            });
            Assert.Null(exception);
        }
    }
}