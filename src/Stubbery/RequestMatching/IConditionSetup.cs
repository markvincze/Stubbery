using System;
using Microsoft.AspNetCore.Http;

namespace Stubbery.RequestMatching
{
    /// <summary>
    /// Represents the different preconditions that can be set on stubs.
    /// </summary>
    public interface IConditionSetup
    {
        /// <summary>
        /// Sets up a condition so that the stub only responds if the request has the specified header.
        /// </summary>
        /// <param name="header">The name of the required header.</param>
        /// <param name="value">The value of the required header.</param>
        /// <returns>The same <see cref="ISetup"/>-instance, so that more conditions can be set fluently.</returns>
        ISetup IfHeader(string header, string value);

        /// <summary>
        /// Sets up a condition so that the stub only responds if the request headers satisfy the <paramref name="check" /> condition.
        /// </summary>
        /// <param name="check">The condition the headers have to satisfy.</param>
        /// <returns>The same <see cref="ISetup"/>-instance, so that more conditions can be set fluently.</returns>
        ISetup IfHeaders(Func<IHeaderDictionary, bool> check);

        /// <summary>
        /// Sets up a condition so that the stub only responds if the request has the specified Content-Type.
        /// </summary>
        /// <param name="contentType">The required value of the Content-Type.</param>
        /// <returns>The same <see cref="ISetup"/>-instance, so that more conditions can be set fluently.</returns>
        ISetup IfContentType(string contentType);

        /// <summary>
        /// Sets up a condition so that the stub only responds if the Content-Type satisfies the <paramref name="check" /> condition.
        /// </summary>
        /// <param name="check">The condition the Content-Type have to satisfy.</param>
        /// <returns>The same <see cref="ISetup"/>-instance, so that more conditions can be set fluently.</returns>
        ISetup IfContentType(Func<string, bool> check);

        /// <summary>
        /// Sets up a condition so that the stub only responds if the request has the Accept header.
        /// </summary>
        /// <param name="accept">The required value of the Accept header.</param>
        /// <returns>The same <see cref="ISetup"/>-instance, so that more conditions can be set fluently.</returns>
        ISetup IfAccept(string accept);

        /// <summary>
        /// Sets up a condition so that the stub only responds if the Accept header satisfy the <paramref name="check" /> condition.
        /// </summary>
        /// <param name="check">The condition the Accept header has to satisfy.</param>
        /// <returns>The same <see cref="ISetup"/>-instance, so that more conditions can be set fluently.</returns>
        ISetup IfAccept(Func<string, bool> check);

        /// <summary>
        /// Sets up a condition so that the stub only responds if the route of the request matches the <paramref name="routeTemplate" />.
        /// </summary>
        /// <param name="routeTemplate">The route template we want to match with the requests.</param>
        /// <returns>The same <see cref="ISetup"/>-instance, so that more conditions can be set fluently.</returns>
        /// <remarks>
        /// <para>
        /// If a route template gets matched, the route arguments are stored in the <see cref="HttpContext" />, and they can be accessed through
        /// the <see cref="RequestArguments.Route" /> property when creating responses.
        /// </para>
        /// <para>
        /// If multiple routes are set up, then they'll be matched one by one until the first match found. The route arguments in <see cref="RequestArguments.Route" />
        /// will be populated based on the first match.
        /// </para>
        /// </remarks>
        ISetup IfRoute(string routeTemplate);
    }
}