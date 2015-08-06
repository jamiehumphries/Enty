namespace Enty
{
    using Enty.Configuration;
    using System;
    using System.Data.Entity;

    internal static class ConfigurationHelper
    {
        internal static void ValidateConfiguration<TContext>(ITestDbConfiguration<TContext> configuration) where TContext : DbContext
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }
            if (configuration.TestIdentityProvider == null)
            {
                throw new ArgumentException("The TestIdentityProvider of the configuration argument cannot be null.");
            }
            if (configuration.ConnectionStringProvider == null)
            {
                throw new ArgumentException("The ConnectionStringProvider of the configuration argument cannot be null.");
            }
            if (configuration.TestDbContextFactory == null)
            {
                throw new ArgumentException("The TestDbContextFactory of the configuration argument cannot be null.");
            }
        }
    }
}
