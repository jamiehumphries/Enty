namespace EntityTestDb.NUnitHelpers
{
    using EntityTestDb.Configuration;
    using NUnit.Framework;

    public class NUnitTestIdentityProvider : ITestIdentityProvider
    {
        public string GetTestIdentity()
        {
            return TestContext.CurrentContext.Test.Name;
        }
    }
}
