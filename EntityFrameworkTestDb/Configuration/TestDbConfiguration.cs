namespace EntityFrameworkTestDb.Configuration
{
    using System.Data.Entity;

    public interface ITestDbConfiguration<out TContext> where TContext : DbContext
    {
        ITestIdentityProvider TestIdentityProvider { get; }
        IConnectionStringProvider ConnectionStringProvider { get; }
        ITestDbContextFactory<TContext> ContextFactory { get; }
    }

    public class TestDbConfiguration<TContext> : ITestDbConfiguration<TContext> where TContext : DbContext
    {
        public ITestIdentityProvider TestIdentityProvider { get; set; }
        public IConnectionStringProvider ConnectionStringProvider { get; set; }
        public ITestDbContextFactory<TContext> ContextFactory { get; set; }
    }
}
