namespace EntityTestDb.Tests
{
    using EntityTestDb.Configuration;
    using EntityTestDb.Tests.Test;
    using EntityTestDb.Tests.TestHelpers;
    using FluentAssertions;
    using global::NUnit.Framework;
    using NCrunch.Framework;
    using System;
    using System.Data.Entity;

    public class TestDbCreationTests
    {
        private TestDb testDb;
        private string connectionString;

        private ITestDbConfiguration<DbContext> defaultConfiguration;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            defaultConfiguration = TestDb.Configuration;
        }

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
        [ExclusivelyUses("GlobalConfiguration")]
        public void Can_create_non_generic_db_using_default_configuration()
        {
            // Given
            TestDb.Configuration = defaultConfiguration;

            // When
            testDb = TestDb.Create();

            // Then
            testDb.Should().Exist();
        }

        [Test]
        [ExclusivelyUses("GlobalConfiguration")]
        public void Can_create_non_generic_db_using_global_configuration()
        {
            // Given
            TestDb.Configuration = new NonGenericTestConfig(connectionString);

            // When
            testDb = TestDb.Create();

            // Then
            testDb.Should().Exist();
        }

        [Test]
        [ExclusivelyUses("GlobalConfiguration")]
        public void Can_create_generic_db_using_global_configuration()
        {
            // Given
            TestDb.Configuration = new GenericTestConfig(connectionString);

            // When
            testDb = TestDb.Create<TestDbContext>();

            // Then
            testDb.GetDbContext().Should().NotBeNull().And.BeAssignableTo<TestDbContext>();
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
            testDb.GetDbContext().Should().NotBeNull().And.BeAssignableTo<TestDbContext>();
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
            testDb.GetDbContext().Should().NotBeNull().And.BeAssignableTo<TestDbContext>();
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
            testDb.GetDbContext().Should().NotBeNull().And.BeAssignableTo<TestDbContext>();
            testDb.Should().Exist();
        }

        public class NonGenericTestConfig : TestConfig<DbContext>
        {
            public NonGenericTestConfig(string connectionString) : base(connectionString) {}
        }

        public class GenericTestConfig : TestConfig<TestDbContext>
        {
            public GenericTestConfig(string connectionString) : base(connectionString) {}
        }

        public class TestConfig<TContext> : TestDbConfiguration<TContext> where TContext : DbContext
        {
            public TestConfig(string connectionString)
            {
                TestIdentityProvider = new TestIdentityProvider(() => "");
                ConnectionStringProvider = new ConnectionStringProvider(connectionString);
                TestDbContextFactory = new TestDbContextFactory<TContext>();
            }
        }
    }
}
