## How to use

The central class of the library is `ApiStub`. In order to start a new server we have to create an instance of `ApiStub`, set up some routes using the methods `Get`, `Post`, `Put` and `Delete`, and start the server by calling `Start`.

The server listens on `localhost` on a randomly picked free port. The full address is returned by the `Address` property.

After usage the server should be stopped to free up the TCP port. This can be done by calling `Dispose` (or using the stub in a `using` block).

### Basic usage

The following code sample shows how a simple stub can be started that responds on the route `/testget` with the string `testresponse`.

```csharp
using (var stub = new ApiStub())
{
    stub.Get(
        "/testget",
        (req, args) => "testresponse");

    stub.Start();

    var result = await httpClient.GetAsync(new UriBuilder(new Uri(stub.Address)) { Path = "/testget" }.Uri);

    // resultString will contain "testresponse"
    var resultString = await result.Content.ReadAsStringAsync();
}
```

### Accessing the request parameters

Parameters of the request can be accessed through the second argument of the lambda setting up the response. The following code sample shows how the route and the query string parameters can be accessed.

```csharp
using (var stub = new ApiStub())
{
    stub.Get(
        "/testget/{arg1}",
        (req, args) => $"testresponse arg1: {args.Route.arg1} queryArg1: {args.Query.queryArg1}");

    stub.Start();

    var result = await httpClient.GetAsync(
        new UriBuilder(new Uri(stub.Address)) { Path = "/testget/orange", Query = "?queryArg1=melon" }.Uri);

    // resultString will contain "testresponse arg1: orange queryArg1: melon"
    var resultString = await result.Content.ReadAsStringAsync();
}
```

The following example shows how the HTTP body can be accessed.

```csharp
using (var stub = new ApiStub())
{
    stub.Post(
        "/testpost",
        (req, args) => $"testresponse body: {args.Body.ReadAsString()}");

    stub.Start();

    var result = await httpClient.PostAsync(
        new UriBuilder(new Uri(stub.Address)) { Path = "/testpost" }.Uri,
        new StringContent("orange"));

    // resultString will contain "testresponse body: orange"
    var resultString = await result.Content.ReadAsStringAsync();
}
```

[More details](response.md) about setting response properties.

### Other verbs

If we want to use a different HTTP verb, the `Request` method can be used.

```csharp
using (var stub = new ApiStub())
{
    sut.Request(HttpMethod.Options)
        .IfRoute("/testoptions")
        .Response((req, args) => "testresponse");

    stub.Start();

    var result = await httpClient.SendAsync(
        new HttpRequestMessage
        {
            RequestUri = new UriBuilder(new Uri(stub.Address)) { Path = "/testoptions" }.Uri,
            Method = HttpMethod.Options
        });

    // resultString will contain "testresponse"
    var resultString = await result.Content.ReadAsStringAsync();
}
```

[More details](preconditions.md) about setting up preconditions.