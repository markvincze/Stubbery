using System.Linq;
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

        public bool Match(HttpContext context) => context.Request.Query[argName].Contains(argValue);
    }
}
