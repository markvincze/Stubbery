using System.Threading.Tasks;
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

        public async Task<bool> Match(HttpContext context) => context.Request.Method == method;
    }
}