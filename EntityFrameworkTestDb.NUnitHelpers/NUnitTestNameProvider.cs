namespace EntityFrameworkTestDb.NUnitHelpers
{
    using EntityFrameworkTestDb.Configuration;
    using NUnit.Framework;

    public class NUnitTestNameProvider : ITestNameProvider
    {
        public string CurrentTestName
        {
            get { return TestContext.CurrentContext.Test.Name; }
        }
    }
}
