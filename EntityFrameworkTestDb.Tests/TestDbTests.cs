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
        public void Navigation_properties_are_loaded_on_retrieved_entities()
        {
            // Given
            var wallace = new Person { Name = "Wallace" };
            var gromit = new Dog { Name = "Gromit", Owner = wallace };
            using (var context = GetDbContext())
            {
                context.People.Add(wallace);
                context.Dogs.Add(gromit);
                context.SaveChanges();
            }

            // When
            var allPeople = testDb.GetAll<Person>();
            var allDogs = testDb.GetAll<Dog>();

            // Then
            allPeople.Should().Contain(p => p.Name == "Wallace").Which.Dogs.Should().HaveCount(1);
            allDogs.Should().Contain(p => p.Name == "Gromit").Which.Owner.Name.Should().Be("Wallace");
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

        [Test]
        public void Can_seed_entities_of_mixed_types()
        {
            // Given
            var jim = new Person { Name = "Jim" };
            var rover = new Dog { Name = "Rover" };

            // When
            testDb.Seed(jim, rover);

            // Then
            using (var context = GetDbContext())
            {
                context.People.Should().Contain(p => p.Name == "Jim");
                context.Dogs.Should().Contain(d => d.Name == "Rover");
            }
        }

        [Test]
        public void Seeded_entities_are_all_given_ids()
        {
            // Given
            var tom = new Person { Name = "Tom" };
            var jerry = new Person { Name = "Jerry" };

            // When
            testDb.Seed(tom, jerry);

            // Then
            tom.Id.Should().NotBe(0);
            jerry.Id.Should().NotBe(0);
        }

        [Test]
        public void Entities_seeded_in_collection_are_all_given_ids()
        {
            // Given
            var fred = new Person { Name = "Fred" };
            var wilma = new Person { Name = "Wilma" };
            var pebbles = new Person { Name = "Pebbles" };
            var people = new List<Person> { fred, wilma, pebbles };

            // When
            testDb.SeedMany(people);

            // Then
            people.Should().OnlyContain(p => p.Id != 0);
        }

        [Test]
        public void Seeded_mixed_type_entities_are_given_ids()
        {
            // Given
            var darren = new Person { Name = "Darren" };
            var patch = new Dog { Name = "Patch" };

            // When
            testDb.Seed(darren, patch);

            // Then
            darren.Id.Should().NotBe(0);
            patch.Id.Should().NotBe(0);
        }

        [Test]
        public void Can_seed_entities_with_foreign_key_to_existing_entity_by_id()
        {
            // Given
            var jon = new Person { Name = "Jon" };
            using (var context = GetDbContext())
            {
                context.People.Add(jon);
                context.SaveChanges();
            }
            var odie = new Dog { Name = "Odie", OwnerId = jon.Id };

            // When
            testDb.Seed(odie);

            // Then
            using (var context = GetDbContext())
            {
                context.Dogs.Should().Contain(d => d.Name == "Odie").Which.OwnerId.Should().Be(jon.Id).And.NotBe(0);
            }
        }

        [Test]
        public void Can_seed_entities_with_foreign_key_to_previously_seeded_object()
        {
            // Given
            var george = new Person { Name = "George" };
            var astro = new Dog { Name = "Astro" };

            // When
            testDb.Seed(george);
            astro.Owner = george;
            testDb.Seed(astro);

            // Then
            using (var context = GetDbContext())
            {
                context.Dogs.Should().Contain(d => d.Name == "Astro").Which.OwnerId.Should().Be(george.Id).And.NotBe(0);
            }
        }

        [Test]
        public void Can_seed_entity_as_parent_of_newly_seeded_entity()
        {
            // Given
            var mickey = new Person { Name = "Mickey" };
            var pluto = new Dog { Name = "Pluto", Owner = mickey };

            // When
            testDb.Seed(pluto);

            // Then
            using (var context = GetDbContext())
            {
                context.People.Should().Contain(p => p.Name == "Mickey");
                context.Dogs.Should().Contain(d => d.Name == "Pluto");
            }
        }

        [Test]
        public void Can_seed_entity_as_parent_of_multiple_newly_seeded_entities()
        {
            // Given
            var shaggy = new Person { Name = "Shaggy" };
            var scoobyDoo = new Dog { Name = "Scooby Doo", Owner = shaggy };
            var scrappyDoo = new Dog { Name = "Scrappy Doo", Owner = shaggy };

            // When
            testDb.Seed(scoobyDoo, scrappyDoo);

            // Then
            using (var context = GetDbContext())
            {
                context.People.Should().ContainSingle(p => p.Name == "Shaggy");
                context.Dogs.Should().Contain(d => d.Name == "Scooby Doo")
                       .And.Contain(d => d.Name == "Scrappy Doo");
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
