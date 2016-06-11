using Microsoft.AspNetCore.Http;

namespace Stubbery
{
    public delegate dynamic CreateStubResponse(HttpRequest request, RequestArguments args);
}