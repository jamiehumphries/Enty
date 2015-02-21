namespace EntityFrameworkTestDb.Configuration
{
    using System;
    using System.Data.Entity;

    public class TestDbContextFactory<T> : TestDbContextFactory where T : DbContext
    {
        public TestDbContextFactory() : base(connectionString => (T)Activator.CreateInstance(typeof(T), connectionString)) {}
    }

    public class TestDbContextFactory : ITestDbContextFactory
    {
        private readonly Func<string, DbContext> dbContextFromConnectionString;

        public TestDbContextFactory(Func<string, DbContext> dbContextFromConnectionString)
        {
            this.dbContextFromConnectionString = dbContextFromConnectionString;
        }

        public DbContext GetDbContext(string connectionString)
        {
            return dbContextFromConnectionString.Invoke(connectionString);
        }
    }
}
