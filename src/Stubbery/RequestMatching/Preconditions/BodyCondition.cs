using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Stubbery.RequestMatching.Preconditions
{
    internal class BodyCondition : IPrecondition
    {
        private readonly Func<Stream, bool> condition;

        public BodyCondition(Func<Stream, bool> condition)
        {
            this.condition = condition;
        }

        public bool Match(HttpContext context)
        {
            return condition(context.Request.Body);
        }
    }
}