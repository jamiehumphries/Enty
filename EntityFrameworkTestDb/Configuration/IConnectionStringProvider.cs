namespace EntityFrameworkTestDb.Configuration
{
    using System;

    public interface IConnectionStringProvider
    {
        string GetConnectionString(string testName, DateTime executionTime);
    }
}
