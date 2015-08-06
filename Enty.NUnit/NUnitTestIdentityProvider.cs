namespace Enty.NUnit
{
    using Enty.Configuration;
    using global::NUnit.Framework;

    public class NUnitTestIdentityProvider : ITestIdentityProvider
    {
        public string GetTestIdentity()
        {
            return TestContext.CurrentContext.Test.Name;
        }
    }
}
