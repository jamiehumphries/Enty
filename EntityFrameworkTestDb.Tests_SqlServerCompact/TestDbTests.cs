namespace EntityFrameworkTestDb.Tests_SqlServerCompact
{
    using EntityFrameworkTestDb.Configuration;
    using EntityFrameworkTestDb.NUnitHelpers;
    using EntityFrameworkTestDb.SqlServerCompactHelpers;
    using EntityFrameworkTestDb.Tests;
    using EntityFrameworkTestDb.Tests.TestHelpers;

    public class TestDbTests : TestDbTests<NUnitSqlServerCompactConfiguration> {}

    public class NUnitSqlServerCompactConfiguration : ITestDbConfiguration<TestDbContext>
    {
        public NUnitSqlServerCompactConfiguration()
        {
            TestIdentityProvider = new NUnitTestIdentityProvider();
            ConnectionStringProvider = new SqlServerCompactConnectionStringProvider();
            ContextFactory = new TestDbContextFactory<TestDbContext>();
        }

        public ITestIdentityProvider TestIdentityProvider { get; private set; }
        public IConnectionStringProvider ConnectionStringProvider { get; private set; }
        public ITestDbContextFactory<TestDbContext> ContextFactory { get; private set; }
    }
}
