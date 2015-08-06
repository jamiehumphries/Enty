namespace Enty.Configuration
{
    using System.Data.Entity;

    public interface ITestDbConfiguration<out TContext> where TContext : DbContext
    {
        ITestIdentityProvider TestIdentityProvider { get; }
        IConnectionStringProvider ConnectionStringProvider { get; }
        ITestDbContextFactory<TContext> TestDbContextFactory { get; }
    }
}
