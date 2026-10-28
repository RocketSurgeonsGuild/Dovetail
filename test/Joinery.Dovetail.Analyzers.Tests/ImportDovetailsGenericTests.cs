using System.Collections.Immutable;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Aspire.Hosting;
using Aspire.Hosting.Testing;
using Joinery.Dovetail.Testing.Joints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;



namespace Joinery.Dovetail.Analyzers.Tests;

public class ImportDovetailsGenericTests() : GeneratorTest()
{
    [Test]
    public async Task Should_Generate_Static_Assembly_Level_Method()
    {
        var result = await WithGenericSharedDeps()
                          .AddSources(
                               @"
using Joinery.Dovetail;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Infrastructure;

"
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Should_Generate_Static_Assembly_Level_Method_Custom_Namespace()
    {
        var result = await WithGenericSharedDeps()
                          .AddSources(
                               """

                               using Joinery.Dovetail;


                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Should_Generate_Static_Assembly_Level_Method_No_Namespace()
    {
        var result = await WithGenericSharedDeps()
                          .AddSources(
                               """

                               using Joinery.Dovetail;


                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Should_Generate_Static_Assembly_Level_Method_Custom_MethodName()
    {
        var result = await WithGenericSharedDeps()
                          .AddSources(
                               """

                               using Joinery.Dovetail;


                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Should_Generate_Static_Assembly_Level_Method_FullName()
    {
        var result = await WithGenericSharedDeps()
                          .AddSources(
                               @"
using Joinery.Dovetail;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Infrastructure;

"
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Should_Support_No_Exported_Dovetail_Assemblies()
    {
        var result = await Builder
                          .AddSources(
                               @"
using Joinery.Dovetail;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Infrastructure;

"
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Should_Support_Imports_And_Exports_In_The_Same_Assembly()
    {
        var result = await Builder
                          .AddSources(
                               @"
using Joinery.Dovetail;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Infrastructure;
using Joinery.Dovetail.Tests;


namespace Joinery.Dovetail.Tests
{
    [DovetailExport]
    internal class Contrib : IDovetailJoint { }
}
",
                               @"
using Joinery.Dovetail;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Infrastructure;

namespace TestProject
{
    public partial class Program
    {
    }
}
"
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Should_Support_Imports_And_Exports_In_The_Same_Assembly_If_Not_Exported()
    {
        var result = await Builder
                          .AddSources(
                               @"
using Joinery.Dovetail;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Infrastructure;
using Joinery.Dovetail.Tests;

namespace Joinery.Dovetail.Tests
{
    [DovetailExport]
    internal class Contrib : IDovetailJoint { }
}
",
                               @"
using Joinery.Dovetail;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Infrastructure;

namespace TestProject
{
    public partial class Program
    {
    }
}
"
                           )
                          .AddGlobalOption("build_property.DovetailExportAssembly", "false")
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    [MethodDataSource(nameof(Should_Generate_Static_Assembly_Methods_For_Runnable_Projects_Data))]
    public async Task Should_Generate_Static_Assembly_Methods_For_Runnable_Projects(ImmutableArray<Type> referencedTypes)
    {
        var result = await WithGenericSharedDeps()
                          .AddReferences(referencedTypes.ToArray())
                          .AddSources(
                               @"
using Joinery.Dovetail;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Infrastructure;

"
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result).UseParameters(Regex.Replace(Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(string.Join("_", referencedTypes.Select(z => z.Name))))), "[^\\d|\\w]", "").ToLowerInvariant());
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task Should_Generate_Static_Assembly_Initializer_When_xunit_is_referenced(bool isTestProject)
    {
        var result = await WithGenericSharedDeps()
                          .AddSources(
                               @"
using Joinery.Dovetail;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Infrastructure;

"
                           )
                          .AddGlobalOption("build_property.IsTestProject", isTestProject ? "true" : "false")
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result).UseParameters(isTestProject);
    }

    public static IEnumerable<object[]> Should_Generate_Static_Assembly_Methods_For_Runnable_Projects_Data()
    {
        yield return [ImmutableArray.CreateRange([typeof(DovetailDistributedApplicationHelpers), typeof(IDistributedApplicationBuilder)])];
        yield return [ImmutableArray.CreateRange([typeof(DovetailDistributedApplicationTestingHelpers), typeof(IDistributedApplicationTestingBuilder)])];
        yield return [ImmutableArray.CreateRange([typeof(DovetailHostingContextExtensions), typeof(WebAssemblyHostBuilder)])];
        yield return [ImmutableArray.CreateRange([typeof(DovetailHostingContextExtensions), typeof(HostApplicationBuilder)])];
        yield return [ImmutableArray.CreateRange([typeof(DovetailHostingContextExtensions), typeof(WebApplicationBuilder)])];
        yield return
        [
            ImmutableArray.CreateRange(
                [
                    typeof(DovetailDistributedApplicationHelpers), typeof(IDistributedApplicationBuilder),
                    typeof(ILogger),
                    typeof(DovetailContext),
                ]
            ),
        ];
        yield return
        [
            ImmutableArray.CreateRange(
                [
                    typeof(DovetailDistributedApplicationTestingHelpers), typeof(IDistributedApplicationTestingBuilder),
                    typeof(ILogger),
                    typeof(DovetailContext),
                ]
            ),
        ];
        yield return
        [
            ImmutableArray.CreateRange(
                [
                    typeof(DovetailHostingContextExtensions), typeof(WebAssemblyHostBuilder),
                    typeof(ILogger),
                    typeof(DovetailContext),
                ]
            ),
        ];
        yield return
        [
            ImmutableArray.CreateRange(
                [
                    typeof(DovetailHostingContextExtensions), typeof(HostApplicationBuilder),
                    typeof(ILogger),
                    typeof(DovetailContext),
                ]
            ),
        ];
        yield return
        [
            ImmutableArray.CreateRange(
                [
                    typeof(DovetailHostingContextExtensions), typeof(WebApplicationBuilder),
                    typeof(ILogger),
                    typeof(DovetailContext),
                ]
            ),
        ];
    }

    [Before(Test)]
    public Task InitializeAsync()
    {
        Configure(b => b.IgnoreOutputFile("Exported_Dovetails.cs"));
        return Task.CompletedTask;
    }
}
