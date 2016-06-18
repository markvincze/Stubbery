using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Stubbery
{
    internal interface IApiStartup
    {
        void Configure(IApplicationBuilder app);

        void ConfigureServices(IServiceCollection services);
    }
}