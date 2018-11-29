# Configure the response

Various properties of the stub response can be customized.

## Response body

If we're using the `Get`, `Post`, `Put` and `Delete` methods, the response body can be specified with the delegate passed as the second argument.

```csharp
stub.Get("/testget", (req, args) => "testresponse");
```

It can also be specified by calling the `Response` method.

```csharp
stub.Request(HttpMethod.Get)
    .IfRoute("/testget")
    .Response((req, args) => "testresponse");
```

### Return type

The `CreateStubResponse` delegate must return a string, which will be directly written to the response body.

### Request arguments

Arguments of the request are accessible through the `RequestArguments` object passed to the delegate.

```csharp
stub.Get(
    "/testget/{arg1}/part/{arg2}",
    (req, args) => $"testresponse arg1: {args.Route.arg1} arg2: {args.Route.arg2} qarg1: {args.Query.qarg1} qarg2: {args.Query.qarg2}");

stub.Start();

var result = await httpClient.GetAsync(
    new UriBuilder(new Uri(stub.Address)) { Path = "/testget/orange/part/apple", Query = "?qarg1=melon&qarg2=pear" }.Uri);

// The resultString will be "testresponse arg1: orange arg2: apple qarg1: melon qarg2: pear".
var resultString = await result.Content.ReadAsStringAsync();
```

## Status code

The status code of the response can be specified with the `StatusCode` method.

```csharp
stub.Get("/testget", (req, args) => "testresponse")
    .StatusCode(StatusCodes.Status206PartialContent);
```

## Headers

Headers of the response can be specified with the `Header` method.

```csharp
stub.Get("/testget", (req, args) => "testresponse")
    .Header("Header1", "HeaderValue1")
    .Header("Header2", "HeaderValue2");
```