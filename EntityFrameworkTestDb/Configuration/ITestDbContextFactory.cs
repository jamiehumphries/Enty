namespace EntityFrameworkTestDb.Configuration
{
    using System.Data.Entity;

    public interface ITestDbContextFactory
    {
        DbContext GetDbContext(string connectionString);
    }
}
