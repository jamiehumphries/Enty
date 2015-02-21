namespace EntityFrameworkTestDb.Configuration
{
    using System;
    using System.Data.Entity;

    public class ContextFactory<T> : ContextFactory where T : DbContext
    {
        public ContextFactory() : base(connectionString => (T)Activator.CreateInstance(typeof(T), connectionString)) {}
    }

    public class ContextFactory : ITestDbContextFactory
    {
        private readonly Func<string, DbContext> dbContextFromConnectionString;

        public ContextFactory(Func<string, DbContext> dbContextFromConnectionString)
        {
            this.dbContextFromConnectionString = dbContextFromConnectionString;
        }

        public DbContext GetDbContext(string connectionString)
        {
            return dbContextFromConnectionString.Invoke(connectionString);
        }
    }
}
