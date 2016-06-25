using Microsoft.AspNetCore.Http;

namespace Stubbery.RequestMatching
{
    public interface IPrecondition
    {
        bool Match(HttpContext context);
    }
}