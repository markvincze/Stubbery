using Microsoft.AspNetCore.Http;

namespace Stubbery.RequestMatching.Preconditions
{
    internal interface IPrecondition
    {
        bool Match(HttpContext context);
    }
}