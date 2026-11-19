# DovetailHostType

- Kind: `enum`
- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Version: `0.7.2-beta.15`
- Namespace: `Joinery.Dovetail`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.c5bb6c9e8f86c7f4f65638c0127e84cdbf96b00b/src/Joinery.Dovetail/DovetailHostType.cs)

The underlying host type (currently only checked for test host) This is so that conventions can avoid adding behaviors that might cause issues with unit testing such as writing to the console or debug pipes, or ensuring that something is configured in a correct way for testing. Careful consideration is needed to make sure that your system doesn't behave extremely differently in a live scenario vs a testing scenario

## Definition

```csharp
namespace Joinery.Dovetail;

public enum DovetailHostType
```

## Enum Members

| Name | Value | Description |
| --- | --- | --- |
| Undefined | 0 | No host type has been defined |
| Live | 1 | This is a live application This also may apply to in memory integration tests running through the HostBuilder infrastructure |
| UnitTest | 2 | This is a unit test context This is so that conventions can avoid adding behaviors that might cause issues with unit testing such as writing to the console or debug pipes, or ensuring that something is configured in a correct way for testing. Careful consideration is needed to make sure that your system doesn't behave extremely differently in a live scenario vs a testing scenario |
