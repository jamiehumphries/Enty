namespace EntityTestDb.Tests
{
    using EntityTestDb.Configuration;
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

        [Test]
        public void Constructing_test_db_with_null_connection_string_throws_exception()
        {
            // When
            // ReSharper disable once ObjectCreationAsStatement
            Action creatingDb = () => new TestDb(null, new TestDbContextFactory<DbContext>());

            // Then
            creatingDb.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Constructing_test_db_with_null_context_factory_throws_exception()
        {
            // When
            // ReSharper disable once ObjectCreationAsStatement
            Action creatingDb = () => new TestDb("db.sdf", null);

            // Then
            creatingDb.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Creating_test_db_with_null_config_throws_exception()
        {
            // When
            Action creatingDb = () => TestDb.Create((ITestDbConfiguration<DbContext>)null);

            // Then
            creatingDb.ShouldThrow<ArgumentNullException>();
        }

        [TestCase(typeof(NoTestIdentityProviderConfig))]
        [TestCase(typeof(NoConnectionStringProviderConfig))]
        [TestCase(typeof(NoTestDbContextFactoryConfig))]
        public void Creating_test_db_with_incomplete_configuration_throws_exception(Type configType)
        {
            // Given
            var config = (ITestDbConfiguration<DbContext>)Activator.CreateInstance(configType);

            // When
            Action creatingDb = () => TestDb.Create(config);

            // Then
            creatingDb.ShouldThrow<ArgumentException>();
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

        public class NoTestIdentityProviderConfig : TestDbConfiguration<DbContext>
        {
            public NoTestIdentityProviderConfig()
            {
                TestIdentityProvider = null;
                ConnectionStringProvider = new ConnectionStringProvider("db.sdf");
                TestDbContextFactory = new TestDbContextFactory<DbContext>();
            }
        }

        public class NoConnectionStringProviderConfig : TestDbConfiguration<DbContext>
        {
            public NoConnectionStringProviderConfig()
            {
                TestIdentityProvider = new TestIdentityProvider(() => "test");
                ConnectionStringProvider = null;
                TestDbContextFactory = new TestDbContextFactory<DbContext>();
            }
        }

        public class NoTestDbContextFactoryConfig : TestDbConfiguration<DbContext>
        {
            public NoTestDbContextFactoryConfig()
            {
                TestIdentityProvider = new TestIdentityProvider(() => "test");
                ConnectionStringProvider = new ConnectionStringProvider("db.sdf");
                TestDbContextFactory = null;
            }
        }
    }
}
