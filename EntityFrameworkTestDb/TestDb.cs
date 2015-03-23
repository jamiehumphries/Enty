﻿namespace EntityFrameworkTestDb
{
    using AutoMapper;
    using EntityFrameworkTestDb.Configuration;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class TestDb : TestDb<DbContext>
    {
        public TestDb(ITestDbConfiguration<DbContext> configuration) : base(configuration) {}
    }

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
            contextFactory = configuration.ContextFactory;
            var testIdentity = configuration.TestIdentityProvider.GetTestIdentity();
            ConnectionString = configuration.ConnectionStringProvider.GetConnectionString(testIdentity, executionTime);
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

        public IEnumerable<T> GetAll<T>() where T : class
        {
            using (var context = GetDbContext())
            {
                return context.Set<T>().AsEnumerable().Select(Mapper.DynamicMap<T>).ToList();
            }
        }

        public void SeedMany(IEnumerable entities)
        {
            Seed(entities.Cast<object>().ToArray());
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
