namespace EntityFrameworkTestDb
{
    using EntityFrameworkTestDb.Configuration;
    using System;

    internal static class ConfigurationHelper
    {
        internal static void ValidateConfiguration(ITestDbConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }
            if (configuration.TestNameProvider == null)
            {
                throw new ArgumentException("The TestNameProvider of the configuration argument cannot be null.");
            }
            if (configuration.ConnectionStringProvider == null)
            {
                throw new ArgumentException("The ConnectionStringProvider of the configuration argument cannot be null.");
            }
            if (configuration.ContextFactory == null)
            {
                throw new ArgumentException("The ContextFactory of the configuration argument cannot be null.");
            }
        }
    }
}
