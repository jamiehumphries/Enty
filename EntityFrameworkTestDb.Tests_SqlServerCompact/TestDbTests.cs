namespace EntityFrameworkTestDb.Tests_SqlServerCompact
{
    using EntityFrameworkTestDb.Configuration;
    using EntityFrameworkTestDb.NUnitHelpers;
    using EntityFrameworkTestDb.SqlServerCompactHelpers;
    using EntityFrameworkTestDb.Tests;
    using EntityFrameworkTestDb.Tests.TestHelpers;

    public class TestDbTests : TestDbTests<NUnitSqlServerCompactConfiguration> {}

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
