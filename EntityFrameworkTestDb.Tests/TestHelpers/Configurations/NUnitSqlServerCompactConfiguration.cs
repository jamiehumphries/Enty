namespace EntityFrameworkTestDb.Tests.TestHelpers.Configurations
{
    using EntityFrameworkTestDb.Configuration;
    using EntityFrameworkTestDb.NUnitHelpers;
    using EntityFrameworkTestDb.SqlServerCompactHelpers;

    public class NUnitSqlServerCompactConfiguration : ITestDbConfiguration
    {
        public NUnitSqlServerCompactConfiguration()
        {
            TestIdentityProvider = new NUnitTestIdentityProvider();
            ConnectionStringProvider = new SqlServerCompactConnectionStringProvider();
            ContextFactory = new ContextFactory<TestDbContext>();
        }

        public ITestIdentityProvider TestIdentityProvider { get; private set; }
        public IConnectionStringProvider ConnectionStringProvider { get; private set; }
        public ITestDbContextFactory ContextFactory { get; private set; }
    }
}
