namespace EntityFrameworkTestDb
{
    using AutoMapper;
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
        private readonly ITestDbConfiguration configuration;
        private readonly DateTime executionTime = DateTime.Now;

        public TestDb(ITestDbConfiguration configuration)
        {
            ConfigurationHelper.ValidateConfiguration(configuration);
            this.configuration = configuration;
        }

        public string ConnectionString
        {
            get
            {
                var testName = configuration.TestNameProvider.CurrentTestName;
                return configuration.ConnectionStringProvider.GetConnectionString(testName, executionTime);
            }
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
                return context.Set<T>().AsEnumerable().Select(Mapper.DynamicMap<T>).ToList();
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

        private DbContext GetDbContext()
        {
            return configuration.ContextFactory.GetDbContext(ConnectionString);
        }
    }
}
