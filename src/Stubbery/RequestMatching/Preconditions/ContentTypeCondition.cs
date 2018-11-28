using System;
using Microsoft.AspNetCore.Http;

namespace Stubbery.RequestMatching.Preconditions
{
    internal class ContentTypeCondition : IPrecondition
    {
        private readonly Func<string, bool> condition;

        public ContentTypeCondition(Func<string, bool> condition)
        {
            this.condition = condition;
        }

        public bool Match(HttpContext context) => condition(context.Request.ContentType);
    }
}