namespace EntityFrameworkTestDb.Configuration
{
    public interface ITestNameProvider
    {
        string CurrentTestName { get; }
    }
}
