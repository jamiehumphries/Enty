namespace EntityFrameworkTestDb.NUnitHelpers
{
    using EntityFrameworkTestDb.Configuration;
    using NUnit.Framework;

    public class NUnitTestIdentityProvider : ITestIdentityProvider
    {
        public string GetTestIdentity()
        {
            return TestContext.CurrentContext.Test.Name;
        }
    }
}
