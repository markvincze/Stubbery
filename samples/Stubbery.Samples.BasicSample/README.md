# Basic sample for integration testing with Stubbery

This solution contains two projects. An ASP.NET Core application, which can return the name of a UK county based on its post code, using the [Postcodes.io](http://postcodes.io) api.

The other project contains some integration tests, in which we are replacing the Postodese.io dependency with a stub using [Stubbery](https://github.com/markvincze/Stubbery).

More details can be found in [this blog post](http://blog.markvincze.com/stubbing-service-dependencies-in-net-using-stubbery/).
