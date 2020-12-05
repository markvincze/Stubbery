using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Stubbery.IntegrationTests
{
    public class ApiStubConcurrencyTests
    {
        [Fact]
        public async Task CreateApiStubInstancesInParallel_ShouldNotCausePortCollision()
        {
            Task[] stubTasks = Enumerable.Range(1, 1000)
                .Select(it =>
                {
                    return Task.Run(async () =>
                    {
                        ApiStub stub = new ApiStub();

                        stub.Start();

                        await Task.Delay(100);
                    });
                }).ToArray();

            await Task.WhenAll(stubTasks);
        }
    }
}
