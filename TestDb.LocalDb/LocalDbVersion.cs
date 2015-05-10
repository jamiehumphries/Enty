namespace EntityTestDb.LocalDb
{
    using System;

    // ReSharper disable InconsistentNaming
    public enum LocalDbVersion
    {
        V11_0,
        ProjectsV12
    }

    public static class LocalDbVersionExtensions
    {
        public static string Name(this LocalDbVersion version)
        {
            switch (version)
            {
                case LocalDbVersion.V11_0:
                    return "v11.0";
                case LocalDbVersion.ProjectsV12:
                    return "ProjectsV12";
                default:
                    throw new ArgumentOutOfRangeException("version", version, null);
            }
        }
    }
}
