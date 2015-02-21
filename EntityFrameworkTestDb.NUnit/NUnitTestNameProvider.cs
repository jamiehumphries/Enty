namespace EntityFrameworkTestDb.NUnit
{
    using EntityFrameworkTestDb.Configuration;
    using global::NUnit.Framework;

    public class NUnitTestNameProvider : ITestNameProvider
    {
        public string CurrentTestName
        {
            get { return TestContext.CurrentContext.Test.Name; }
        }
    }
}
