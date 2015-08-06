namespace Enty.LocalDb
{
    using Enty.Configuration;
    using System;
    using System.Diagnostics;
    using System.IO;

    public class LocalDbConnectionStringProvider : IConnectionStringProvider
    {
        private static readonly Stopwatch StopWatch = Stopwatch.StartNew();
        private readonly string version;

        public LocalDbConnectionStringProvider() : this(LocalDbVersion.V11_0) {}

        public LocalDbConnectionStringProvider(LocalDbVersion version) : this(version.ToVersionString()) {}

        public LocalDbConnectionStringProvider(string version)
        {
            this.version = version;
        }

        public string GetConnectionString(string testIdentity)
        {
            var cleanedTestIdentity = String.Concat(testIdentity.Split(Path.GetInvalidFileNameChars()));
            var dbFileName = "TestDb_" + cleanedTestIdentity + StopWatch.ElapsedTicks;
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
