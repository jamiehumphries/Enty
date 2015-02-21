namespace EntityFrameworkTestDb
{
    using EntityFrameworkTestDb.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class TestDb<TConfiguration> : TestDb where TConfiguration : ITestDbConfiguration
    {
        public TestDb() : base(Activator.CreateInstance<TConfiguration>()) {}
    }

    public class TestDb : IDisposable
    {
        private readonly Func<DbContext> contextFactoryMethod;

        public TestDb(ITestDbConfiguration configuration) : this(ConfigurationHelper.GetContextFactoryMethod(configuration)) {}

        public TestDb(Func<DbContext> contextFactoryMethod)
        {
            if (contextFactoryMethod == null)
            {
                throw new ArgumentNullException("contextFactoryMethod");
            }
            this.contextFactoryMethod = contextFactoryMethod;
        }

        public void Create()
        {
            using (var context = GetDbContext())
            {
                context.Database.CreateIfNotExists();
            }
        }

        public void Dispose()
        {
            using (var context = GetDbContext())
            {
                context.Database.Delete();
            }
        }

        public IEnumerable<T> GetAll<T>() where T : class
        {
            using (var context = GetDbContext())
            {
                return context.Set<T>().ToList();
            }
        }

        public void Seed<T>(params T[] entities) where T : class
        {
            SeedMany(entities);
        }

        public void SeedMany<T>(IEnumerable<T> entities) where T : class
        {
            using (var context = GetDbContext())
            {
                context.Set<T>().AddRange(entities);
                context.SaveChanges();
            }
        }

        private DbContext GetDbContext()
        {
            return contextFactoryMethod.Invoke();
        }
    }
}
