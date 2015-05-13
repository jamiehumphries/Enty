namespace EntityTestDb.Configuration
{
    using System;

    public class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly Func<string, DateTime, string> getConnectionString;

        public ConnectionStringProvider(string connectionString) : this((testName, executionTime) => connectionString) {}

        public ConnectionStringProvider(Func<string, DateTime, string> getConnectionString)
        {
            this.getConnectionString = getConnectionString;
        }

        public string GetConnectionString(string testName, DateTime executionTime)
        {
            return getConnectionString(testName, executionTime);
        }
    }
}
