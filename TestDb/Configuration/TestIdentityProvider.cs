namespace EntityTestDb.Configuration
{
    using System;

    public class TestIdentityProvider : ITestIdentityProvider
    {
        private readonly Func<string> getTestIdentity;

        public TestIdentityProvider(Func<string> getTestIdentity)
        {
            this.getTestIdentity = getTestIdentity;
        }

        public string GetTestIdentity()
        {
            return getTestIdentity.Invoke();
        }
    }
}
