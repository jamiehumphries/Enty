namespace Enty.SqlServerCompact
{
    using Enty.Configuration;
    using System;
    using System.Diagnostics;
    using System.IO;

    public class SqlServerCompactConnectionStringProvider : IConnectionStringProvider
    {
        private static readonly Stopwatch Stopwatch = Stopwatch.StartNew();

        public string GetConnectionString(string testIdentity)
        {
            var cleanedTestIdentity = String.Concat(testIdentity.Split(Path.GetInvalidFileNameChars()));
            var dbFileName = "TestDb_" + cleanedTestIdentity + Stopwatch.ElapsedTicks;
            if (dbFileName.Length > 100)
            {
                dbFileName = dbFileName.Substring(0, 50) + "…" + dbFileName.Substring(dbFileName.Length - 50);
            }
            return String.Format("Data Source={0}.sdf;", dbFileName);
        }
    }
}
