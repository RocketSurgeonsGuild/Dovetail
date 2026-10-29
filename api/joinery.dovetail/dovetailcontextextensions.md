# DovetailContextExtensions

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Version: `0.2.4-beta.13`
- Namespace: `Joinery.Dovetail`
- Target framework: `net10.0`

Base convention extensions

## Definition

```csharp
namespace Joinery.Dovetail;

public static class DovetailContextExtensions
{
    // ...
}
```

## Methods

- [AddIfMissing(IDovetailContext, T)](/Dovetail/api/joinery.dovetail/dovetailcontextextensions/methods.md#addifmissing-idovetailcontext-t) : [IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md) `extension` -- Set key to the value if the type is missing
- [AddIfMissing(IDovetailContext, Type, object)](/Dovetail/api/joinery.dovetail/dovetailcontextextensions/methods.md#addifmissing-idovetailcontext-type-object) : [IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md) `extension` -- Set key to the value if the key is missing
- [AddIfMissing(IDovetailContext, string, T)](/Dovetail/api/joinery.dovetail/dovetailcontextextensions/methods.md#addifmissing-idovetailcontext-string-t) : [IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md) `extension` -- Set key to the value if the key is missing
- [Get(IDovetailContext)](/Dovetail/api/joinery.dovetail/dovetailcontextextensions/methods.md#get-idovetailcontext) : `T?` `extension` -- Get a value by type from the context
- [Get(IDovetailContext, string)](/Dovetail/api/joinery.dovetail/dovetailcontextextensions/methods.md#get-idovetailcontext-string) : `T?` `extension` -- Get a value by key from the context
- [GetHostType(IDovetailContext)](/Dovetail/api/joinery.dovetail/dovetailcontextextensions/methods.md#gethosttype-idovetailcontext) : [DovetailHostType](/Dovetail/api/joinery.dovetail/dovetailhosttype.md) `extension` -- Get the host type the context is currently running under
- [GetOrAdd(IDovetailContext, Func<T>)](/Dovetail/api/joinery.dovetail/dovetailcontextextensions/methods.md#getoradd-idovetailcontext-func-t) : `T` `extension` -- Get a value by key from the context
- [GetOrAdd(IDovetailContext, string, Func<T>)](/Dovetail/api/joinery.dovetail/dovetailcontextextensions/methods.md#getoradd-idovetailcontext-string-func-t) : `T` `extension` -- Get a value by key from the context
- [IsUnitTestHost(IDovetailContext)](/Dovetail/api/joinery.dovetail/dovetailcontextextensions/methods.md#isunittesthost-idovetailcontext) : `bool` `extension` -- Check if this is a test host (to allow conventions to behave differently during unit tests)
- [RegisterJoints(IDovetailContext, Action<DovetailExecutor>)](/Dovetail/api/joinery.dovetail/dovetailcontextextensions/methods.md#registerjoints-idovetailcontext-action-dovetailexecutor) `extension` -- Register a set of conventions
- [RegisterJointsAsync(IDovetailContext, Action<DovetailExecutor>, CancellationToken)](/Dovetail/api/joinery.dovetail/dovetailcontextextensions/methods.md#registerjointsasync-idovetailcontext-action-dovetailexecutor-cancellationtoken) : `ValueTask` `extension` -- Register a set of conventions
- [RegisterJointsWithContext(IDovetailContext, Action<DovetailExecutor>)](/Dovetail/api/joinery.dovetail/dovetailcontextextensions/methods.md#registerjointswithcontext-idovetailcontext-action-dovetailexecutor) : [IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md) `extension` -- Register a set of conventions
- [RegisterJointsWithContextAsync(IDovetailContext, Action<DovetailExecutor>, CancellationToken)](/Dovetail/api/joinery.dovetail/dovetailcontextextensions/methods.md#registerjointswithcontextasync-idovetailcontext-action-dovetailexecutor-cancellationtoken) : `ValueTask<IDovetailContext>` `extension` -- Register a set of conventions
- [Require(IDovetailContext)](/Dovetail/api/joinery.dovetail/dovetailcontextextensions/methods.md#require-idovetailcontext) : `T` `extension` -- Get a value by type from the context or throw
- [Require(IDovetailContext, string)](/Dovetail/api/joinery.dovetail/dovetailcontextextensions/methods.md#require-idovetailcontext-string) : `T` `extension` -- Get a value by type from the context or throw
- [Set(IDovetailContext, T)](/Dovetail/api/joinery.dovetail/dovetailcontextextensions/methods.md#set-idovetailcontext-t) : [IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md) `extension` -- Get a value by type from the context
- [Set(IDovetailContext, Type, object)](/Dovetail/api/joinery.dovetail/dovetailcontextextensions/methods.md#set-idovetailcontext-type-object) : [IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md) `extension` -- Get a value by type from the context
- [Set(IDovetailContext, string, T)](/Dovetail/api/joinery.dovetail/dovetailcontextextensions/methods.md#set-idovetailcontext-string-t) : [IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md) `extension` -- Get a value by type from the context
