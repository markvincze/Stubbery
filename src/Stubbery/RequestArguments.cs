using System.IO;

namespace Stubbery
{
    public class RequestArguments
    {
        public dynamic Route { get; }

        public dynamic Query { get; }

        public Stream Body { get; }

        internal RequestArguments(DynamicValues route, DynamicValues query, Stream body)
        {
            Route = route;
            Query = query;
            Body = body;
        }
    }
}