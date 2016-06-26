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
            if (values == null)
            {
                this.values = new Dictionary<string, string>();
            }
            else
            {
                this.values = values.ToDictionary(v => v.Key, v => v.Value.ToString());
            }
        }

        internal DynamicValues(IEnumerable<KeyValuePair<string, object>> values)
        {
            if (values == null)
            {
                this.values = new Dictionary<string, string>();
            }
            else
            {
                this.values = values.ToDictionary(v => v.Key, v => v.Value.ToString());
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            throw new InvalidOperationException("You cannot modify the argument values.");
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string value;
            var found = values.TryGetValue(binder.Name, out value);
            result = value;
            return found;
        }
    }
}