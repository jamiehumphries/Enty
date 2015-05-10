namespace EntityTestDb.Tests_LocalDb
{
    using EntityTestDb.Configuration;
    using EntityTestDb.LocalDbHelpers;
    using EntityTestDb.NUnitHelpers;
    using EntityTestDb.Tests;
    using EntityTestDb.Tests.TestHelpers;
    using NUnit.Framework;

    [TestFixture(typeof(NUnitLocalDbV11Configuration))]
    [TestFixture(typeof(NUnitLocalDbV12Configuration))]
    public class TestDbTests_LocalDb<T> : TestDbTests<T> where T : ITestDbConfiguration<TestDbContext>
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public override void Can_get_blank_db_context_from_test_db()
        {
            base.Can_get_blank_db_context_from_test_db();
        }

        [Test]
        public override void Can_create_database()
        {
            base.Can_create_database();
        }

        [Test]
        public override void Disposing_deletes_database()
        {
            base.Disposing_deletes_database();
        }

        [Test]
        public override void Can_get_all_entities_of_a_type()
        {
            base.Can_get_all_entities_of_a_type();
        }

        [Test]
        public override void Navigation_properties_are_loaded_on_retrieved_entities()
        {
            base.Navigation_properties_are_loaded_on_retrieved_entities();
        }

        [Test]
        public override void Can_seed_single_entity()
        {
            base.Can_seed_single_entity();
        }

        [Test]
        public override void Can_seed_multiple_entities()
        {
            base.Can_seed_multiple_entities();
        }

        [Test]
        public override void Can_seed_multiple_entities_in_a_collection()
        {
            base.Can_seed_multiple_entities_in_a_collection();
        }

        [Test]
        public override void Can_seed_entities_of_mixed_types()
        {
            base.Can_seed_entities_of_mixed_types();
        }

        [Test]
        public override void Seeded_entities_are_all_given_ids()
        {
            base.Seeded_entities_are_all_given_ids();
        }

        [Test]
        public override void Entities_seeded_in_collection_are_all_given_ids()
        {
            base.Entities_seeded_in_collection_are_all_given_ids();
        }

        [Test]
        public override void Seeded_mixed_type_entities_are_given_ids()
        {
            base.Seeded_mixed_type_entities_are_given_ids();
        }

        [Test]
        public override void Can_seed_entities_with_foreign_key_to_existing_entity_by_id()
        {
            base.Can_seed_entities_with_foreign_key_to_existing_entity_by_id();
        }

        [Test]
        public override void Can_seed_entities_with_foreign_key_to_previously_seeded_object()
        {
            base.Can_seed_entities_with_foreign_key_to_previously_seeded_object();
        }

        [Test]
        public override void Can_seed_entity_as_parent_of_newly_seeded_entity()
        {
            base.Can_seed_entity_as_parent_of_newly_seeded_entity();
        }

        [Test]
        public override void Can_seed_entity_as_parent_of_multiple_newly_seeded_entities()
        {
            base.Can_seed_entity_as_parent_of_multiple_newly_seeded_entities();
        }

        [Test]
        public override void Single_seeded_entities_are_returned()
        {
            base.Single_seeded_entities_are_returned();
        }

        [Test]
        public override void Multiple_seeded_entities_are_returned()
        {
            base.Multiple_seeded_entities_are_returned();
        }

        [Test]
        public override void Collection_of_seeded_entities_are_returned()
        {
            base.Collection_of_seeded_entities_are_returned();
        }
    }

    public class NUnitLocalDbV11Configuration : NUnitLocalDbConfiguration
    {
        public NUnitLocalDbV11Configuration() : base(LocalDbVersion.V11_0) {}
    }

    public class NUnitLocalDbV12Configuration : NUnitLocalDbConfiguration
    {
        public NUnitLocalDbV12Configuration() : base(LocalDbVersion.ProjectsV12) {}
    }

    public class NUnitLocalDbConfiguration : ITestDbConfiguration<TestDbContext>
    {
        public NUnitLocalDbConfiguration(LocalDbVersion version)
        {
            TestIdentityProvider = new NUnitTestIdentityProvider();
            ConnectionStringProvider = new LocalDbConnectionStringProvider(version);
            TestDbContextFactory = new TestDbContextFactory<TestDbContext>();
        }

        public ITestIdentityProvider TestIdentityProvider { get; private set; }
        public IConnectionStringProvider ConnectionStringProvider { get; private set; }
        public ITestDbContextFactory<TestDbContext> TestDbContextFactory { get; private set; }
    }
}
