using System;
using System.Threading.Tasks;
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

        public async Task<bool> Match(HttpContext context) => condition(context.Request.ContentType);
    }
}