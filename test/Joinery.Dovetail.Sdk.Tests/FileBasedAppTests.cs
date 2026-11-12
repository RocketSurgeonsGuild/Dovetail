namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// A file-based app (<c>dotnet run app.cs</c>) applies the SDK with a single <c>#:sdk</c> directive: the SDK brings Microsoft.NET.Sdk, which
/// that directive replaces. It is a console application like any other, so it gets the generic host, Joinery.Dovetail.Hosting,
/// Joinery.Dovetail.Serilog and Joinery.Dovetail.OpenTelemetry without a single <c>#:package</c> directive.
/// </summary>
[TargetFrameworks]
public class FileBasedAppTests(string targetFramework)
{
    [Test]
    public async Task FileBasedApp_GetsTheGenericHostSerilogAndOpenTelemetry()
    {
        using var project = new SdkTestProject();
        project.AddFile(
            "app/app.cs",
            $$"""
            #:sdk Joinery.Dovetail.Sdk
            #:property TargetFramework={{targetFramework}}

            using app;

            using var host = await Host.CreateApplicationBuilder(args).ConfigureDovetail();
            System.Console.WriteLine("Serilog=" + typeof(Joinery.Dovetail.Serilog.SerilogJoint).Assembly.GetName().Name);
            System.Console.WriteLine("OpenTelemetry=" + typeof(Joinery.Dovetail.OpenTelemetry.Joints.IResourceBuilderJoint).Assembly.GetName().Name);
            """
        );

        var output = await project.RunFile("app/app.cs");

        output.ShouldContain("Serilog=Joinery.Dovetail.Serilog");
        output.ShouldContain("OpenTelemetry=Joinery.Dovetail.OpenTelemetry");
    }

    /// <summary>
    /// The project of a file-based app is named after the file, extension included (<c>Build.cs</c>). Its <c>appsettings.yaml</c> must still
    /// reach the output directory under the name of its assembly (<c>Build.yaml</c>), which the generated configuration joint requires and
    /// the runtime conventions look for, rather than <c>Build.cs.yaml</c>.
    /// </summary>
    [Test]
    public async Task FileBasedApp_LoadsItsAppSettingsFile()
    {
        using var project = new SdkTestProject();
        project.AddFile(
            "app/Build.cs",
            $$"""
            #:sdk Joinery.Dovetail.Sdk
            #:property TargetFramework={{targetFramework}}
            #:property DovetailEnableYamlConfiguration=true

            {{PrintGreeting}}
            """
        );
        project.AddFile("app/appsettings.yaml", "Greeting: from-appsettings");

        var output = await project.RunFile("app/Build.cs");

        output.ShouldContain("Greeting=from-appsettings");
        output.ShouldContain("Build.yaml=True");
        output.ShouldNotContain("Build.cs.yaml");
    }

    /// <summary>
    /// Without the Dovetail SDK, a file-based app has no default <c>None</c> items, so its <c>appsettings.{ext}</c> files were never copied to
    /// the output directory, and the generated configuration joint failed at startup looking for <c>Build.cs.{ext}</c> there. The packages
    /// now copy them for it, under the name of its assembly.
    /// </summary>
    [Test]
    [Arguments("yaml", "Joinery.Dovetail.Configuration.Yaml", null)]
    [Arguments("yml", "Joinery.Dovetail.Configuration.Yaml", null)]
    [Arguments("json", "Microsoft.Extensions.Configuration.Json", "10.0.9")]
    [Arguments("toml", "Joinery.Dovetail.Configuration.Toml", null)]
    public async Task FileBasedApp_ReferencingThePackages_LoadsItsAppSettingsFile(string extension, string providerPackageId, string? providerVersion)
    {
        using var project = new SdkTestProject();
        project.AddFile(
            "app/Build.cs",
            $$"""
            #:package Joinery.Dovetail.Hosting@{{Config.DovetailPackageVersion("Joinery.Dovetail.Hosting")}}
            #:package {{providerPackageId}}@{{providerVersion ?? Config.DovetailPackageVersion(providerPackageId)}}
            #:package Microsoft.Extensions.Hosting@10.0.9
            #:property TargetFramework={{targetFramework}}

            using Microsoft.Extensions.Hosting;

            {{PrintGreeting}}
            """
        );
        // What the Dovetail SDK (or a package such as Joinery.Dovetail.ModularPipelines) would otherwise declare: the host to configure.
        project.AddFile(
            "app/Directory.Build.props",
            """
            <Project>
              <PropertyGroup>
                <ImportDovetailAssembly>true</ImportDovetailAssembly>
              </PropertyGroup>
              <ItemGroup>
                <DovetailHost Include="HostApplication" HostBuilderType="global::Microsoft.Extensions.Hosting.HostApplicationBuilder" HostBuilderName="builder" HostType="global::Microsoft.Extensions.Hosting.IHost" HelperName="global::Joinery.Dovetail.DovetailHostApplicationHelpers" />
              </ItemGroup>
            </Project>
            """
        );
        project.AddFile(
            $"app/appsettings.{extension}",
            extension switch
            {
                "json" => """{ "Greeting": "from-appsettings" }""",
                "toml" => """Greeting = "from-appsettings" """,
                _ => "Greeting: from-appsettings",
            }
        );

        var output = await project.RunFile("app/Build.cs");

        output.ShouldContain("Greeting=from-appsettings");
        output.ShouldContain($"Build.{extension}=True");
        output.ShouldNotContain($"Build.cs.{extension}");
    }

