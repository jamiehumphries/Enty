namespace EntityTestDb.SqlServerCompactHelpers
{
    using EntityTestDb.Configuration;
    using System;
    using System.IO;

    public class SqlServerCompactConnectionStringProvider : IConnectionStringProvider
    {
        public string GetConnectionString(string testName, DateTime executionTime)
        {
            var cleanedTestName = String.Concat(testName.Split(Path.GetInvalidFileNameChars()));
            var dbFileName = cleanedTestName + executionTime.Ticks;
            if (dbFileName.Length > 100)
            {
                dbFileName = dbFileName.Substring(0, 50) + "…" + dbFileName.Substring(dbFileName.Length - 50);
            }
            return String.Format("Data Source={0}.sdf;", dbFileName);
        }
    }
}
