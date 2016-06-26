# Stubbery

Simple library for creating and running Api stubs in .NET.

[![Build status](https://ci.appveyor.com/api/projects/status/lv48y6utx9ulcwdh?svg=true)](https://ci.appveyor.com/project/markvincze/stubbery)

## Introduction

In many situations it comes handy if we're able to start a simple service that responds on certain routes with preconfigured static responses.

This is particularly important in integration testing, when we might want to replace some of our dependencies with a stub that can reliably provide the expected responses.

**Stubbery** is a library with which we can simply configure and start a web server that responds on particular routes with the configured results.
It supports .NET Core and the full .NET Framework up from .NET 4.5.1.

The binaries are published on [NuGet](https://www.nuget.org/packages/Stubbery/) and you can find the source code on [GitHub](https://github.com/markvincze/Stubbery).

## Basic usage

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

More details can be found in the [Documentation](documentation/intro.md) and in the [Api Reference](api/index.md).