    /// <summary>
    /// The configuration joints of the provider packages (YamlJoint, JsonJoint, TomlJoint) also load <c>{ApplicationName}.{ext}</c>, the
    /// environment and local variants included, from the application's output directory: for a file-based app that is <c>Build.{ext}</c>,
    /// not <c>Build.cs.{ext}</c>. Dovetail copies only <c>appsettings.{ext}</c> to the output directory (as <c>Build.{ext}</c>, the same
    /// path), so an app that keeps a <c>Build.{ext}</c> of its own copies it there itself, as here.
    /// </summary>
    [Test]
    [Arguments("yaml", "Joinery.Dovetail.Configuration.Yaml")]
    [Arguments("yml", "Joinery.Dovetail.Configuration.Yaml")]
    [Arguments("json", "Joinery.Dovetail.Configuration.Json")]
    [Arguments("toml", "Joinery.Dovetail.Configuration.Toml")]
    public async Task FileBasedApp_ReferencingThePackages_LoadsAConfigurationFileNamedAfterItsAssembly(string extension, string providerPackageId)
    {
        using var project = new SdkTestProject();
        project.AddFile(
            "app/Build.cs",
            $$"""
            #:package Joinery.Dovetail.Hosting@{{Config.DovetailPackageVersion("Joinery.Dovetail.Hosting")}}
            #:package {{providerPackageId}}@{{Config.DovetailPackageVersion(providerPackageId)}}
            #:package Microsoft.Extensions.Hosting@10.0.9
            #:property TargetFramework={{targetFramework}}

            using Microsoft.Extensions.Hosting;

            {{PrintGreeting}}
            """
        );
        project.AddFile(
            "app/Directory.Build.props",
            $"""
            <Project>
              <PropertyGroup>
                <ImportDovetailAssembly>true</ImportDovetailAssembly>
              </PropertyGroup>
              <ItemGroup>
                <DovetailHost Include="HostApplication" HostBuilderType="global::Microsoft.Extensions.Hosting.HostApplicationBuilder" HostBuilderName="builder" HostType="global::Microsoft.Extensions.Hosting.IHost" HelperName="global::Joinery.Dovetail.DovetailHostApplicationHelpers" />
                <None Include="Build.{extension}" CopyToOutputDirectory="PreserveNewest" />
              </ItemGroup>
            </Project>
            """
        );
        project.AddFile(
            $"app/Build.{extension}",
            extension switch
            {
                "json" => """{ "Greeting": "from-build" }""",
                "toml" => """Greeting = "from-build" """,
                _ => "Greeting: from-build",
            }
        );

        var output = await project.RunFile("app/Build.cs");

        output.ShouldContain("Greeting=from-build");
    }

    private const string PrintGreeting = """
        using Build;
        using Microsoft.Extensions.Configuration;
        using Microsoft.Extensions.DependencyInjection;

        using var host = await Host.CreateApplicationBuilder(args).ConfigureDovetail();
        System.Console.WriteLine("Greeting=" + host.Services.GetRequiredService<IConfiguration>()["Greeting"]);
        foreach (var file in System.IO.Directory.EnumerateFiles(System.AppContext.BaseDirectory, "Build.*"))
        {
            System.Console.WriteLine(System.IO.Path.GetFileName(file) + "=True");
        }
        """;
}
