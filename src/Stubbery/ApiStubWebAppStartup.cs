using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.DependencyInjection;

namespace Stubbery
{
    internal class ApiStubWebAppStartup : IApiStartup
    {
        //private readonly ICollection<EndpointStubConfig> configuredEndpoints;

        private readonly IApiStubRequestHandler apiStubRequestHandler;

        public ApiStubWebAppStartup(IApiStubRequestHandler apiStubRequestHandler)
        {
            //this.configuredEndpoints = configuredEndpoints;
            this.apiStubRequestHandler = apiStubRequestHandler;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddRouting();
        }

        public void Configure(IApplicationBuilder app)
        {
            try
            {
                app.Run(async httpContext =>
                {
                    await apiStubRequestHandler.HandleAsync(httpContext);
                });

                return;

                app.Run(async ctx =>
                {
                    var temp = TemplateParser.Parse("testget/{arg1}/part/{arg2}");
                    var temp2 = TemplateParser.Parse("notmatching/{param}/route");

                    var matcher = new TemplateMatcher(temp, new RouteValueDictionary());
                    var matcher2 = new TemplateMatcher(temp, new RouteValueDictionary());

                    var values = matcher.Match(ctx.Request.Path);
                    var values2 = matcher2.Match(ctx.Request.Path);

                    await Task.Delay(1);
                });

                return;

                RouteBuilder builder = new RouteBuilder(app);

                builder.MapVerb(
                    HttpMethod.Get.Method,
                    "notmatching/{param}/route",
                    async context =>
                    {
                        await context.Response.WriteAsync("alma1");
                    });

                builder.MapVerb(
                    HttpMethod.Get.Method,
                    "testget/{arg1}/part/{arg2}",
                    async context =>
                    {
                        await context.Response.WriteAsync("alma2");
                    });

                app.UseRouter(builder.Build());

                return;

                app.Run(async context =>
                {
                    try
                    {
                        var template = TemplateParser.Parse("testget/{arg1}/part/{arg2}");

                        //template.

                        var route = new Route(
                            new RouteHandler(async c =>
                            {
                                Console.WriteLine("valami");
                                await Task.Delay(5);
                            }),
                            "testget/{arg1}/part/{arg2}",
                            defaults: null,
                            constraints: new RouteValueDictionary(new { httpMethod = new HttpMethodRouteConstraint(HttpMethod.Get.Method) }),
                            dataTokens: null,
                            inlineConstraintResolver: builder.ServiceProvider.GetRequiredService<IInlineConstraintResolver>());

                        var routeContext = new RouteContext(context);
                        await route.RouteAsync(routeContext);


                        await context.Response.WriteAsync("alma");
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                });

                return;

                //foreach (var endpoint in configuredEndpoints)
                //{
                //    builder.MapVerb(
                //        endpoint.Method.Method,
                //        endpoint.Route.TrimStart('/'),
                //        async context =>
                //        {
                //            var arguments = new RequestArguments(
                //                new DynamicValues(context.GetRouteData().Values),
                //                new DynamicValues(context.Request.Query),
                //                context.Request.Body);

                //            await context.Response.WriteAsync((string)endpoint.Response(context.Request, arguments));
                //        });
                //}

                //app.UseRouter(builder.Build());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}