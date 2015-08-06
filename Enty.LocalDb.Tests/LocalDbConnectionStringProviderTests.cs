namespace Enty.LocalDb.Tests
{
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class LocalDbConnectionStringProviderTests
    {
        private LocalDbConnectionStringProvider provider;

        [SetUp]
        public void SetUp()
        {
            provider = new LocalDbConnectionStringProvider();
        }

        [Test]
        public void Data_source_for_connection_string_is_local_db()
        {
            // When
            var connectionString = provider.GetConnectionString("Some_unit_test");

            // Then
            connectionString.DataSource().Should().Be(@"(LocalDb)\v11.0");
        }

        [Test]
        public void Database_file_should_have_mdf_extension()
        {
            // When
            var connectionString = provider.GetConnectionString("Some_unit_test");

            // Then
            connectionString.AttachDbFilename().Should().EndWith(".mdf");
        }

        [TestCase("Some_test_name", "Other_test_name")] // Different test names.
        [TestCase("Some_test_name", "Some_test_name")] // Same test name.
        public void Connection_strings_are_unique(string testIdentity1, string testIdentity2)
        {
            // When
            var connectionString1 = provider.GetConnectionString(testIdentity1);
            var connectionString2 = provider.GetConnectionString(testIdentity2);
            var connectionStrings = new[] { connectionString1, connectionString2 };

            // Then
            connectionStrings.Select(c => c.AttachDbFilename()).Should().OnlyHaveUniqueItems();
            connectionStrings.Select(c => c.InitialCatalog()).Should().OnlyHaveUniqueItems();
        }

        [TestCase("Some_unit_test")]
        [TestCase("Test_name_with_invalid_characters<string>(\"hello\")")]
        [TestCase("Test_name_that_is_very_long_lorem_ipsum_dolor_sit_amet_consectetuer_adipiscing_elit_aenean_commodo_ligula_eget_dolor_aenean_massa_cum_sociis_natoque_penatibus_et_magnis_dis_parturient_montes_nascetur_ridiculus_mus_donec_quam_felis_ultricies_nec_pellentesque_eu_pretium_quis_sem")]
        public void Connection_strings_data_source_is_valid_file_path(string testIdentity)
        {
            // When
            var connectionString = provider.GetConnectionString(testIdentity);

            // Then
            // ReSharper disable once ObjectCreationAsStatement
            ((Action)(() => new FileInfo(connectionString.AttachDbFilename()))).ShouldNotThrow();
        }
    }

    internal static class ConnectionStringExtensions
    {
        internal static string DataSource(this string connectionString)
        {
            return connectionString.GetPart("Data Source");
        }

        internal static string AttachDbFilename(this string connectionString)
        {
            return connectionString.GetPart("AttachDbFilename");
        }

        internal static string InitialCatalog(this string connectionString)
        {
            return connectionString.GetPart("Initial Catalog");
        }

        private static string GetPart(this string connectionString, string part)
        {
            var pattern = String.Format(@"(?<={0}=)[^;]*", part);
            return Regex.Match(connectionString, pattern).ToString();
        }
    }
}
