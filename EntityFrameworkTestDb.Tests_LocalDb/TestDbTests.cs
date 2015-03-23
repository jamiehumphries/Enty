namespace EntityFrameworkTestDb.Tests_LocalDb
{
    using EntityFrameworkTestDb.Configuration;
    using EntityFrameworkTestDb.LocalDbHelpers;
    using EntityFrameworkTestDb.NUnitHelpers;
    using EntityFrameworkTestDb.Tests;
    using EntityFrameworkTestDb.Tests.TestHelpers;

    public class TestDbTests : TestDbTests<NUnitLocalDbConfiguration> {}

    public class NUnitLocalDbConfiguration : ITestDbConfiguration
    {
        public NUnitLocalDbConfiguration()
        {
            TestIdentityProvider = new NUnitTestIdentityProvider();
            ConnectionStringProvider = new LocalDbConnectionStringProvider();
            ContextFactory = new ContextFactory<TestDbContext>();
        }

        public ITestIdentityProvider TestIdentityProvider { get; private set; }
        public IConnectionStringProvider ConnectionStringProvider { get; private set; }
        public ITestDbContextFactory ContextFactory { get; private set; }
    }
}
