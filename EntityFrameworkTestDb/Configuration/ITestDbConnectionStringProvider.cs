namespace EntityFrameworkTestDb.Configuration
{
    using System;

    public interface ITestDbConnectionStringProvider
    {
        string GetConnectionString(string testName, DateTime executionTime);
    }
}
