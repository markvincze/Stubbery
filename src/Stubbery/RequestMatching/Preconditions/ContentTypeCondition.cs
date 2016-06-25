using System;
using Microsoft.AspNetCore.Http;

namespace Stubbery.RequestMatching.Preconditions
{
    public class ContentTypeCondition : IPrecondition
    {
        private readonly Func<string, bool> condition;

        public ContentTypeCondition(Func<string, bool> condition)
        {
            this.condition = condition;
        }

        public bool Match(HttpContext context)
        {
            return condition(context.Request.ContentType);
        }
    }
}