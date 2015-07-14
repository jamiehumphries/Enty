namespace EntityTestDb
{
    using AutoMapper;
    using EntityTestDb.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class TestDb<TContext> : TestDb, IDisposable where TContext : DbContext
    {
        public TestDb(string connectionString, ITestDbContextFactory<TContext> contextFactory) : base(connectionString, contextFactory) {}

        public new TContext GetDbContext()
        {
            return (TContext)base.GetDbContext();
        }
    }

    public partial class TestDb
    {
        private readonly ITestDbContextFactory<DbContext> contextFactory;

        public TestDb(string connectionString, ITestDbContextFactory<DbContext> contextFactory)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException("connectionString");
            }
            if (contextFactory == null)
            {
                throw new ArgumentNullException("contextFactory");
            }
            ConnectionString = connectionString;
            this.contextFactory = contextFactory;
        }

        public string ConnectionString { get; private set; }

        public DbContext GetDbContext()
        {
            return contextFactory.GetDbContext(ConnectionString);
        }

        public void CreateIfNotExists()
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

        public ICollection<T> GetAll<T>() where T : class
        {
            using (var context = GetDbContext())
            {
                MappingHelper.CreateMapsForContextTypes(context);
                return Mapper.Map<IEnumerable<T>>(context.Set<T>()).ToList();
            }
        }

        public ICollection<T> SeedMany<T>(IEnumerable<T> entities) where T : class
        {
            return Seed(entities.ToArray());
        }

        public T Seed<T>(T entity) where T : class
        {
            return Seed(new[] { entity }).Single();
        }

        public ICollection<T> Seed<T>(params T[] entities) where T : class
        {
            using (var context = GetDbContext())
            {
                context.Set<T>().AddRange(entities);
                context.SaveChanges();
            }
            return entities;
        }

        public void Seed(params object[] entities)
        {
            using (var context = GetDbContext())
            {
                foreach (var entity in entities)
                {
                    context.Set(entity.GetType()).Add(entity);
                }
                context.SaveChanges();
            }
        }
    }
}
