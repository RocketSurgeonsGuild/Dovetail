# DovetailExceptionPolicyDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-459/api/joinery.dovetail.md)
- Version: `0.7.2-pr.11`
- Namespace: `Joinery.Dovetail`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-459-merge.Sha.a1990fe6695c5fa9e9c003a3a333f8ff361aa602/src/Joinery.Dovetail/DovetailExceptionPolicyDelegate.cs)
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

- [DovetailExceptionPolicyDelegate(object, nint)](/Dovetail/pr-preview/pr-459/api/joinery.dovetail/dovetailexceptionpolicydelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(Exception, AsyncCallback, object)](/Dovetail/pr-preview/pr-459/api/joinery.dovetail/dovetailexceptionpolicydelegate/methods.md#begininvoke-exception-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-459/api/joinery.dovetail/dovetailexceptionpolicydelegate/methods.md#endinvoke-iasyncresult) : `bool` `virtual`
