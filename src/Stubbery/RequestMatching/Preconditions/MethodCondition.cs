using Microsoft.AspNetCore.Http;

namespace Stubbery.RequestMatching.Preconditions
{
    internal class MethodCondition : IPrecondition
    {
        private readonly string method;

        public MethodCondition(string method)
        {
            this.method = method;
        }

        public bool Match(HttpContext context)
        {
            return context.Request.Method == method;
        }
    }
}