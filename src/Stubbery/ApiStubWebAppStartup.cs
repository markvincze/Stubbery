using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;

namespace Stubbery
{
    internal class ApiStubWebAppStartup : IApiStartup
    {
        private readonly IApiStubRequestHandler apiStubRequestHandler;
        private readonly Func<OutputFormatter> getDefaultOutputFormatter;

        public ApiStubWebAppStartup(IApiStubRequestHandler apiStubRequestHandler, Func<OutputFormatter> getDefaultOutputFormatter)
        {
            this.apiStubRequestHandler = apiStubRequestHandler;
            this.getDefaultOutputFormatter = getDefaultOutputFormatter;
        }

        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app)
        {
            app.Run(async httpContext =>
            {
                await apiStubRequestHandler.HandleAsync(httpContext, getDefaultOutputFormatter());
            });
        }
    }
}