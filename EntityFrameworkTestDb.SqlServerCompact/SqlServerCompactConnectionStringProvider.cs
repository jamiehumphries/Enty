namespace EntityFrameworkTestDb.SqlServerCompact
{
    using EntityFrameworkTestDb.Configuration;
    using System;
    using System.IO;

    public class SqlServerCompactConnectionStringProvider : ITestDbConnectionStringProvider
    {
        public string GetConnectionString(string testName, DateTime executionTime)
        {
            var cleanedTestName = String.Concat(testName.Split(Path.GetInvalidFileNameChars()));
            var dbFileName = cleanedTestName + executionTime.ToString("yyyyMMddHHmmssf");
            if (dbFileName.Length > 100)
            {
                dbFileName = dbFileName.Substring(0, 50) + "…" + dbFileName.Substring(dbFileName.Length - 50);
            }
            return String.Format("Data Source={0}.sdf;", dbFileName);
        }
    }
}
