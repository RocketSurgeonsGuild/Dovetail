# DovetailExceptionPolicyDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Version: `0.4.2-beta.1`
- Namespace: `Joinery.Dovetail`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

A policy delegate that can be used to determine if a given exception should be ignored or not

## Definition

```csharp
namespace Joinery.Dovetail;

public sealed delegate bool DovetailExceptionPolicyDelegate(
    System.Exception exception)
```

## Delegate Parameters

- `exception` (`Exception`) -- The exception thrown by a convention.

## Constructors

- [DovetailExceptionPolicyDelegate(object, nint)](/Dovetail/api/joinery.dovetail/dovetailexceptionpolicydelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(Exception, AsyncCallback, object)](/Dovetail/api/joinery.dovetail/dovetailexceptionpolicydelegate/methods.md#begininvoke-exception-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail/dovetailexceptionpolicydelegate/methods.md#endinvoke-iasyncresult) : `bool` `virtual`
