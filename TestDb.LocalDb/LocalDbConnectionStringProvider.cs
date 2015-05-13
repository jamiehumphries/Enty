﻿namespace EntityTestDb.LocalDb
{
    using EntityTestDb.Configuration;
    using System;
    using System.IO;

    public class LocalDbConnectionStringProvider : IConnectionStringProvider
    {
        private readonly string version;

        public LocalDbConnectionStringProvider() : this(LocalDbVersion.MSSQLLocalDb) {}

        public LocalDbConnectionStringProvider(LocalDbVersion version) : this(version.ToVersionString()) {}

        public LocalDbConnectionStringProvider(string version)
        {
            this.version = version;
        }

        public string GetConnectionString(string testName, DateTime executionTime)
        {
            var cleanedTestName = String.Concat(testName.Split(Path.GetInvalidFileNameChars()));
            var dbFileName = cleanedTestName + executionTime.Ticks;
            if (dbFileName.Length > 100)
            {
                dbFileName = dbFileName.Substring(0, 50) + "…" + dbFileName.Substring(dbFileName.Length - 50);
            }
            var dbFilePath = Path.Combine(Directory.GetCurrentDirectory(), dbFileName + ".mdf");
            return String.Format(@"Data Source=(LocalDb)\{0};AttachDbFilename={1};Initial Catalog={2};Integrated Security=True;MultipleActiveResultSets=True;",
                version, dbFilePath, dbFileName);
        }
    }
}