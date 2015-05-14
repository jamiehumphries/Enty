namespace EntityTestDb.Tests.Configuration
{
    using EntityTestDb.Configuration;
    using FluentAssertions;
    using global::NUnit.Framework;

    public class ConnectionStringProviderTests
    {
        [Test]
        public void Provides_static_connection_string_if_constructed_with_one()
        {
            // Given
            var provider = new ConnectionStringProvider("dummy connection string");

            // When
            var connectionString = provider.GetConnectionString("Test_name");

            // Then
            connectionString.Should().Be("dummy connection string");
        }

        [Test]
        public void Uses_delegate_from_constructor()
        {
            // Given
            var provider = new ConnectionStringProvider(testIdentity => testIdentity + "ConnectionString");

            // When
            var connectionString = provider.GetConnectionString("Test");

            // Then
            connectionString.Should().Be("TestConnectionString");
        }
    }
}
