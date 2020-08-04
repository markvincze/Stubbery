using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Stubbery.RequestMatching
{
    /// <summary>
    /// Represents the response arguments which can be customized for stubs.
    /// </summary>
    public interface IResultSetup
    {
        /// <summary>
        /// Sets up the response to be sent by the stubbed endpoint.
        /// </summary>
        /// <param name="responder">The delegate that is called to create the response.</param>
        /// <remarks>
        /// The object returned by the <paramref name="responder" /> delegate must be a string. That string will be directly written to the response body.
        /// </remarks>
        /// <returns>The same <see cref="ISetup"/>-instance, so that more parameters can be set fluently.</returns>
        ISetup Response(CreateStubResponse responder);

        /// <summary>
        /// Sets the status code of the stub response.
        /// </summary>
        /// <param name="statusCode">The status code to be set on the response.</param>
        /// <returns>The same <see cref="ISetup"/>-instance, so that more parameters can be set fluently.</returns>
        ISetup StatusCode(int statusCode);

        /// <summary>
        /// Sets the function providing the status code of the stub response.
        /// </summary>
        /// <param name="statusCodeProvider">The function which provides the status code based on the request.</param>
        /// <returns>The same <see cref="ISetup"/>-instance, so that more parameters can be set fluently.</returns>
        ISetup StatusCode(Func<HttpRequest, RequestArguments, int> statusCodeProvider);

        /// <summary>
        /// Sets a header to be added to the stub response.
        /// </summary>
        /// <param name="header">The name of the header to be added to the response.</param>
        /// <param name="value">The value of the header to be added to the response.</param>
        /// <returns>The same <see cref="ISetup"/>-instance, so that more parameters can be set fluently.</returns>
        ISetup Header(string header, string value);

        /// <summary>
        /// Sets the function providing a header to be added to the stub response.
        /// </summary>
        /// <param name="headerProvider">The function which provides the header based on the request.</param>
        /// <returns>The same <see cref="ISetup"/>-instance, so that more parameters can be set fluently.</returns>
        ISetup Header(Func<HttpRequest, RequestArguments, (string, string)> headerProvider);

        /// <summary>
        /// Sets the given headers to be added to the response.
        /// </summary>
        /// <param name="headers">The headers to be added to the response.</param>
        /// <returns>The same <see cref="ISetup"/>-instance, so that more parameters can be set fluently.</returns>
        ISetup Headers(params (string, string)[] headers);

        /// <summary>
        /// Sets the function providing headers to be added to the stub response.
        /// </summary>
        /// <param name="headersProvider">The function which provides the headers based on the request.</param>
        /// <returns>The same <see cref="ISetup"/>-instance, so that more parameters can be set fluently.</returns>
        ISetup Headers(Func<HttpRequest, RequestArguments, (string, string)[]> headersProvider);
    }
}
