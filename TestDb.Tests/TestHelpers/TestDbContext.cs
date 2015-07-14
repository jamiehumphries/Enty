﻿namespace EntityTestDb.Tests.TestHelpers
{
    using EntityTestDb.Tests.TestHelpers.Models;
    using System.Data.Entity;

    public class TestDbContext : DbContext
    {
        public TestDbContext(string nameOrConnectionString) : base(nameOrConnectionString) {}

        public DbSet<Person> People { get; set; }
        public DbSet<Dog> Dogs { get; set; }
    }
}
