using Dovetail.Infrastructure;
using Dovetail.Joints;
using FakeItEasy;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Dovetail.WebAssembly.Hosting.Tests;

public class SharedHostConfigurationTests
{
    [Test]
    public async Task Should_Chain_Dovetail_Configuration_Into_The_Host()
    {
        var configuration = new WebAssemblyHostConfiguration();
        var context = await CreateContextAsync(static (_, builder) => builder.AddInMemoryCollection(new Dictionary<string, string?> { ["Dovetail:Chained"] = "yes", }));

        await DovetailWebAssemblyHelpers.SharedHostConfigurationAsync(context, configuration, CancellationToken.None);

        configuration["Dovetail:Chained"].ShouldBe("yes");
    }

    [Test]
    public async Task Should_Keep_The_Hosts_Json_When_Its_Sources_Are_Read_Only()
    {
        // WebAssemblyHostConfiguration loads appsettings.json itself, and only hands out a read-only copy of its sources.
        var configuration = new WebAssemblyHostConfiguration();
        ( (IConfigurationBuilder)configuration ).Add(new JsonStreamConfigurationSource { Stream = new MemoryStream("""{ "Blazor": "default", "Dovetail": "default" }"""u8.ToArray()), });
        var context = await CreateContextAsync(static (_, builder) => builder.AddInMemoryCollection(new Dictionary<string, string?> { ["Dovetail"] = "dovetail", }));

        await DovetailWebAssemblyHelpers.SharedHostConfigurationAsync(context, configuration, CancellationToken.None);

        configuration["Blazor"].ShouldBe("default");
        configuration["Dovetail"].ShouldBe("dovetail");
    }

#if NET11_0_OR_GREATER
    [Test]
    public async Task Should_Prefer_RSG_Environment_Variables_Over_Dovetail_Configuration()
    {
        const string Variable = "RSG_DovetailWasmTests__FromEnvironment";
        Environment.SetEnvironmentVariable(Variable, "from-rsg");
        try
        {
            var configuration = new WebAssemblyHostConfiguration();
            var context = await CreateContextAsync(
                static (_, builder) => builder.AddInMemoryCollection(new Dictionary<string, string?> { ["DovetailWasmTests:FromEnvironment"] = "from-dovetail", })
            );

            await DovetailWebAssemblyHelpers.SharedHostConfigurationAsync(context, configuration, CancellationToken.None);

            configuration["DovetailWasmTests:FromEnvironment"].ShouldBe("from-rsg");
        }
        finally
        {
            Environment.SetEnvironmentVariable(Variable, null);
        }
    }
#endif

    [Test]
    public async Task Should_Expose_The_WebAssembly_Host_Environment()
    {
        var environment = A.Fake<IWebAssemblyHostEnvironment>();
        var context = await DovetailContext.FromAsync(DovetailContextBuilder.Create([], new DovetailDictionary(), []).Set(environment));

        context.WebAssemblyHostEnvironment.ShouldBeSameAs(environment);
    }

    private static async Task<IDovetailContext> CreateContextAsync(ConfigurationJointDelegate configure) =>
        await DovetailContext.FromAsync(DovetailContextBuilder.Create([], new DovetailDictionary(), []).ConfigureConfiguration(configure));
}
