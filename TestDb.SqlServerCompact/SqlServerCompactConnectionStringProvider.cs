namespace EntityTestDb.SqlServerCompact
{
    using EntityTestDb.Configuration;
    using System;
    using System.Diagnostics;
    using System.IO;

    public class SqlServerCompactConnectionStringProvider : IConnectionStringProvider
    {
        private static readonly Stopwatch Stopwatch = Stopwatch.StartNew();

        public string GetConnectionString(string testName)
        {
            var cleanedTestName = String.Concat(testName.Split(Path.GetInvalidFileNameChars()));
            var dbFileName = "TestDb_" + cleanedTestName + Stopwatch.ElapsedTicks;
            if (dbFileName.Length > 100)
            {
                dbFileName = dbFileName.Substring(0, 50) + "…" + dbFileName.Substring(dbFileName.Length - 50);
            }
            return String.Format("Data Source={0}.sdf;", dbFileName);
        }
    }
}
