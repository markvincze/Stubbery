using Microsoft.AspNetCore.Http;

namespace Stubbery.RequestMatching
{
    public delegate dynamic CreateStubResponse(HttpRequest request, RequestArguments args);
}