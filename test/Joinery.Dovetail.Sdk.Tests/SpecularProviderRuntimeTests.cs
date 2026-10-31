using Microsoft.Build.Utilities.ProjectCreation;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
///     Dovetail takes no dependency on Specular, yet an application that references it gets the provider Specular generates
///     into the context the generated <c>Imports</c> creates, so a Joint reads it with <c>context.Get&lt;ISpecularProvider&gt;()</c>
///     without a hand-written <c>Set</c>. These build and run an application with the real Specular package, whose generator
///     runs next to Dovetail's and whose <c>SpecularProvider</c> Dovetail's generator never sees as a type.
/// </summary>
[TargetFrameworks]
public class SpecularProviderRuntimeTests(string targetFramework)
{
    [Test]
    public async Task LibraryJoint_ReadsTheApplicationsProvider_AndScansTheReferencedAssemblies()
    {
        using var project = CreateScaffold(
            """
            using app;
            using Joinery.Dovetail;
            using Microsoft.Extensions.Hosting;

            var builder = Host.CreateApplicationBuilder(args);
            using var host = await builder.ConfigureDovetail();
            """
        );

        await project.BuildProject("app");
        var output = await project.Run("app");

        // Nothing was set by hand, and referencing Specular needs no suppressions: the seeding builds against what Specular generates.
        project.Warnings("app").ShouldBeEmpty();
        // The library emits no provider (SpecularEmitProvider=false), so the one it finds is the application's, which scans across assemblies.
        output.ShouldContain("provider=SpecularProvider");
        output.ShouldContain("plugins=IPlugin,PluginOne,PluginTwo");
    }

    [Test]
    public async Task ApplicationThatReachesSpecularOnlyThroughALibrary_GetsTheProviderToo()
    {
        // Specular's generator and its runtime package flow to the application through the library, so the application generates a provider.
        using var project = CreateScaffold(
            """
            using app;
            using Joinery.Dovetail;
            using Microsoft.Extensions.Hosting;

            var builder = Host.CreateApplicationBuilder(args);
            using var host = await builder.ConfigureDovetail();
            """,
            referencesSpecular: false
        );

        var output = await project.Run("app");

        output.ShouldContain("provider=SpecularProvider");
        output.ShouldContain("plugins=IPlugin,PluginOne,PluginTwo");
    }

    [Test]
    public async Task ExplicitSet_WinsOverTheSeededProvider()
    {
        using var project = CreateScaffold(
            """
            using System.Collections.Generic;
            using System.Reflection;
            using System.Runtime.CompilerServices;
            using app;
            using Joinery.Dovetail;
            using Microsoft.Extensions.DependencyInjection;
            using Microsoft.Extensions.Hosting;
            using Specular;
            using Specular.Abstractions;

            var builder = Host.CreateApplicationBuilder(args);
            using var host = await builder.ConfigureDovetail(b => b.Set<ISpecularProvider>(new HandWrittenProvider()));

            internal sealed class HandWrittenProvider : ISpecularProvider
            {
                public IEnumerable<Assembly> GetAssemblies(System.Action<IReflectionAssemblySelector> action, int lineNumber = 0, string filePath = "", string argumentExpression = "") => [];

                public IEnumerable<System.Type> GetTypes(System.Func<IReflectionTypeSelector, IEnumerable<System.Type>> selector, int lineNumber = 0, string filePath = "", string argumentExpression = "") => [typeof(string)];

                public IServiceCollection Scan(IServiceCollection services, System.Action<IServiceDescriptorAssemblySelector> selector, int lineNumber = 0, string filePath = "", string argumentExpression = "") => services;
            }
            """
        );

        var output = await project.Run("app");

        output.ShouldContain("provider=HandWrittenProvider");
        output.ShouldContain("plugins=String");
    }

