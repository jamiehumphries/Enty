namespace EntityTestDb.LocalDb
{
    using System;

    // ReSharper disable InconsistentNaming
    public enum LocalDbVersion
    {
        V11_0,
        MSSQLLocalDb,
        ProjectsV12
    }

    internal static class LocalDbVersionExtensions
    {
        internal static string ToVersionString(this LocalDbVersion version)
        {
            switch (version)
            {
                case LocalDbVersion.V11_0:
                    return "v11.0";
                case LocalDbVersion.MSSQLLocalDb:
                case LocalDbVersion.ProjectsV12:
                    return version.ToString();
                default:
                    throw new ArgumentOutOfRangeException("version", version, null);
            }
        }
    }
}
