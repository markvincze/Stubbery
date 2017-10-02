using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using Stubbery.RequestMatching;

namespace Stubbery
{
    /// <summary>
    /// Represents an Api stub that responds to certain requests with preconfigured responses.
    /// </summary>
    /// <remarks>
    /// After instantiating and setting up the stubbed endpoints, the stub needs to be started by calling the <see cref="Start" /> method.
    /// </remarks>
    public class ApiStub : IDisposable
    {
        private readonly ICollection<Setup> configuredEndpoints = new List<Setup>();

        private readonly ApiHost apiHost;

        private ApiStubState state = ApiStubState.Stopped;

        private string address;

        /// <summary>
        /// Creates a new instance of <see cref="ApiStub" />
        /// </summary>
        /// <remarks>
        /// The constructor does not start up the stub web server. In order to start listening for requests, the <see cref="Start" /> method needs to be called.
        /// </remarks>
        public ApiStub()
        {
            var startup = new ApiStubWebAppStartup(
                new ApiStubRequestHandler(configuredEndpoints),
                () => this.DefaultOutputFormatter);

            apiHost = new ApiHost(startup);
        }

        /// <summary>
        /// Gets the full url at which the stub is accessible.
        /// </summary>
        /// <remarks>
        /// When calling the <see cref="Start" /> method, the stub is started on a randomly picked free port.
        /// Thus <see cref="Address" /> will return a url like "http://localhost:51234".
        /// </remarks>
        public string Address
        {
            get
            {
                if (state == ApiStubState.Stopped)
                {
                    throw new InvalidOperationException($"The api stub is not started yet. It can be started by calling the {nameof(Start)} method.");
                }

                return address;
            }
        }

        /// <summary>
        /// Starts the stub server and starts listening to requests.
        /// </summary>
        /// <remarks>
        /// The stub is started on a randomly picked free port, so it will be accessible on an url like "http://localhost:51234".
        /// The exact address can be retrieved through the <see cref="Address" /> property.
        /// </remarks>
        public void Start()
        {
            if (state == ApiStubState.Started)
            {
                throw new InvalidOperationException("The api stub is already started.");
            }

            address = apiHost.StartHosting();

            state = ApiStubState.Started;
        }

        /// <summary>
        /// Stops the stub server.
        /// </summary>
        /// <remarks>
        /// The stub needs to be disposed when it's not needed, so the TCP port used is freed up.
        /// </remarks>
        public void Dispose()
        {
            apiHost.Stop();

            state = ApiStubState.Stopped;
        }

        /// <summary>
        /// The default output formatter to used when returning response bodies.
        /// </summary>
        /// <remarks>
        /// If in an endpoint configuration the response object is a string, it is directly returned in the HTTP Response body.
        /// On the other hand, if it's any other object, the response body is created using this output formatter.
        /// By default, a Json output formatter is used.
        /// </remarks>
        public OutputFormatter DefaultOutputFormatter { get; set; } = new JsonOutputFormatter(new JsonSerializerSettings(), ArrayPool<char>.Shared);

        /// <summary>
        /// Sets up a new stubbed GET request on a specific route with a response.
        /// </summary>
        /// <param name="route">The route on which the stub will respond.</param>
        /// <param name="responder">The stubbed response to send.</param>
        /// <returns>An <see cref="ISetup" /> object on which further conditions can be set.</returns>
        public ISetup Get(string route, CreateStubResponse responder)
        {
            return Request(HttpMethod.Get)
                .IfRoute(route)
                .Response(responder);
        }

        /// <summary>
        /// Sets up a new stubbed GET request.
        /// </summary>
        /// <returns>An <see cref="ISetup" /> object on which further conditions can be set.</returns>
        public ISetup Get()
        {
            return Request(HttpMethod.Get);
        }

        /// <summary>
        /// Sets up a new stubbed POST request on a specific route with a response.
        /// </summary>
        /// <param name="route">The route on which the stub will respond.</param>
        /// <param name="responder">The stubbed response to send.</param>
        /// <returns>An <see cref="ISetup" /> object on which further conditions can be set.</returns>
        public ISetup Post(string route, CreateStubResponse responder)
        {
            return Request(HttpMethod.Post)
                .IfRoute(route)
                .Response(responder);
        }

        /// <summary>
        /// Sets up a new stubbed POST request.
        /// </summary>
        /// <returns>An <see cref="ISetup" /> object on which further conditions can be set.</returns>
        public ISetup Post()
        {
            return Request(HttpMethod.Post);
        }

        /// <summary>
        /// Sets up a new stubbed PUT request on a specific route with a response.
        /// </summary>
        /// <param name="route">The route on which the stub will respond.</param>
        /// <param name="responder">The stubbed response to send.</param>
        /// <returns>An <see cref="ISetup" /> object on which further conditions can be set.</returns>
        public ISetup Put(string route, CreateStubResponse responder)
        {
            return Request(HttpMethod.Put)
                .IfRoute(route)
                .Response(responder);
        }

        /// <summary>
        /// Sets up a new stubbed PUT request.
        /// </summary>
        /// <returns>An <see cref="ISetup" /> object on which further conditions can be set.</returns>
        public ISetup Put()
        {
            return Request(HttpMethod.Put);
        }

        /// <summary>
        /// Sets up a new stubbed DELETE request on a specific route with a response.
        /// </summary>
        /// <param name="route">The route on which the stub will respond.</param>
        /// <param name="responder">The stubbed response to send.</param>
        /// <returns>An <see cref="ISetup" /> object on which further conditions can be set.</returns>
        public ISetup Delete(string route, CreateStubResponse responder)
        {
            return Request(HttpMethod.Delete)
                .IfRoute(route)
                .Response(responder);
        }

        /// <summary>
        /// Sets up a new stubbed DELETE request.
        /// </summary>
        /// <returns>An <see cref="ISetup" /> object on which further conditions can be set.</returns>
        public ISetup Delete()
        {
            return Request(HttpMethod.Delete);
        }

        /// <summary>
        /// Sets up a new stubbed request for the specified HTTP <paramref name="methods" />.
        /// </summary>
        /// <param name="methods">The HTTP methods for which we want to respond.</param>
        /// <returns>An <see cref="ISetup" /> object on which further conditions can be set.</returns>
        public ISetup Request(params HttpMethod[] methods)
        {
            var setup = new Setup(methods);

            configuredEndpoints.Add(setup);

            return setup;
        }
    }
}