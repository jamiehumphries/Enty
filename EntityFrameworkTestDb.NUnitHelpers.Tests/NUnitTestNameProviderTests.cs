namespace EntityFrameworkTestDb.NUnitHelpers.Tests
{
    using FluentAssertions;
    using NUnit.Framework;

    public class NUnitTestNameProviderTests
    {
        private readonly NUnitTestNameProvider provider = new NUnitTestNameProvider();

        [Test]
        public void Current_test_name_is_returned_in_full()
        {
            provider.CurrentTestName.Should().Be("Current_test_name_is_returned_in_full");
        }
    }
}
