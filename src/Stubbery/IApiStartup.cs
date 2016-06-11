using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Stubbery
{
    public interface IApiStartup
    {
        void Configure(IApplicationBuilder app);

        void ConfigureServices(IServiceCollection services);
    }
}