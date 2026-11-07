using Microsoft.Build.Utilities.ProjectCreation;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
///     Joinery.Dovetail writes a service joint into each project that runs a ZeroAlloc source generator
///     (<c>src/Joinery.Dovetail/build/Joinery.Dovetail.ZeroAlloc.targets</c>), which calls the registration method the generator wrote
///     into that project: <c>AddMediator()</c> (with the bridge packages' <c>With...()</c> calls), <c>AddZeroAllocValidators()</c> and
///     <c>AddZeroAllocAspNetCoreValidation()</c>. These build projects with and without the generators, which also proves the joints
///     compile, and run an application whose library has a handler and a validator.
/// </summary>
[TargetFrameworks]
public class ZeroAllocJointTests(string targetFramework)
{
    private const string Mediator = "DovetailZeroAllocMediatorJoint";
    private const string Validators = "DovetailZeroAllocValidatorsJoint";
    private const string AspNetCoreValidation = "DovetailZeroAllocAspNetCoreValidationJoint";

    [Test]
    public async Task WithoutZeroAlloc_EmitsNoJoints()
    {
        using var project = CreateApplication();

        ( await BuildAndReadJoints(project) ).ShouldBeEmpty();
    }

    /// <summary>
    ///     ZeroAlloc.Mediator alone has no <c>AddMediator()</c>: its generator writes that into the project, so the generator is what
    ///     turns the joint on.
    /// </summary>
    [Test]
    public async Task MediatorWithoutItsGenerator_EmitsNoJoint()
    {
        using var project = CreateApplication(app => app.ItemPackageReference("ZeroAlloc.Mediator", Version("ZeroAlloc.Mediator")));

        ( await BuildAndReadJoints(project) ).ShouldBeEmpty();
    }

    [Test]
    public async Task MediatorGenerator_EmitsTheMediatorJoint_WithoutBridges()
    {
        using var project = CreateApplication(WithMediator);

        ( await BuildAndReadJoints(project) ).ShouldBe([Mediator]);
        var joint = ReadJoint(project, Mediator);
        joint.ShouldContain("var mediator = services.AddMediator();");
        joint.ShouldNotContain("(mediator)");
    }

    [Test]
    public async Task MediatorBridges_AreTurnedOnForEachOneReferenced()
    {
        using var project = CreateApplication(
            app => WithMediator(app)
                  .ItemPackageReference("ZeroAlloc.Mediator.Validation", Version("ZeroAlloc.Mediator.Validation"))
                  .ItemPackageReference("ZeroAlloc.Mediator.Cache", Version("ZeroAlloc.Mediator.Cache"))
        );

        ( await BuildAndReadJoints(project) ).ShouldBe([Mediator]);
        var joint = ReadJoint(project, Mediator);
        joint.ShouldContain("global::ZeroAlloc.Mediator.Validation.MediatorValidationServiceCollectionExtensions.WithValidation(mediator);");
        joint.ShouldContain("global::ZeroAlloc.Mediator.Cache.MediatorCacheServiceCollectionExtensions.WithCache(mediator);");
        joint.ShouldNotContain("WithResilience");
        joint.ShouldNotContain("WithTelemetry");
    }

    [Test]
    public async Task ValidationInject_EmitsTheValidatorsJoint()
    {
        using var project = CreateApplication(app => WithValidation(app), ValidatedModel);

        ( await BuildAndReadJoints(project) ).ShouldBe([Validators]);
    }

    [Test]
    public async Task ValidationAspNetCore_EmitsTheAspNetCoreJoint_WhenReferencedDirectly()
    {
        using var project = CreateApplication(
            app => app
                  .ItemInclude("FrameworkReference", "Microsoft.AspNetCore.App")
                  .ItemPackageReference("ZeroAlloc.Validation", Version("ZeroAlloc.Validation"))
                  .ItemPackageReference("ZeroAlloc.Validation.AspNetCore", Version("ZeroAlloc.Validation.AspNetCore")),
            ValidatedModel
        );

        ( await BuildAndReadJoints(project) ).ShouldBe([AspNetCoreValidation]);
    }

    [Test]
    [Arguments("DovetailEnableZeroAllocMediator", new[] { Validators })]
    [Arguments("DovetailEnableZeroAllocValidation", new[] { Mediator })]
    public async Task EnableProperty_False_LeavesThatRegistrationToTheProject(string property, string[] expected)
    {
        using var project = CreateApplication(app => WithValidation(WithMediator(app).Property(property, "false")), ValidatedModel);

        ( await BuildAndReadJoints(project) ).ShouldBe(expected);
    }

    /// <summary>
    ///     A library's handler and validator are registered by the joints written into the library and exported to the application,
    ///     which only calls <c>ConfigureDovetail()</c>: the request is sent through the validation bridge, so an invalid one never
    ///     reaches the handler.
    /// </summary>
    [Test]
    public async Task LibraryHandlersAndValidators_AreRegistered_AndValidatedBeforeTheHandler()
    {
        using var project = new SdkTestProject();
        project.AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"));

        // track: false - built only through the application's ProjectReference (see HostConfigurationPropagationTests).
        var lib = WithValidation(
            WithMediator(ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework))
               .ItemPackageReference("ZeroAlloc.Mediator.Validation", Version("ZeroAlloc.Mediator.Validation"))
        );
        project
           .AddProject("lib/lib.csproj", lib, track: false)
           .AddFile(
                "lib/Greet.cs",
                """
                using System.Threading;
                using System.Threading.Tasks;
                using ZeroAlloc.Mediator;
                using ZeroAlloc.Validation;

                namespace lib;

                [Validate]
                public partial class Greet : IRequest<string>
                {
                    [NotEmpty]
                    public string Name { get; set; } = "";
                }

                public sealed class GreetHandler : IRequestHandler<Greet, string>
                {
                    public ValueTask<string> Handle(Greet request, CancellationToken cancellationToken) => new("hello " + request.Name);
                }

                // ZeroAlloc.Mediator generates IMediator, internal, into the assembly that has the handlers.
                public static class Greeter
                {
                    public static ValueTask<string> Send(System.IServiceProvider services, Greet request) =>
                        Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<IMediator>(services).Send(request);
                }
                """
            );

