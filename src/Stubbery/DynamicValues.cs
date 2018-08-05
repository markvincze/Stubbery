using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Microsoft.Extensions.Primitives;

namespace Stubbery
{
    /// <summary>
    /// Provides a convenient collection of request arguments, which are either coming from the route or the query string.
    /// </summary>
    internal class DynamicValues : DynamicObject
    {
        private readonly Dictionary<string, string> values;

        internal DynamicValues(IEnumerable<KeyValuePair<string, StringValues>> values)
        {
            this.values = values == null ?
                new Dictionary<string, string>() :
                values.ToDictionary(v => v.Key.TrimStart('?'), v => v.Value.ToString());
        }

        internal DynamicValues(IEnumerable<KeyValuePair<string, object>> values)
        {
            this.values = values == null ?
                new Dictionary<string, string>() :
                values.ToDictionary(v => v.Key, v => v.Value.ToString());
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            throw new InvalidOperationException("You cannot modify the argument values.");
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var found = values.TryGetValue(binder.Name, out var value);
            result = value;
            return found;
        }
    }
}