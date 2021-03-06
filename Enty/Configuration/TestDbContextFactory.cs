﻿namespace Enty.Configuration
{
    using System;
    using System.Data.Entity;

    public class TestDbContextFactory<TContext> : ITestDbContextFactory<TContext> where TContext : DbContext
    {
        private readonly Func<string, TContext> dbContextFromConnectionString;

        public TestDbContextFactory() : this(connectionString => (TContext)Activator.CreateInstance(typeof(TContext), connectionString)) {}

        public TestDbContextFactory(Func<string, TContext> dbContextFromConnectionString)
        {
            this.dbContextFromConnectionString = dbContextFromConnectionString;
        }

        public TContext GetDbContext(string connectionString)
        {
            return dbContextFromConnectionString.Invoke(connectionString);
        }
    }
}
