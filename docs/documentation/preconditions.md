# Setting up preconditions

When configuring a stubbed endpoint, various preconditions can be configured, so the stub only responds if the conditions are met.
The conditions can chained together with a fluent api.

## HTTP methods

For requiring the most common methods, the `Get`, `Post`, `Put` and `Delete` methods can be used.

```csharp
// Will match a GET
stub.Get("/testget", (req, args) => "testresponse");

// Will match a POST
stub.Post("/testpost", (req, args) => "testresponse");
```

If another method is needed, the `Request` method can be used.

 ```csharp
 // Will respond to OPTIONS
stub.Request(HttpMethod.Options)
    .IfRoute("/testoptions")
    .Response((req, args) => "testresponse");
```

## Headers

The presence of specific headers can be required with the `IfHeader` method.

```csharp
// Will only respond if the Origin header with the given value is present.
stub.Get("/testget", (req, args) => "testresponse")
    .IfHeader("Origin", "http://www.example.com");
```

## Route

If we only want to respond on a particular path, we can pass a route template to the `Get`, `Post`, `Put` and `Delete` methods, or we can call the `IfRoute` method. 

```csharp
// Will only respond if the request path is "/testget".
stub.Request(HttpMethod.Get)
    .IfRoute("/testget")
    .Response((req, args) => "testresponse");
```

### Route arguments

Matching route templates is implemented by using the [built-in routing](https://docs.asp.net/en/latest/fundamentals/routing.html#template-routes) of ASP.NET, the same syntax can be used here.
If we chain multiple route templates, the first matching one will be used to extract the route arguments.

The arguments are accessible when we setup the response through the `Route` property of `RequestArguments`.

```csharp
stub.Request(HttpMethod.Get)
    .IfRoute("/testget/{myArg}")
    .Response((req, args) => $"testresponse arg: {args.Route.myArg}");
``` 

## Query

If we only want to respond if a particular query string argument is present with a specific value, we can use the `IfQueryArg` method.

```csharp
// Will only respond if the request has the query string argument testarg with value testval. (for example /foo?testarg=testval)
stub.Request(HttpMethod.Get)
    .IfQueryArg("testarg", "testval")
    .Response((req, args) => "testresponse");
```

## Chaining multiple conditons

An arbitrary number of preconditions can be chained together in a fluent fashion.
If we specify multiple header conditions, the stubb will only respond if all of them are satisfied.
The other types of conditions are considered satisfied is at least on of them matches.

```csharp
stub.Get("/testget1/{arg1}", (req, args) => $"testresponse, {args.Route.arg2}")
    .IfHeader("Header1", "TestValue1")
    .IfHeader("Header2", "TestValue2")
    .IfContentType("custom/stubbery")
    .IfAccept("custom/accept")
    .IfRoute("/testget2/{arg2}")
    .StatusCode(StatusCodes.Status300MultipleChoices)
    .Header("ResponseHeader1", "ResponseValue1")
    .Header("ResponseHeader2", "ResponseValue2");
``` 
