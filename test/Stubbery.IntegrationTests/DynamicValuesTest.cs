using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace Stubbery.IntegrationTests
{
    public class DynamicValuesTest
    {
        [Fact]
        public void Constructor_CalledWithNullStringStringValues_NoExceptionThrown()
        {
            // Act
            var sut = new DynamicValues(default(IEnumerable<KeyValuePair<string, StringValues>>));
        }

        [Fact]
        public void Constructor_CalledWithNullStringObject_NoExceptionThrown()
        {
            // Act
            var sut = new DynamicValues(default(IEnumerable<KeyValuePair<string, object>>));
        }

        [Fact]
        public void TrySetMember_Called_InvalidOperationException()
        {
            var sut = new DynamicValues(new Dictionary<string, object>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<InvalidOperationException>(() => sut.TrySetMember(null, null));
        }
    }
}
