using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Stubbery
{
    internal interface IApiStubRequestHandler
    {
        Task HandleAsync(HttpContext httpContext);
    }
}