        var app = ProjectCreator
                 .Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe")
                 .ItemPackageReference("Joinery.Dovetail.Hosting", Config.DovetailPackageVersion("Joinery.Dovetail.Hosting"))
                 .ItemPackageReference("Microsoft.Extensions.Hosting", Config.CentralPackageVersion("Microsoft.Extensions.Hosting", targetFramework))
                 .ItemProjectReference(lib);
        project
           .AddProject("app/app.csproj", app)
           .AddFile(
                "app/Program.cs",
                """
                using app;
                using Joinery.Dovetail;
                using Microsoft.Extensions.DependencyInjection;
                using Microsoft.Extensions.Hosting;
                using ZeroAlloc.Validation;

                var builder = Host.CreateApplicationBuilder(args);
                using var host = await builder.ConfigureDovetail();
                System.Console.WriteLine("validator=" + (host.Services.GetService<ValidatorFor<lib.Greet>>() is not null));
                System.Console.WriteLine(await lib.Greeter.Send(host.Services, new lib.Greet { Name = "dovetail" }));
                try
                {
                    System.Console.WriteLine("invalid=" + await lib.Greeter.Send(host.Services, new lib.Greet()));
                }
                catch (System.Exception exception)
                {
                    System.Console.WriteLine("invalid threw " + exception.GetType().Name);
                }
                """
            );

        var lines = ( await project.Run("app") ).Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        lines.ShouldContain("validator=True");
        lines.ShouldContain("hello dovetail");
        lines.ShouldNotContain("invalid=hello ");
        lines.ShouldContain(line => line.StartsWith("invalid threw ", StringComparison.Ordinal));
    }

    private ProjectCreator WithMediator(ProjectCreator project) =>
        project
           .ItemPackageReference("ZeroAlloc.Mediator", Version("ZeroAlloc.Mediator"))
           .ItemPackageReference("ZeroAlloc.Mediator.Generator", Version("ZeroAlloc.Mediator.Generator"), privateAssets: "all")
           // ZeroAlloc.Mediator depends on Microsoft.Extensions.DependencyInjection.Abstractions 10.0.14, newer than some frameworks'.
           .ItemPackageReference("Microsoft.Extensions.DependencyInjection", Config.CentralPackageVersion("Microsoft.Extensions.DependencyInjection", targetFramework));

    private ProjectCreator WithValidation(ProjectCreator project) =>
        project
           .ItemPackageReference("ZeroAlloc.Validation", Version("ZeroAlloc.Validation"))
           .ItemPackageReference("ZeroAlloc.Validation.Inject", Version("ZeroAlloc.Validation.Inject"), privateAssets: "all");

    /// <summary>A model with a generated validator, for the projects that run the ZeroAlloc.Validation generators.</summary>
    private const string ValidatedModel = """
                                          using ZeroAlloc.Validation;

                                          namespace app;

                                          [Validate]
                                          public partial class Order
                                          {
                                              [NotEmpty]
                                              public string Id { get; set; } = "";
                                          }
                                          """;

    private static string Version(string packageId) => Config.CentralPackageVersion(packageId, "net10.0");

    private SdkTestProject CreateApplication(Func<ProjectCreator, ProjectCreator>? configure = null, string? source = null)
    {
        var app = ProjectCreator
                 .Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe")
                 .ItemPackageReference("Joinery.Dovetail.Hosting", Config.DovetailPackageVersion("Joinery.Dovetail.Hosting"));
        app = configure?.Invoke(app) ?? app;

        var project = new SdkTestProject();
        project
           .AddProject("app/app.csproj", app)
           .AddFile("app/Program.cs", "System.Console.WriteLine(\"hello\");");
        if (source is not null)
            project.AddFile("app/Model.cs", source);
        return project;
    }

    private static async Task<IReadOnlyList<string>> BuildAndReadJoints(SdkTestProject project)
    {
        await project.BuildProject("app");
        return Directory
              .EnumerateFiles(Path.Combine(project.Directory, "app", "obj"), "DovetailZeroAlloc.*.g.cs", SearchOption.AllDirectories)
              .Select(Path.GetFileName)
              .OfType<string>()
              .Select(z => z["DovetailZeroAlloc.".Length..^".g.cs".Length])
              .Distinct(StringComparer.Ordinal)
              .Order(StringComparer.Ordinal)
              .ToArray();
    }

    private static string ReadJoint(SdkTestProject project, string joint) =>
        File.ReadAllText(
            Directory.EnumerateFiles(Path.Combine(project.Directory, "app", "obj"), $"DovetailZeroAlloc.{joint}.g.cs", SearchOption.AllDirectories).First()
        );
}
