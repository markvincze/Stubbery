using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Stubbery.RequestMatching
{
    internal class SetupConditionParameters
    {
        public IList<string> Methods { get; } = new List<string>();

        public IList<Func<IHeaderDictionary, bool>> HeaderConditions { get; } = new List<Func<IHeaderDictionary, bool>>();

        public IList<Func<string, bool>> ContentTypeConditions { get; } = new List<Func<string, bool>>();

        public IList<Func<string, bool>> AcceptConditions { get; } = new List<Func<string, bool>>();

        public IList<string> Routes { get; } = new List<string>();
    }
}