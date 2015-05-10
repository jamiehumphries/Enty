namespace EntityTestDb.Configuration
{
    using System.Data.Entity;

    public interface ITestDbContextFactory<out TContext> where TContext : DbContext
    {
        TContext GetDbContext(string connectionString);
    }
}
