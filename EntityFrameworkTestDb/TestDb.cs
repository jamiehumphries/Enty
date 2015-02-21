namespace EntityFrameworkTestDb
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class TestDb<TContext> where TContext : DbContext
    {
        private readonly string connectionString;
        private readonly Func<string, TContext> contextFactoryMethod;

        public TestDb(string connectionString, Func<string, TContext> contextFactoryMethod)
        {
            this.connectionString = connectionString;
            this.contextFactoryMethod = contextFactoryMethod;
        }

        public IList<T> GetAll<T>() where T : class
        {
            using (var context = GetDbContext())
            {
                return context.Set<T>().ToList();
            }
        }

        protected virtual TContext GetDbContext()
        {
            return contextFactoryMethod(connectionString);
        }
    }
}
