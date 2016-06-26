using System.IO;

namespace Stubbery
{
    /// <summary>
    /// Represents the most important arguments of an HTTP request.
    /// </summary>
    public class RequestArguments
    {
        /// <summary>
        /// The route arguments.
        /// </summary>
        /// <remarks>
        /// The arguments are accessible with their names specified in the route template.
        /// For example, if the route template is "testroute/{arg1}/path/{arg2}", and we send a request to "testroute/value1/path/value2",
        /// we can access the two arguments by retrieving the properties arg1 and arg2.
        /// <code>
        /// var arg1 = args.Route.arg1;
        /// var arg2 = args.Route.arg2;
        /// </code>
        /// </remarks>
        public dynamic Route { get; }

        /// <summary>
        /// The query string arguments.
        /// </summary>
        /// <remarks>
        /// The arguments are accessible with their names specified in the query string.
        /// For example, if the query string is "?arg1=value1&arg2=value2", we can access the two arguments by retrieving the properties arg1 and arg2.
        /// <code>
        /// var arg1 = args.Query.arg1;
        /// var arg2 = args.Query.arg2;
        /// </code>
        /// </remarks>
        public dynamic Query { get; }

        /// <summary>
        /// The stream of the HTTP request body.
        /// </summary>
        public Stream Body { get; }

        internal RequestArguments(DynamicValues route, DynamicValues query, Stream body)
        {
            Route = route;
            Query = query;
            Body = body;
        }
    }
}