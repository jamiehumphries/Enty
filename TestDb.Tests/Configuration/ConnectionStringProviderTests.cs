namespace EntityTestDb.Tests.Configuration
{
    using EntityTestDb.Configuration;
    using FluentAssertions;
    using global::NUnit.Framework;
    using System;

    public class ConnectionStringProviderTests
    {
        [Test]
        public void Provides_static_connection_string_if_constructed_with_one()
        {
            // Given
            var provider = new ConnectionStringProvider("dummy connection string");

            // When
            var connectionString = provider.GetConnectionString("Test_name", DateTime.Now);

            // Then
            connectionString.Should().Be("dummy connection string");
        }

        [Test]
        public void Uses_delegate_from_constructor()
        {
            // Given
            var provider = new ConnectionStringProvider((testName, executionDate) => testName + executionDate.ToString("yyyyMMdd"));

            // When
            var connectionString = provider.GetConnectionString("Test", new DateTime(1989, 05, 19));

            // Then
            connectionString.Should().Be("Test19890519");
        }
    }
}
