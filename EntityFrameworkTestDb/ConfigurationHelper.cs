namespace EntityFrameworkTestDb
{
    using EntityFrameworkTestDb.Configuration;
    using System;
    using System.Data.Entity;

    internal static class ConfigurationHelper
    {
        internal static Func<DbContext> GetContextFactoryMethod(ITestDbConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }
            if (configuration.TestNameProvider == null)
            {
                throw new ArgumentException("The TestNameProvider of the configuration argument cannot be null.");
            }
            if (configuration.TestDbConnectionStringProvider == null)
            {
                throw new ArgumentException("The TestDbConnectionStringProvider of the configuration argument cannot be null.");
            }
            if (configuration.TestDbContextFactory == null)
            {
                throw new ArgumentException("The TestDbContextFactory of the configuration argument cannot be null.");
            }
            var testName = configuration.TestNameProvider.CurrentTestName;
            var connectionString = configuration.TestDbConnectionStringProvider.GetConnectionString(testName, DateTime.Now);
            return () => configuration.TestDbContextFactory.GetDbContext(connectionString);
        }
    }
}
