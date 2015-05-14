namespace EntityTestDb.Configuration
{
    using System;

    public class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly Func<string, string> getConnectionString;

        public ConnectionStringProvider(string connectionString) : this(testName => connectionString) {}

        public ConnectionStringProvider(Func<string, string> getConnectionString)
        {
            this.getConnectionString = getConnectionString;
        }

        public string GetConnectionString(string testName)
        {
            return getConnectionString(testName);
        }
    }
}
