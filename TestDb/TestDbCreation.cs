namespace EntityTestDb
{
    using EntityTestDb.Configuration;
    using System;
    using System.Data.Entity;

    public partial class TestDb
    {
        public static TestDb<TContext> Create<TContext, TConfig>() where TContext : DbContext where TConfig : ITestDbConfiguration<TContext>
        {
            return Create(Activator.CreateInstance<TConfig>());
        }

        public static TestDb<TContext> Create<TContext>(ITestDbConfiguration<TContext> configuration) where TContext : DbContext
        {
            ConfigurationHelper.ValidateConfiguration(configuration);
            var testIdentity = configuration.TestIdentityProvider.GetTestIdentity();
            var connectionString = configuration.ConnectionStringProvider.GetConnectionString(testIdentity, DateTime.Now);
            return Create(connectionString, configuration.TestDbContextFactory);
        }

        public static TestDb<TContext> Create<TContext>(string connectionString) where TContext : DbContext
        {
            return Create(connectionString, new TestDbContextFactory<TContext>());
        }

        public static TestDb<TContext> Create<TContext>(string connectionString, ITestDbContextFactory<TContext> contextFactory) where TContext : DbContext
        {
            var testDb = new TestDb<TContext>(connectionString, contextFactory);
            testDb.CreateIfNotExists();
            return testDb;
        }

        public static TestDb Create<TConfig>() where TConfig : ITestDbConfiguration<DbContext>
        {
            return Create<DbContext, TConfig>();
        }

        public static TestDb Create(ITestDbConfiguration<DbContext> configuration)
        {
            return Create<DbContext>(configuration);
        }

        public static TestDb Create(string connectionString)
        {
            return Create<DbContext>(connectionString);
        }

        public static TestDb Create(string connectionString, ITestDbContextFactory<DbContext> contextFactory)
        {
            return Create<DbContext>(connectionString, contextFactory);
        }
    }
}
