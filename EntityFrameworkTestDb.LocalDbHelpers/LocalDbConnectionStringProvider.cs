namespace EntityFrameworkTestDb.LocalDbHelpers
{
    using EntityFrameworkTestDb.Configuration;
    using System;
    using System.IO;

    public class LocalDbConnectionStringProvider : IConnectionStringProvider
    {
        public string GetConnectionString(string testName, DateTime executionTime)
        {
            var cleanedTestName = String.Concat(testName.Split(Path.GetInvalidFileNameChars()));
            var dbFileName = cleanedTestName + executionTime.Ticks;
            if (dbFileName.Length > 100)
            {
                dbFileName = dbFileName.Substring(0, 50) + "…" + dbFileName.Substring(dbFileName.Length - 50);
            }
            var dbFilePath = Path.Combine(Directory.GetCurrentDirectory(), dbFileName + ".mdf");
            return String.Format(@"Data Source=(LocalDb)\v11.0;AttachDbFilename={0};Initial Catalog={1};Integrated Security=True;MultipleActiveResultSets=True;", dbFilePath, dbFileName);
        }
    }
}
