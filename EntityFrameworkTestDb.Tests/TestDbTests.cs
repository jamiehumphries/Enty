namespace EntityFrameworkTestDb.Tests
{
    using EntityFrameworkTestDb.Configuration;
    using EntityFrameworkTestDb.NUnitHelpers;
    using EntityFrameworkTestDb.SqlServerCompactHelpers;
    using EntityFrameworkTestDb.Tests.TestHelpers;
    using EntityFrameworkTestDb.Tests.TestHelpers.Models;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Collections.Generic;

    public class TestDbTests
    {
        private TestDb testDb;
        private TestDbConfiguration configuration;

        [SetUp]
        public void SetUp()
        {
            configuration = new TestDbConfiguration
                            {
                                TestNameProvider = new NUnitTestNameProvider(),
                                ConnectionStringProvider = new SqlServerCompactConnectionStringProvider(),
                                ContextFactory = new ContextFactory<TestDbContext>()
                            };
            testDb = new TestDb(configuration);
        }

        [TearDown]
        public void TearDown()
        {
            testDb.Dispose();
        }

        [Test]
        public void Can_create_database()
        {
            // When
            testDb.Create();

            // Then
            using (var context = GetDbContext())
            {
                context.Database.Exists().Should().BeTrue();
            }
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
            var tom = new Person { Name = "Tom" };
            var dick = new Person { Name = "Dick" };
            var harry = new Person { Name = "Harry" };
            var people = new[] { tom, dick, harry };
            using (var context = GetDbContext())
            {
                context.People.AddRange(people);
                context.SaveChanges();
            }

            // When
            var allPeople = testDb.GetAll<Person>();

            // Then
            allPeople.Should().Equal(people, MatchedByName);
        }

        [Test]
        public void Can_seed_single_entity()
        {
            // Given
            var jamie = new Person { Name = "Jamie" };

            // When
            testDb.Seed(jamie);

            // Then
            using (var context = GetDbContext())
            {
                context.People.Should().Contain(p => p.Name == "Jamie");
            }
        }

        [Test]
        public void Can_seed_multiple_entities()
        {
            // Given
            var huey = new Person { Name = "Huey" };
            var dewey = new Person { Name = "Dewey" };
            var louie = new Person { Name = "Louie" };

            // When
            testDb.Seed(huey, dewey, louie);

            // Then
            using (var context = GetDbContext())
            {
                context.People.Should().Equal(new[] { huey, dewey, louie }, MatchedByName);
            }
        }

        [Test]
        public void Can_seed_multiple_entities_in_a_collection()
        {
            // Given
            var matt = new Person { Name = "Matt" };
            var jeff = new Person { Name = "Jeff" };
            var people = new List<Person> { matt, jeff };

            // When
            testDb.SeedMany(people);

            // Then
            using (var context = GetDbContext())
            {
                context.People.Should().Equal(people, MatchedByName);
            }
        }

        private TestDbContext GetDbContext()
        {
            return new TestDbContext(testDb.ConnectionString);
        }

        private bool MatchedByName(Person p1, Person p2)
        {
            return p1.Name == p2.Name;
        }
    }
}
