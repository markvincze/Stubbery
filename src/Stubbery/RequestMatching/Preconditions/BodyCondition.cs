using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Stubbery.RequestMatching.Preconditions
{
    internal class BodyCondition : IPrecondition
    {
        private readonly Func<string, bool> condition;

        public BodyCondition(Func<string, bool> condition)
        {
            this.condition = condition;
        }

        public async Task<bool> Match(HttpContext context)
        {
            if (context.Request.Body == null)
            {
                return false;
            }

            context.Request.EnableBuffering();

            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true);

            var body = await reader.ReadToEndAsync();
            context.Request.Body.Seek(0, SeekOrigin.Begin);
            return condition(body);
        }
    }
}