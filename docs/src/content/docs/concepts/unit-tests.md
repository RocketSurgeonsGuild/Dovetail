---
title: Unit Tests
description: How to write unit tests for your joints using DovetailContextBuilder and TestHost.
---

# Unit Testing with Dovetails

Dovetail offers some great magic but without being able to write unit tests, then the magic hurts more than it might help.

Unit Tests can be written many different ways.

## Use <xref:Joinery.Dovetail.DovetailContextBuilder> directly

You can make use of the <xref:Joinery.Dovetail.DovetailContextBuilder> by itself, and apply all the joints. This requires more manual configuration but gives the greatest flexibility.

[!code-c#[Sample Test](../../sample/Sample.Core.Tests/SampleTests.cs?name=codeblock)]

## Use <xref:Joinery.Dovetail.Hosting.TestHost>

The <xref:Joinery.Dovetail.Hosting.TestHost> can be used to create a `IHostBuilder` configured for testing. This will automatically do the work for setting up Configuration, Services and Logging.

[!code-c#[Sample Test](../../sample/Sample.Tests/SampleTestHostTests.cs?name=codeblock)]

> [!TIP]
> There is <xref:Joinery.Dovetail.WebAssembly.Hosting.TestWebAssemblyHost> for WebAssembly / Blazor apps

## Logging

Some unit testing frameworks have the ability to capture log output for a given test, this can be handy for helping identify any problems.

Dovetail itself does not log while it scans and applies joints. <xref:Joinery.Dovetail.DovetailContextBuilder> and <xref:Joinery.Dovetail.IDovetailContext>
take no `ILogger` or `ILoggerFactory`, and nothing in the packages looks for one, so a logger given to either produces no diagnostic output about the
scan. What a test can capture is what your own code logs, including your joints.

> [!TIP]
> With TUnit, use the `TestRecord` from `Rocket.Surgery.Extensions.Testing.TUnit` (as in `AutoFakeTest<TestRecord>`): its Serilog logger writes to the test's output.

The builder is a bag of properties, so the test can put its logger there for a joint to find:

```c#
// In the test, where Logger is the test's Serilog logger.
builder.Set(Logger);
```

```c#
// In the joint under test.
public class AuditJoint : IServiceJoint
{
    public void Register(IDovetailContext context, IServiceCollection services) =>
        context.Get<Serilog.ILogger>()?.Information("Registering the audit services");
}
```

A joint can also take the logger as a constructor parameter, as described in [Injecting dependencies](../defining-joints/#injecting-dependencies).
In an application, as opposed to a test, the sinks and enrichers of the logger are configured by the [Serilog joint](../serilog/#the-serilog-joint).
