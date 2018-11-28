using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Stubbery.IntegrationTests
{
    public class HangingConnectionTest
    {
        private readonly HttpClient httpClient = new HttpClient();

        [Fact]
        public async Task MultiplePosts_OneMatch_DoesntHangConnectionOnLinux()
        {
            var largeText = new string('a', 6000);

            using (var sut = new ApiStub())
            {
                sut.Post("/", (req, args) => "testresponse1");

                sut.Start();

                for (var i = 0; i < 5; i++)
                {
                    var postContent = new StringContent(largeText, Encoding.UTF8, "text/plain");

                    var result = await httpClient.PostAsync(sut.Address, postContent);

                    Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                    var resultString = await result.Content.ReadAsStringAsync();

                    Assert.Equal("testresponse1", resultString);
                }
            }
        }
    }
}
