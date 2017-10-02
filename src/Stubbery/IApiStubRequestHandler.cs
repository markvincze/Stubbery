using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Stubbery
{
    internal interface IApiStubRequestHandler
    {
        Task HandleAsync(HttpContext httpContext, OutputFormatter defaultOutputFormatter);
    }
}