using System.Collections.Generic;

namespace Stubbery.RequestMatching
{
    public interface IResultSetup
    {
        ISetup Response(CreateStubResponse responder);

        ISetup StatusCode(int statusCode);

        ISetup Header(string header, string value);

        ISetup Headers(params KeyValuePair<string, string>[] headers);
    }
}