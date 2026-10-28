# Unit Tests

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

Some unit testing frameworks have the ability to capture log output for a given test, this can be handy for helping identify any problems. If you provide a `ILogger` or a `ILoggerFactory` into the
<xref:Joinery.Dovetail.DovetailContextBuilder> or <xref:Joinery.Dovetail.IDovetailContext> it will be used during the joint scanning process and spit out diagnostic logs that may be useful.

> [!TIP]
> With TUnit, use the `TestRecord` from `Rocket.Surgery.Extensions.Testing.TUnit` (as in `AutoFakeTest<TestRecord>`): its Serilog logger writes to the test's output.