# Stubbery

Simple library for creating and running Api stubs in .NET.

[![Build status](https://ci.appveyor.com/api/projects/status/lv48y6utx9ulcwdh?svg=true)](https://ci.appveyor.com/project/markvincze/stubbery)

## Introduction

In many situations it comes handy if we're able to start a simple service that responds on certain routes with preconfigured static responses.

This is particularly important in integration testing, when we might want to replace some of our dependencies with a stub that can reliably provide the expected responses.

**Stubbery** is a library with which we can simply configure and start a web server that responds on particular routes with the configured results.
It supports .NET Core and the full .NET Framework up from .NET 4.5.1.

## How to use

The central class of the library is `ApiStub`. In order to start a new server we have to create an instance of `ApiStub`, set up some routes using the methods `Get`, `Post`, `Put` and `Delete`, and start the server by calling `Start`.

The server listens on `localhost` on a randomly picked free port. The full address is returned by the `Address` property.

After usage the server should be stopped to free up the TCP port. This can be done by calling `Dispose` (or use the stub in a `using` block).

### Basic usage

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