    /// <summary>
    ///     The seeding is a generic call and a static property read, so it adds no reflection. Published trimmed or with Native AOT (which
    ///     trims and analyzes the application too), the application raises no trim or AOT warnings, still starts with the provider in its
    ///     context, and still scans across assemblies.
    /// </summary>
    [Test]
    [Arguments("PublishTrimmed")]
    [Arguments("PublishAot")]
    public async Task PublishedApplication_ReadsTheSeededProvider_WithoutReflection(string publishProperty)
    {
        using var project = CreateScaffold(
            """
            using app;
            using Joinery.Dovetail;
            using Microsoft.Extensions.Hosting;

            System.Console.WriteLine("dynamicCode=" + System.Runtime.CompilerServices.RuntimeFeature.IsDynamicCodeSupported);
            var builder = Host.CreateApplicationBuilder(args);
            using var host = await builder.ConfigureDovetail();
            """,
            app => app
                  .Property(publishProperty, "true")
                  .Property("SelfContained", "true")
                  .Property("RuntimeIdentifier", "$(NETCoreSdkPortableRuntimeIdentifier)")
        );

        var output = await project.PublishAndRun("app");

        output.ShouldContain("provider=SpecularProvider");
        output.ShouldContain("plugins=IPlugin,PluginOne,PluginTwo");
        // Native AOT has no JIT: this shows the application that ran was the ahead-of-time compiled one.
        output.ShouldContain(publishProperty == "PublishAot" ? "dynamicCode=False" : "dynamicCode=True");
        project.Warnings("app").Where(z => z.StartsWith("IL2", StringComparison.Ordinal) || z.StartsWith("IL3", StringComparison.Ordinal)).ShouldBeEmpty();
    }

    /// <summary>
    ///     An application and a library that each reference Specular, with the library's Joint scanning for the library's plugins.
    ///     The library does not generate a provider of its own.
    /// </summary>
    private SdkTestProject CreateScaffold(string program, Action<ProjectCreator>? configureApp = null, bool referencesSpecular = true)
    {
        var project = new SdkTestProject();
        project.AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"));

        // track: false - built only through the application's ProjectReference (see HostConfigurationPropagationTests).
        var lib = ProjectCreator
                 .Templates.SdkCsproj(targetFramework: targetFramework)
                 .Property("SpecularEmitProvider", "false")
                 .ItemPackageReference("Specular", Config.CentralPackageVersion("Specular", targetFramework))
                 .ItemPackageReference("Microsoft.Extensions.Hosting", Config.CentralPackageVersion("Microsoft.Extensions.Hosting", targetFramework));
        project
           .AddProject("lib/lib.csproj", lib, track: false)
           .AddFile(
                "lib/PluginScanner.cs",
                """
                using System.Linq;
                using Joinery.Dovetail;
                using Joinery.Dovetail.Attributes;
                using Joinery.Dovetail.Joints;
                using Specular;

                namespace lib;

                public interface IPlugin;

                public sealed class PluginOne : IPlugin;

                public sealed class PluginTwo : IPlugin;

                [DovetailExport]
                public sealed class PluginScanner : ISetupJoint
                {
                    public void Register(IDovetailContext context)
                    {
                        // Nothing in the application sets this: Dovetail seeds the provider Specular generated for the application.
                        var provider = context.Get<ISpecularProvider>();
                        System.Console.WriteLine("provider=" + ( provider?.GetType().Name ?? "none" ));
                        if (provider is null) return;

                        var plugins = provider.GetTypes(z => z.FromAssemblyOf<IPlugin>().GetTypes(f => f.AssignableTo<IPlugin>())).Select(z => z.Name).Order();
                        System.Console.WriteLine("plugins=" + string.Join(",", plugins));
                    }
                }
                """
            );

        var app = ProjectCreator
                 .Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe")
                 .ItemPackageReference("Microsoft.Extensions.Hosting", Config.CentralPackageVersion("Microsoft.Extensions.Hosting", targetFramework))
                 .ItemProjectReference(lib);
        if (referencesSpecular) app.ItemPackageReference("Specular", Config.CentralPackageVersion("Specular", targetFramework));
        configureApp?.Invoke(app);
        return project
              .AddProject("app/app.csproj", app)
              .AddFile("app/Program.cs", program);
    }
}
