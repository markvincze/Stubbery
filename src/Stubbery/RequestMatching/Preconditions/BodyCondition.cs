using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace Stubbery.RequestMatching.Preconditions
{
    internal class BodyCondition : IPrecondition
    {
        private readonly Func<string, bool> condition;

        public BodyCondition(Func<string, bool> condition)
        {
            this.condition = condition;
        }

        public bool Match(HttpContext context)
        {
            if (context.Request.Body == null)
            {
                return false;
            }

            context.Request.EnableRewind();
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                var body = reader.ReadToEnd();
                context.Request.Body.Seek(0, SeekOrigin.Begin);
                return condition(body);    
            }
        }
    }
}