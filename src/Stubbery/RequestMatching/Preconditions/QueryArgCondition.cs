using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Stubbery.RequestMatching.Preconditions
{
    internal class QueryArgCondition : IPrecondition
    {
        private readonly string argName;
        private readonly string argValue;

        public QueryArgCondition(string argName, string argValue)
        {
            this.argName = argName;
            this.argValue = argValue;
        }

        public async Task<bool> Match(HttpContext context) => context.Request.Query[argName].Contains(argValue);
    }
}
