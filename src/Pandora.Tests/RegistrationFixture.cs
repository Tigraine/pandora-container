using Xunit;

namespace Pandora.Tests
{
    public class RegistrationFixture
    {
        [Fact]
        public void CanCompareTwoIRegistrations()
        {
            IRegistration registration = new Registration();
            IRegistration registration2 = new Registration();
            Assert.False(registration == registration2);
        }

        [Fact]
        public void ConstructorAssignsGuid()
        {
            Assert.NotNull(new Registration().Guid);
        }
    }
}