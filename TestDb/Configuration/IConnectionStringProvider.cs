namespace EntityTestDb.Configuration
{
    public interface IConnectionStringProvider
    {
        string GetConnectionString(string testName);
    }
}
