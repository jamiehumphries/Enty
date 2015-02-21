namespace EntityFrameworkTestDb.Configuration
{
    public interface ITestDbConfiguration
    {
        ITestNameProvider TestNameProvider { get; set; }
        ITestDbConnectionStringProvider TestDbConnectionStringProvider { get; set; }
        ITestDbContextFactory TestDbContextFactory { get; set; }
    }
}
