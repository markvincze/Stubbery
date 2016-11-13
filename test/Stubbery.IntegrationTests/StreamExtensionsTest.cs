using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Stubbery.IntegrationTests
{
    public class StreamExtensionsTest
    {
        [Fact]
        public async Task ReadAsStringAsync_CalledOnNull_ArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => ((Stream) null).ReadAsStringAsync());
        }

        [Fact]
        public async Task ReadAsStringAsync_CalledOnStream_StreamContentReturned()
        {
            using (var ms = new MemoryStream())
            {
                var bytes = Encoding.UTF8.GetBytes("TestString");

                ms.Write(bytes, 0, bytes.Length);

                ms.Position = 0;

                // Act
                var result = await ms.ReadAsStringAsync();

                Assert.Equal("TestString", result);
            }
        }

        [Fact]
        public void ReadAsString_CalledOnNull_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ((Stream) null).ReadAsString());
        }

        [Fact]
        public void ReadAsString_CalledOnStream_StreamContentReturned()
        {
            using (var ms = new MemoryStream())
            {
                var bytes = Encoding.UTF8.GetBytes("TestString");

                ms.Write(bytes, 0, bytes.Length);

                ms.Position = 0;

                // Act
                var result = ms.ReadAsString();

                Assert.Equal("TestString", result);
            }
        }
    }
}