namespace Enty.Tests
{
    using Enty.Configuration;
    using Enty.Tests.TestHelpers;
    using Enty.Tests.TestHelpers.Models;
    using FluentAssertions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    // Run via derived test projects targetting specific database providers.
    public class TestDbTests<TConfig> where TConfig : ITestDbConfiguration<TestDbContext>
    {
        private TestDb<TestDbContext> testDb;

        public virtual void SetUp()
        {
            testDb = TestDb.Create(Activator.CreateInstance<TConfig>());
        }

        public virtual void TearDown()
        {
            testDb.Dispose();
        }

        public virtual void Can_get_blank_db_context_from_test_db()
        {
            testDb.GetDbContext().Should().NotBeNull();
        }

        public virtual void Can_create_database()
        {
            // When
            testDb.CreateIfNotExists();

            // Then
            using (var context = testDb.GetDbContext())
            {
                context.Database.Exists().Should().BeTrue();
            }
        }

        public virtual void Disposing_deletes_database()
        {
            // Given
            using (var context = testDb.GetDbContext())
            {
                context.Database.CreateIfNotExists();
            }

            // When
            testDb.Dispose();

            // Then
            using (var context = testDb.GetDbContext())
            {
                context.Database.Exists().Should().BeFalse();
            }
        }

        public virtual void Can_get_all_entities_of_a_type()
        {
            // Given
            var tom = new Person { Name = "Tom" };
            var dick = new Person { Name = "Dick" };
            var harry = new Person { Name = "Harry" };
            var people = new[] { tom, dick, harry };
            using (var context = testDb.GetDbContext())
            {
                context.People.AddRange(people);
                context.SaveChanges();
            }

            // When
            var allPeople = testDb.GetAll<Person>();

            // Then
            allPeople.Should().Equal(people, MatchedByName);
        }

        public virtual void Navigation_properties_are_loaded_on_retrieved_entities()
        {
            // Given
            var wallace = new Person { Name = "Wallace" };
            var gromit = new Dog { Name = "Gromit", Owner = wallace };
            using (var context = testDb.GetDbContext())
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
            allPeople.Should().OnlyContain(p => p.Dogs.All(d => d.Owner == p)); // Ensure cyclic references are loaded correctly.
        }

        public virtual void Can_seed_single_entity()
        {
            // Given
            var jamie = new Person { Name = "Jamie" };

            // When
            testDb.Seed(jamie);

            // Then
            using (var context = testDb.GetDbContext())
            {
                context.People.Should().Contain(p => p.Name == "Jamie");
            }
        }

        public virtual void Can_seed_multiple_entities()
        {
            // Given
            var huey = new Person { Name = "Huey" };
            var dewey = new Person { Name = "Dewey" };
            var louie = new Person { Name = "Louie" };

            // When
            testDb.Seed(huey, dewey, louie);

            // Then
            using (var context = testDb.GetDbContext())
            {
                context.People.Should().Equal(new[] { huey, dewey, louie }, MatchedByName);
            }
        }

        public virtual void Can_seed_multiple_entities_in_a_collection()
        {
            // Given
            var matt = new Person { Name = "Matt" };
            var jeff = new Person { Name = "Jeff" };
            var people = new List<Person> { matt, jeff };

            // When
            testDb.SeedMany(people);

            // Then
            using (var context = testDb.GetDbContext())
            {
                context.People.Should().Equal(people, MatchedByName);
            }
        }

        public virtual void Can_seed_entities_of_mixed_types()
        {
            // Given
            var jim = new Person { Name = "Jim" };
            var rover = new Dog { Name = "Rover" };

            // When
            testDb.Seed(jim, rover);

            // Then
            using (var context = testDb.GetDbContext())
            {
                context.People.Should().Contain(p => p.Name == "Jim");
                context.Dogs.Should().Contain(d => d.Name == "Rover");
            }
        }

        public virtual void Seeded_entities_are_all_given_ids()
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

        public virtual void Entities_seeded_in_collection_are_all_given_ids()
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

        public virtual void Seeded_mixed_type_entities_are_given_ids()
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

        public virtual void Can_seed_entities_with_foreign_key_to_existing_entity_by_id()
        {
            // Given
            var jon = new Person { Name = "Jon" };
            using (var context = testDb.GetDbContext())
            {
                context.People.Add(jon);
                context.SaveChanges();
            }
            var odie = new Dog { Name = "Odie", OwnerId = jon.Id };

            // When
            testDb.Seed(odie);

            // Then
            using (var context = testDb.GetDbContext())
            {
                context.Dogs.Should().Contain(d => d.Name == "Odie").Which.OwnerId.Should().Be(jon.Id).And.NotBe(0);
            }
        }

        public virtual void Can_seed_entities_with_foreign_key_to_previously_seeded_object()
        {
            // Given
            var george = new Person { Name = "George" };
            var astro = new Dog { Name = "Astro" };

            // When
            testDb.Seed(george);
            astro.Owner = george;
            testDb.Seed(astro);

            // Then
            using (var context = testDb.GetDbContext())
            {
                context.Dogs.Should().Contain(d => d.Name == "Astro").Which.OwnerId.Should().Be(george.Id).And.NotBe(0);
            }
        }

        public virtual void Can_seed_entity_as_parent_of_newly_seeded_entity()
        {
            // Given
            var mickey = new Person { Name = "Mickey" };
            var pluto = new Dog { Name = "Pluto", Owner = mickey };

            // When
            testDb.Seed(pluto);

            // Then
            using (var context = testDb.GetDbContext())
            {
                context.People.Should().Contain(p => p.Name == "Mickey");
                context.Dogs.Should().Contain(d => d.Name == "Pluto");
            }
        }

        public virtual void Can_seed_entity_as_parent_of_multiple_newly_seeded_entities()
        {
            // Given
            var shaggy = new Person { Name = "Shaggy" };
            var scoobyDoo = new Dog { Name = "Scooby Doo", Owner = shaggy };
            var scrappyDoo = new Dog { Name = "Scrappy Doo", Owner = shaggy };

            // When
            testDb.Seed(scoobyDoo, scrappyDoo);

            // Then
            using (var context = testDb.GetDbContext())
            {
                context.People.Should().ContainSingle(p => p.Name == "Shaggy");
                context.Dogs.Should().Contain(d => d.Name == "Scooby Doo")
                       .And.Contain(d => d.Name == "Scrappy Doo");
            }
        }

        public virtual void Single_seeded_entities_are_returned()
        {
            // Given
            var jack = new Person { Name = "Jack" };

            // When
            var seededEntity = testDb.Seed(jack);

            // Then
            seededEntity.Should().Be(jack);
        }

        public virtual void Multiple_seeded_entities_are_returned()
        {
            // Given
            var dan = new Person { Name = "Dan" };
            var pete = new Person { Name = "Pete" };
            var phil = new Person { Name = "Phil" };

            // When
            var seededEntities = testDb.Seed(dan, pete, phil);

            // Then
            seededEntities.Should().BeEquivalentTo(dan, pete, phil);
        }

        public virtual void Collection_of_seeded_entities_are_returned()
        {
            // Given
            var otacon = new Person { Name = "Otacon" };
            var snake = new Person { Name = "Snake" };
            var people = new List<Person> { otacon, snake };

            // When
            var seededEntities = testDb.SeedMany(people);

            // Then
            seededEntities.Should().BeEquivalentTo(people);
        }

        private static bool MatchedByName(Person p1, Person p2)
        {
            return p1.Name == p2.Name;
        }
    }
}
