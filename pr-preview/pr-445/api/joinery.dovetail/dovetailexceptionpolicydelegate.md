# DovetailExceptionPolicyDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-445/api/joinery.dovetail.md)
- Version: `0.8.2-pr.14`
- Namespace: `Joinery.Dovetail`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-445-merge.Sha.e9bf50100253767b2d628a32eb5673b0a7476031/src/Joinery.Dovetail/DovetailExceptionPolicyDelegate.cs)
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

- [DovetailExceptionPolicyDelegate(object, nint)](/Dovetail/pr-preview/pr-445/api/joinery.dovetail/dovetailexceptionpolicydelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(Exception, AsyncCallback, object)](/Dovetail/pr-preview/pr-445/api/joinery.dovetail/dovetailexceptionpolicydelegate/methods.md#begininvoke-exception-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-445/api/joinery.dovetail/dovetailexceptionpolicydelegate/methods.md#endinvoke-iasyncresult) : `bool` `virtual`
