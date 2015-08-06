namespace Enty.Configuration
{
    public interface IConnectionStringProvider
    {
        string GetConnectionString(string testIdentity);
    }
}
