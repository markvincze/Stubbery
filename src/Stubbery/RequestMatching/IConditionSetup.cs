using System;
using Microsoft.AspNetCore.Http;

namespace Stubbery.RequestMatching
{
    public interface IConditionSetup
    {
        ISetup IfHeader(string header, string value);

        ISetup IfHeaders(Func<IHeaderDictionary, bool> check);

        ISetup IfContentType(string contentType);

        ISetup IfContentType(Func<string, bool> check);

        ISetup IfAccept(string accept);

        ISetup IfAccept(Func<string, bool> accept);

        ISetup Route(string routeTemplate);
    }
}