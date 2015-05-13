namespace EntityTestDb.Tests
{
    using EntityTestDb.Configuration;
    using EntityTestDb.Tests.Test;
    using EntityTestDb.Tests.TestHelpers;
    using global::NUnit.Framework;
    using NCrunch.Framework;
    using System;
    using System.Data.Entity;

    public class TestDbCreationTests
    {
        private TestDb testDb;
        private string connectionString;

        [SetUp]
        public void SetUp()
        {
            connectionString = Guid.NewGuid().ToString();
        }

        [TearDown]
        public void TearDown()
        {
            if (testDb != null)
            {
                testDb.Dispose();
            }
        }

        [Test]
        [ExclusivelyUses("TestConfigConnectionString")]
        public void Can_create_non_generic_test_db_using_configuration_class()
        {
            // Given
            TestConfig.ConnectionString = connectionString;

            // When
            testDb = TestDb.Create<NonGenericTestConfig>();

            // Then
            testDb.Should().Exist();
        }

        [Test]
        [ExclusivelyUses("TestConfigConnectionString")]
        public void Can_create_generic_test_db_using_configuration_class()
        {
            // Given
            TestConfig.ConnectionString = connectionString;

            // When
            testDb = TestDb.Create<TestDbContext, GenericTestConfig>();

            // Then
            testDb.Should().BeAssignableTo<TestDb<TestDbContext>>();
            testDb.Should().Exist();
        }

        [Test]
        public void Can_create_non_generic_test_db_using_configuration_instance()
        {
            // Given
            var configuration = new NonGenericTestConfig(connectionString);

            // When
            testDb = TestDb.Create(configuration);

            // Then
            testDb.Should().Exist();
        }

        [Test]
        public void Can_create_generic_test_db_using_configuration_instance()
        {
            // Given
            var configuration = new GenericTestConfig(connectionString);

            // When
            testDb = TestDb.Create(configuration);

            // Then
            testDb.Should().BeAssignableTo<TestDb<TestDbContext>>();
            testDb.Should().Exist();
        }

        [Test]
        public void Can_create_non_generic_test_db_using_connection_string()
        {
            // When
            testDb = TestDb.Create(connectionString);

            // Then
            testDb.Should().Exist();
        }

        [Test]
        public void Can_create_generic_test_db_using_connection_string()
        {
            // When
            testDb = TestDb.Create<TestDbContext>(connectionString);

            // Then
            testDb.Should().BeAssignableTo<TestDb<TestDbContext>>();
            testDb.Should().Exist();
        }

        [Test]
        public void Can_create_non_generic_test_db_using_connection_string_and_context_factory()
        {
            // When
            testDb = TestDb.Create(connectionString, new TestDbContextFactory<DbContext>());

            // Then
            testDb.Should().Exist();
        }

        [Test]
        public void Can_create_generic_test_db_using_connection_string_and_context_factory()
        {
            // When
            testDb = TestDb.Create(connectionString, new TestDbContextFactory<TestDbContext>());

            // Then
            testDb.Should().BeAssignableTo<TestDb<TestDbContext>>();
            testDb.Should().Exist();
        }

        public static class TestConfig
        {
            public static string ConnectionString;
        }

        public class NonGenericTestConfig : TestConfig<DbContext>
        {
            public NonGenericTestConfig() {}
            public NonGenericTestConfig(string connectionString) : base(connectionString) {}
        }

        public class GenericTestConfig : TestConfig<TestDbContext>
        {
            public GenericTestConfig() {}
            public GenericTestConfig(string connectionString) : base(connectionString) {}
        }

        public class TestConfig<TContext> : TestDbConfiguration<TContext> where TContext : DbContext
        {
            public TestConfig() : this(TestConfig.ConnectionString) {}

            public TestConfig(string connectionString)
            {
                TestIdentityProvider = new TestIdentityProvider(() => "");
                ConnectionStringProvider = new ConnectionStringProvider(connectionString);
                TestDbContextFactory = new TestDbContextFactory<TContext>();
            }
        }
    }
}
