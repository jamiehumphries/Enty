namespace EntityTestDb
{
    using AutoMapper;
    using EntityTestDb.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class TestDb<TContext, TConfig> : TestDb<TContext> where TConfig : ITestDbConfiguration<TContext> where TContext : DbContext
    {
        public TestDb() : base(Activator.CreateInstance<TConfig>()) {}
    }

    public class TestDb<TContext> : IDisposable where TContext : DbContext
    {
        private readonly DateTime executionTime = DateTime.Now;
        private readonly ITestDbContextFactory<TContext> contextFactory;

        public TestDb(ITestDbConfiguration<TContext> configuration)
        {
            ConfigurationHelper.ValidateConfiguration(configuration);
            contextFactory = configuration.TestDbContextFactory;
            var testIdentity = configuration.TestIdentityProvider.GetTestIdentity();
            ConnectionString = configuration.ConnectionStringProvider.GetConnectionString(testIdentity, executionTime);
        }

        public TestDb(string connectionString) : this(connectionString, new TestDbContextFactory<TContext>()) {}

        public TestDb(string connectionString, ITestDbContextFactory<TContext> contextFactory)
        {
            ConnectionString = connectionString;
            this.contextFactory = contextFactory;
        }

        public string ConnectionString { get; private set; }

        public TContext GetDbContext()
        {
            return contextFactory.GetDbContext(ConnectionString);
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
