# DovetailHostBuilderExtensions

- Kind: `class`
- Package: [Dovetail](/Dovetail/v0.2.2/api/dovetail.md)
- Version: `0.2.2`
- Namespace: `Dovetail`
- Target framework: `net10.0`

Base convention extensions

## Definition

```csharp
namespace Dovetail;

public static class DovetailHostBuilderExtensions
{
    // ...
}
```

## Methods

- [AddIfMissing(DovetailContextBuilder, T)](/Dovetail/v0.2.2/api/dovetail/dovetailhostbuilderextensions/methods.md#addifmissing-dovetailcontextbuilder-t) : [DovetailContextBuilder](/Dovetail/v0.2.2/api/dovetail/dovetailcontextbuilder.md) `extension` -- Set key to the value if the type is missing
- [AddIfMissing(DovetailContextBuilder, Type, object)](/Dovetail/v0.2.2/api/dovetail/dovetailhostbuilderextensions/methods.md#addifmissing-dovetailcontextbuilder-type-object) : [DovetailContextBuilder](/Dovetail/v0.2.2/api/dovetail/dovetailcontextbuilder.md) `extension` -- Set key to the value if the key is missing
- [AddIfMissing(DovetailContextBuilder, string, T)](/Dovetail/v0.2.2/api/dovetail/dovetailhostbuilderextensions/methods.md#addifmissing-dovetailcontextbuilder-string-t) : [DovetailContextBuilder](/Dovetail/v0.2.2/api/dovetail/dovetailcontextbuilder.md) `extension` -- Set key to the value if the key is missing
- [Get(DovetailContextBuilder)](/Dovetail/v0.2.2/api/dovetail/dovetailhostbuilderextensions/methods.md#get-dovetailcontextbuilder) : `T?` `extension` -- Get a value by type from the context
- [Get(DovetailContextBuilder, string)](/Dovetail/v0.2.2/api/dovetail/dovetailhostbuilderextensions/methods.md#get-dovetailcontextbuilder-string) : `T?` `extension` -- Get a value by key from the context
- [GetHostType(DovetailContextBuilder)](/Dovetail/v0.2.2/api/dovetail/dovetailhostbuilderextensions/methods.md#gethosttype-dovetailcontextbuilder) : [DovetailHostType](/Dovetail/v0.2.2/api/dovetail/dovetailhosttype.md) `extension` -- Get the host type the builder is currently configured for
- [GetOrAdd(DovetailContextBuilder, Func<T>)](/Dovetail/v0.2.2/api/dovetail/dovetailhostbuilderextensions/methods.md#getoradd-dovetailcontextbuilder-func-t) : `T` `extension` -- Get a value by key from the context
- [GetOrAdd(DovetailContextBuilder, string, Func<T>)](/Dovetail/v0.2.2/api/dovetail/dovetailhostbuilderextensions/methods.md#getoradd-dovetailcontextbuilder-string-func-t) : `T` `extension` -- Get a value by key from the context
- [IsUnitTestHost(DovetailContextBuilder)](/Dovetail/v0.2.2/api/dovetail/dovetailhostbuilderextensions/methods.md#isunittesthost-dovetailcontextbuilder) : `bool` `extension` -- Check if this is a test host (to allow conventions to behave differently during unit tests)
- [Require(DovetailContextBuilder)](/Dovetail/v0.2.2/api/dovetail/dovetailhostbuilderextensions/methods.md#require-dovetailcontextbuilder) : `T` `extension` -- Get a value by type from the context or throw
- [Require(DovetailContextBuilder, string)](/Dovetail/v0.2.2/api/dovetail/dovetailhostbuilderextensions/methods.md#require-dovetailcontextbuilder-string) : `T` `extension` -- Get a value by type from the context or throw
- [Set(DovetailContextBuilder, T)](/Dovetail/v0.2.2/api/dovetail/dovetailhostbuilderextensions/methods.md#set-dovetailcontextbuilder-t) : [DovetailContextBuilder](/Dovetail/v0.2.2/api/dovetail/dovetailcontextbuilder.md) `extension` -- Get a value by type from the context
- [Set(DovetailContextBuilder, Type, object)](/Dovetail/v0.2.2/api/dovetail/dovetailhostbuilderextensions/methods.md#set-dovetailcontextbuilder-type-object) : [DovetailContextBuilder](/Dovetail/v0.2.2/api/dovetail/dovetailcontextbuilder.md) `extension` -- Get a value by type from the context
- [Set(DovetailContextBuilder, string, T)](/Dovetail/v0.2.2/api/dovetail/dovetailhostbuilderextensions/methods.md#set-dovetailcontextbuilder-string-t) : [DovetailContextBuilder](/Dovetail/v0.2.2/api/dovetail/dovetailcontextbuilder.md) `extension` -- Get a value by type from the context
- [UseServiceProviderFactory(DovetailContextBuilder, IServiceProviderFactory<TContainerBuilder>)](/Dovetail/v0.2.2/api/dovetail/dovetailhostbuilderextensions/methods.md#useserviceproviderfactory-dovetailcontextbuilder-iserviceproviderfactory-tcontainerbuilder) : [DovetailContextBuilder](/Dovetail/v0.2.2/api/dovetail/dovetailcontextbuilder.md) `extension` -- Set the service provider factory to be used for hosting or other systems.
- [UseServiceProviderFactory(DovetailContextBuilder, Func<IDovetailContext, IServiceCollection, CancellationToken, ValueTask<IServiceProviderFactory<TContainerBuilder>>>)](/Dovetail/v0.2.2/api/dovetail/dovetailhostbuilderextensions/methods.md#useserviceproviderfactory-dovetailcontextbuilder-func-idovetailcontext-iservicecollection-cancellationtoken-valuetask-iserviceproviderfactory-tcontainerbuilder) : [DovetailContextBuilder](/Dovetail/v0.2.2/api/dovetail/dovetailcontextbuilder.md) `extension` -- Set the service provider factory to be used for hosting or other systems.
- [UseServiceProviderFactory(DovetailContextBuilder, Func<IDovetailContext, IServiceCollection, ValueTask<IServiceProviderFactory<TContainerBuilder>>>)](/Dovetail/v0.2.2/api/dovetail/dovetailhostbuilderextensions/methods.md#useserviceproviderfactory-dovetailcontextbuilder-func-idovetailcontext-iservicecollection-valuetask-iserviceproviderfactory-tcontainerbuilder) : [DovetailContextBuilder](/Dovetail/v0.2.2/api/dovetail/dovetailcontextbuilder.md) `extension` -- Set the service provider factory to be used for hosting or other systems.
- [UseServiceProviderFactory(IDovetailContext, IServiceProviderFactory<TContainerBuilder>)](/Dovetail/v0.2.2/api/dovetail/dovetailhostbuilderextensions/methods.md#useserviceproviderfactory-idovetailcontext-iserviceproviderfactory-tcontainerbuilder) : [IDovetailContext](/Dovetail/v0.2.2/api/dovetail/idovetailcontext.md) `extension` -- Set the service provider factory to be used for hosting or other systems.
- [UseServiceProviderFactory(IDovetailContext, Func<IDovetailContext, IServiceCollection, CancellationToken, ValueTask<IServiceProviderFactory<TContainerBuilder>>>)](/Dovetail/v0.2.2/api/dovetail/dovetailhostbuilderextensions/methods.md#useserviceproviderfactory-idovetailcontext-func-idovetailcontext-iservicecollection-cancellationtoken-valuetask-iserviceproviderfactory-tcontainerbuilder) : [IDovetailContext](/Dovetail/v0.2.2/api/dovetail/idovetailcontext.md) `extension` -- Set the service provider factory to be used for hosting or other systems.
- [UseServiceProviderFactory(IDovetailContext, Func<IDovetailContext, IServiceCollection, ValueTask<IServiceProviderFactory<TContainerBuilder>>>)](/Dovetail/v0.2.2/api/dovetail/dovetailhostbuilderextensions/methods.md#useserviceproviderfactory-idovetailcontext-func-idovetailcontext-iservicecollection-valuetask-iserviceproviderfactory-tcontainerbuilder) : [IDovetailContext](/Dovetail/v0.2.2/api/dovetail/idovetailcontext.md) `extension` -- Set the service provider factory to be used for hosting or other systems.
