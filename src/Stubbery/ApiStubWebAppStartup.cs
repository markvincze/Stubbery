using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Stubbery
{
    internal class ApiStubWebAppStartup : IApiStartup
    {
        private readonly IApiStubRequestHandler apiStubRequestHandler;

        public ApiStubWebAppStartup(IApiStubRequestHandler apiStubRequestHandler)
        {
            this.apiStubRequestHandler = apiStubRequestHandler;
        }

        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app)
        {
            app.Run(async httpContext =>
            {
                await apiStubRequestHandler.HandleAsync(httpContext);
            });
        }
    }
}