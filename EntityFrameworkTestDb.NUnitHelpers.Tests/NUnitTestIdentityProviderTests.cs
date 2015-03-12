﻿namespace EntityFrameworkTestDb.NUnitHelpers.Tests
{
    using FluentAssertions;
    using NUnit.Framework;

    public class NUnitTestIdentityProviderTests
    {
        private readonly NUnitTestIdentityProvider provider = new NUnitTestIdentityProvider();

        [Test]
        public void Current_test_name_is_returned_as_identity()
        {
            provider.GetTestIdentity().Should().Be("Current_test_name_is_returned_as_identity");
        }
    }
}
