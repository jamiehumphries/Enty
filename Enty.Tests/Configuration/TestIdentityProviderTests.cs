namespace Enty.Tests.Configuration
{
    using Enty.Configuration;
    using FluentAssertions;
    using global::NUnit.Framework;

    public class TestIdentityProviderTests
    {
        [Test]
        public void Uses_delegate_from_constructor()
        {
            // Given
            var provider = new TestIdentityProvider(() => "foobar");

            // When
            var identity = provider.GetTestIdentity();

            // Then
            identity.Should().Be("foobar");
        }
    }
}
