# DovetailHostBuilderExtensions

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Version: `0.2.4-beta.12`
- Namespace: `Joinery.Dovetail`
- Target framework: `net10.0`

Base convention extensions

## Definition

```csharp
namespace Joinery.Dovetail;

public static class DovetailHostBuilderExtensions
{
    // ...
}
```

## Methods

- [AddIfMissing(DovetailContextBuilder, T)](/Dovetail/api/joinery.dovetail/dovetailhostbuilderextensions/methods.md#addifmissing-dovetailcontextbuilder-t) : [DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md) `extension` -- Set key to the value if the type is missing
- [AddIfMissing(DovetailContextBuilder, Type, object)](/Dovetail/api/joinery.dovetail/dovetailhostbuilderextensions/methods.md#addifmissing-dovetailcontextbuilder-type-object) : [DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md) `extension` -- Set key to the value if the key is missing
- [AddIfMissing(DovetailContextBuilder, string, T)](/Dovetail/api/joinery.dovetail/dovetailhostbuilderextensions/methods.md#addifmissing-dovetailcontextbuilder-string-t) : [DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md) `extension` -- Set key to the value if the key is missing
- [Get(DovetailContextBuilder)](/Dovetail/api/joinery.dovetail/dovetailhostbuilderextensions/methods.md#get-dovetailcontextbuilder) : `T?` `extension` -- Get a value by type from the context
- [Get(DovetailContextBuilder, string)](/Dovetail/api/joinery.dovetail/dovetailhostbuilderextensions/methods.md#get-dovetailcontextbuilder-string) : `T?` `extension` -- Get a value by key from the context
- [GetHostType(DovetailContextBuilder)](/Dovetail/api/joinery.dovetail/dovetailhostbuilderextensions/methods.md#gethosttype-dovetailcontextbuilder) : [DovetailHostType](/Dovetail/api/joinery.dovetail/dovetailhosttype.md) `extension` -- Get the host type the builder is currently configured for
- [GetOrAdd(DovetailContextBuilder, Func<T>)](/Dovetail/api/joinery.dovetail/dovetailhostbuilderextensions/methods.md#getoradd-dovetailcontextbuilder-func-t) : `T` `extension` -- Get a value by key from the context
- [GetOrAdd(DovetailContextBuilder, string, Func<T>)](/Dovetail/api/joinery.dovetail/dovetailhostbuilderextensions/methods.md#getoradd-dovetailcontextbuilder-string-func-t) : `T` `extension` -- Get a value by key from the context
- [IsUnitTestHost(DovetailContextBuilder)](/Dovetail/api/joinery.dovetail/dovetailhostbuilderextensions/methods.md#isunittesthost-dovetailcontextbuilder) : `bool` `extension` -- Check if this is a test host (to allow conventions to behave differently during unit tests)
- [Require(DovetailContextBuilder)](/Dovetail/api/joinery.dovetail/dovetailhostbuilderextensions/methods.md#require-dovetailcontextbuilder) : `T` `extension` -- Get a value by type from the context or throw
- [Require(DovetailContextBuilder, string)](/Dovetail/api/joinery.dovetail/dovetailhostbuilderextensions/methods.md#require-dovetailcontextbuilder-string) : `T` `extension` -- Get a value by type from the context or throw
- [Set(DovetailContextBuilder, T)](/Dovetail/api/joinery.dovetail/dovetailhostbuilderextensions/methods.md#set-dovetailcontextbuilder-t) : [DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md) `extension` -- Get a value by type from the context
- [Set(DovetailContextBuilder, Type, object)](/Dovetail/api/joinery.dovetail/dovetailhostbuilderextensions/methods.md#set-dovetailcontextbuilder-type-object) : [DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md) `extension` -- Get a value by type from the context
- [Set(DovetailContextBuilder, string, T)](/Dovetail/api/joinery.dovetail/dovetailhostbuilderextensions/methods.md#set-dovetailcontextbuilder-string-t) : [DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md) `extension` -- Get a value by type from the context
- [UseServiceProviderFactory(DovetailContextBuilder, IServiceProviderFactory<TContainerBuilder>)](/Dovetail/api/joinery.dovetail/dovetailhostbuilderextensions/methods.md#useserviceproviderfactory-dovetailcontextbuilder-iserviceproviderfactory-tcontainerbuilder) : [DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md) `extension` -- Set the service provider factory to be used for hosting or other systems.
- [UseServiceProviderFactory(DovetailContextBuilder, Func<IDovetailContext, IServiceCollection, CancellationToken, ValueTask<IServiceProviderFactory<TContainerBuilder>>>)](/Dovetail/api/joinery.dovetail/dovetailhostbuilderextensions/methods.md#useserviceproviderfactory-dovetailcontextbuilder-func-idovetailcontext-iservicecollection-cancellationtoken-valuetask-iserviceproviderfactory-tcontainerbuilder) : [DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md) `extension` -- Set the service provider factory to be used for hosting or other systems.
- [UseServiceProviderFactory(DovetailContextBuilder, Func<IDovetailContext, IServiceCollection, ValueTask<IServiceProviderFactory<TContainerBuilder>>>)](/Dovetail/api/joinery.dovetail/dovetailhostbuilderextensions/methods.md#useserviceproviderfactory-dovetailcontextbuilder-func-idovetailcontext-iservicecollection-valuetask-iserviceproviderfactory-tcontainerbuilder) : [DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md) `extension` -- Set the service provider factory to be used for hosting or other systems.
- [UseServiceProviderFactory(IDovetailContext, IServiceProviderFactory<TContainerBuilder>)](/Dovetail/api/joinery.dovetail/dovetailhostbuilderextensions/methods.md#useserviceproviderfactory-idovetailcontext-iserviceproviderfactory-tcontainerbuilder) : [IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md) `extension` -- Set the service provider factory to be used for hosting or other systems.
- [UseServiceProviderFactory(IDovetailContext, Func<IDovetailContext, IServiceCollection, CancellationToken, ValueTask<IServiceProviderFactory<TContainerBuilder>>>)](/Dovetail/api/joinery.dovetail/dovetailhostbuilderextensions/methods.md#useserviceproviderfactory-idovetailcontext-func-idovetailcontext-iservicecollection-cancellationtoken-valuetask-iserviceproviderfactory-tcontainerbuilder) : [IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md) `extension` -- Set the service provider factory to be used for hosting or other systems.
- [UseServiceProviderFactory(IDovetailContext, Func<IDovetailContext, IServiceCollection, ValueTask<IServiceProviderFactory<TContainerBuilder>>>)](/Dovetail/api/joinery.dovetail/dovetailhostbuilderextensions/methods.md#useserviceproviderfactory-idovetailcontext-func-idovetailcontext-iservicecollection-valuetask-iserviceproviderfactory-tcontainerbuilder) : [IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md) `extension` -- Set the service provider factory to be used for hosting or other systems.
