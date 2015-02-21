namespace EntityFrameworkTestDb.Configuration
{
    public interface ITestDbConfiguration
    {
        ITestNameProvider TestNameProvider { get; set; }
        IConnectionStringProvider ConnectionStringProvider { get; set; }
        ITestDbContextFactory ContextFactory { get; set; }
    }

    public class TestDbConfiguration : ITestDbConfiguration
    {
        public ITestNameProvider TestNameProvider { get; set; }
        public IConnectionStringProvider ConnectionStringProvider { get; set; }
        public ITestDbContextFactory ContextFactory { get; set; }
    }
}
