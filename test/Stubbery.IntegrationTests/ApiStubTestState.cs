using System;
using Xunit;

namespace Stubbery.IntegrationTests
{
    public partial class ApiStubTest
    {
        [Fact]
        public void Start_StartTwice_Exception()
        {
            var sut = new ApiStub();

            sut.Start();

            Assert.Throws<InvalidOperationException>(() => sut.Start());
        }

        [Fact]
        public void Address_NotStarted_Exception()
        {
            var sut = new ApiStub();

            Assert.Throws<InvalidOperationException>(() => sut.Address);
        }
    }
}
