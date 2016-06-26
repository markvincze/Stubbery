using System;
using Microsoft.AspNetCore.Http;

namespace Stubbery.RequestMatching.Preconditions
{
    internal class AcceptCondition : IPrecondition
    {
        private readonly Func<string, bool> condition;

        public AcceptCondition(Func<string, bool> condition)
        {
            this.condition = condition;
        }

        public bool Match(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Accept"))
            {
                return false;
            }

            var accept = context.Request.Headers["Accept"].ToString();

            return condition(accept);
        }
    }
}