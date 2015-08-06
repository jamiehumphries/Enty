namespace Enty.Configuration
{
    using System.Data.Entity;

    public class TestDbConfiguration<TContext> : ITestDbConfiguration<TContext> where TContext : DbContext
    {
        public ITestIdentityProvider TestIdentityProvider { get; set; }
        public IConnectionStringProvider ConnectionStringProvider { get; set; }
        public ITestDbContextFactory<TContext> TestDbContextFactory { get; set; }
    }
}
