namespace EntityFrameworkTestDb.Configuration
{
    public interface ITestDbConfiguration
    {
        ITestNameProvider TestNameProvider { get; }
        IConnectionStringProvider ConnectionStringProvider { get; }
        ITestDbContextFactory ContextFactory { get; }
    }

    public class TestDbConfiguration : ITestDbConfiguration
    {
        public ITestNameProvider TestNameProvider { get; set; }
        public IConnectionStringProvider ConnectionStringProvider { get; set; }
        public ITestDbContextFactory ContextFactory { get; set; }
    }
}
