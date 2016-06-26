using Microsoft.AspNetCore.Http;

namespace Stubbery.RequestMatching
{
    /// <summary>
    /// Specifies the signature of a delegate that will create the stub response.
    /// </summary>
    /// <param name="request">The HTTP request.</param>
    /// <param name="args">The arguments extracted from the HTTP request.</param>
    /// <returns>The stubbed HTTP response.</returns>
    /// <remarks>
    /// The object returned by the <see cref="CreateStubResponse" /> delegate must be a string. That string will be directly written to the response body.
    /// </remarks>
    public delegate object CreateStubResponse(HttpRequest request, RequestArguments args);
}