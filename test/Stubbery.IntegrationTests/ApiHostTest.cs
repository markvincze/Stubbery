using System;
using Xunit;

namespace Stubbery.IntegrationTests
{
    public class ApiHostTest
    {
        [Fact]
        public void Ctor_StartupNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ApiHost(null));
        }
    }
}