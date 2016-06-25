using System;
using Microsoft.AspNetCore.Http;

namespace Stubbery.RequestMatching.Preconditions
{
    public class HeaderCondition : IPrecondition
    {
        private readonly Func<IHeaderDictionary, bool> condition;

        public HeaderCondition(Func<IHeaderDictionary, bool> condition)
        {
            this.condition = condition;
        }

        public bool Match(HttpContext context)
        {
            return condition(context.Request.Headers);
        }
    }
}