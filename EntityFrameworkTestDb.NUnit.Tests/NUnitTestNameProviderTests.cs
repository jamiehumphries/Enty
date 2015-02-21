namespace EntityFrameworkTestDb.NUnit.Tests
{
    using FluentAssertions;
    using global::NUnit.Framework;

    public class NUnitTestNameProviderTests
    {
        [Test]
        public void Current_test_name_is_returned_in_full()
        {
            var provider = new NUnitTestNameProvider();
            provider.CurrentTestName.Should().Be("Current_test_name_is_returned_in_full");
        }
    }
}
