using Microsoft.AspNetCore.Http;

namespace Stubbery.RequestMatching.Preconditions
{
    public class MethodPrecondition : IPrecondition
    {
        private readonly string method;

        public MethodPrecondition(string method)
        {
            this.method = method;
        }

        public bool Match(HttpContext context)
        {
            return context.Request.Method == method;
        }
    }
}