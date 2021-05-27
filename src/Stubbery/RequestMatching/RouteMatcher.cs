using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.AspNetCore.WebUtilities;

namespace Stubbery.RequestMatching
{
    internal class RouteMatcher : IRouteMatcher
    {
        public RouteValueDictionary Match(string routeTemplate, string requestPath, IQueryCollection query)
        {
            // The TemplateParser can only parse the route part, and not the query string.
            // If the template provided by the user also has a query string, we separate that and match it manually.
            var regex = new Regex(@"(.*)(\?[^{}]*$)");
            var match = regex.Match(routeTemplate);
            if (match.Success)
            {
                var queryString = match.Groups[2].Value;
                routeTemplate = match.Groups[1].Value;

                var queryInTemplate = QueryHelpers.ParseQuery(queryString);

                if (!query.All(arg => queryInTemplate.ContainsKey(arg.Key.TrimStart('?')) && queryInTemplate[arg.Key.TrimStart('?')] == arg.Value))
                {
                    return null;
                }
            }

            var (isSuccess, template) = TryParseTemplate(routeTemplate);

            if (!isSuccess)
            {
                return null;
            }

            var matcher = new TemplateMatcher(template, GetDefaults(template));

            var values = new RouteValueDictionary();

            return matcher.TryMatch(requestPath, values) ? values : null;
        }

        private static (bool IsSuccess, RouteTemplate Template) TryParseTemplate(string routeTemplate)
        {
            try
            {
                return (true, TemplateParser.Parse(routeTemplate));
            }
            catch
            {
                return (false, null);
            }
        }

        private RouteValueDictionary GetDefaults(RouteTemplate parsedTemplate)
        {
            var result = new RouteValueDictionary();

            foreach (var parameter in parsedTemplate.Parameters)
            {
                if (parameter.DefaultValue != null)
                {
                    result.Add(parameter.Name, parameter.DefaultValue);
                }
            }

            return result;
        }
    }
}
