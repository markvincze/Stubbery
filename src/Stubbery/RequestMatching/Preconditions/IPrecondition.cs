using Microsoft.AspNetCore.Http;

namespace Stubbery.RequestMatching.Preconditions
{
    public interface IPrecondition
    {
        bool Match(HttpContext context);
    }
}