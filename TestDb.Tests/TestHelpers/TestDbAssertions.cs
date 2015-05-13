namespace EntityTestDb.Tests.TestHelpers
{
    using FluentAssertions;
    using FluentAssertions.Execution;
    using FluentAssertions.Primitives;

    public static class TestDbAssertionsExtensions
    {
        public static TestDbAssertions Should(this TestDb testDb)
        {
            return new TestDbAssertions(testDb);
        }
    }

    public class TestDbAssertions : ObjectAssertions
    {
        private readonly TestDb value;

        public TestDbAssertions(TestDb value) : base(value)
        {
            this.value = value;
        }

        public AndConstraint<TestDbAssertions> Exist()
        {
            Execute.Assertion
                   .ForCondition(value != null)
                   .FailWith("Expected database to exist but TestDb was null.");

            // ReSharper disable once PossibleNullReferenceException
            Execute.Assertion
                   .ForCondition(value.GetDbContext().Database.Exists())
                   .FailWith("Expected database to exist with connection string: {0}", value.ConnectionString);

            return new AndConstraint<TestDbAssertions>(this);
        }
    }
}
