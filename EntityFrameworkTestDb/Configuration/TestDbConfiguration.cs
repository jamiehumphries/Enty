namespace EntityFrameworkTestDb.Configuration
{
    public interface ITestDbConfiguration
    {
        ITestIdentityProvider TestIdentityProvider { get; }
        IConnectionStringProvider ConnectionStringProvider { get; }
        ITestDbContextFactory ContextFactory { get; }
    }

    public class TestDbConfiguration : ITestDbConfiguration
    {
        public ITestIdentityProvider TestIdentityProvider { get; set; }
        public IConnectionStringProvider ConnectionStringProvider { get; set; }
        public ITestDbContextFactory ContextFactory { get; set; }
    }
}
