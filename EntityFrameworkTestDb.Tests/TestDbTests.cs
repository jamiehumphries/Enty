namespace EntityFrameworkTestDb.Tests
{
    using EntityFrameworkTestDb.Tests.TestHelpers;
    using EntityFrameworkTestDb.Tests.TestHelpers.Models;
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.IO;

    public class TestDbTests
    {
        private TestDb<TestDbContext> testDb;
        private string connectionString;

        [SetUp]
        public void SetUp()
        {
            var dbFileName = String.Concat(TestContext.CurrentContext.Test.FullName.Split(Path.GetInvalidFileNameChars())) + DateTime.Now.ToString("yyyyMMddHHmmssf");
            connectionString = String.Format("Data Source={0}.sdf", dbFileName);
            testDb = new TestDb<TestDbContext>(connectionString, s => new TestDbContext(s));
        }

        [TearDown]
        public void TearDown()
        {
            testDb.Dispose();
        }

        [Test]
        public void Disposing_deletes_database()
        {
            // Given
            using (var context = GetDbContext())
            {
                context.Database.CreateIfNotExists();
            }

            // When
            testDb.Dispose();

            // Then
            using (var context = GetDbContext())
            {
                context.Database.Exists().Should().BeFalse();
            }
        }

        [Test]
        public void Can_get_all_entities_of_a_type()
        {
            // Given
            var people = new[] { new Person { Name = "Tom" }, new Person { Name = "Dick" }, new Person { Name = "Harry" } };
            using (var context = GetDbContext())
            {
                context.People.AddRange(people);
                context.SaveChanges();
            }

            // When
            var allPeople = testDb.GetAll<Person>();

            // Then
            allPeople.Should().Equal(people, (p1, p2) => p1.Name == p2.Name);
        }

        private TestDbContext GetDbContext()
        {
            return new TestDbContext(connectionString);
        }
    }
}
