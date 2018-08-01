using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace Stubbery.RequestMatching.Preconditions
{
    public class BodyCondition : IPrecondition
    {
        private readonly Func<string, bool> _condition;

        public BodyCondition(Func<string, bool> condition)
        {
            _condition = condition;
        }

        public bool Match(HttpContext context)
        {
            if (context.Request.Body == null)
            {
                return false;
            }

            context.Request.EnableRewind();
            var reader = new StreamReader(context.Request.Body);
            var body = reader.ReadToEnd();
            context.Request.Body.Seek(0, SeekOrigin.Begin);
            return _condition(body);
        }
    }
}