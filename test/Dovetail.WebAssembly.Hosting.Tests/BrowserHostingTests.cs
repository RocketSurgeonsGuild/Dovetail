#if NET11_0_OR_GREATER
using Microsoft.Playwright;
#endif

namespace Dovetail.WebAssembly.Hosting.Tests;

#if NET11_0_OR_GREATER
/// <summary>
///     Runs the sample's net11.0-browser build behind the Blazor Gateway.
/// </summary>
[ClassDataSource<PlaywrightFixture>(Shared = SharedType.PerClass)]
public class BrowserHostingTests(PlaywrightFixture fixture)
{
    private const string BlazorScript = """<script src="_framework/blazor.webassembly.js"></script>""";

    [Test]
    public async Task Should_Start_A_Dovetail_Registered_Hosted_Service()
    {
        var page = await OpenSampleAsync();

        await Assert.That(await page.Locator("#hosted-service").TextContentAsync()).IsEqualTo("started");
    }

    [Test]
    public async Task Should_Load_Dovetail_Configuration()
    {
        var page = await OpenSampleAsync();

        await Assert.That(await page.Locator("#app-settings").TextContentAsync()).IsEqualTo("true");
    }

    [Test]
    public async Task Should_Prefer_RSG_Environment_Variables_Over_Dovetail_Configuration()
    {
        var page = await OpenSampleAsync(("RSG_Sample__FromEnvironment", "from-rsg"));

        await Assert.That(await page.Locator("#environment").TextContentAsync()).IsEqualTo("from-rsg");
    }

    private async Task<IPage> OpenSampleAsync(params (string Name, string Value)[] environmentVariables)
    {
        var page = await fixture.Browser!.NewPageAsync();
        if (environmentVariables is { Length: > 0, })
        {
            // Environment variables reach the WebAssembly runtime through Blazor.start, so start Blazor by hand.
            var variables = string.Concat(environmentVariables.Select(z => $"dotnet.withEnvironmentVariable({Quote(z.Name)}, {Quote(z.Value)});"));
            await page.RouteAsync(
                fixture.Uri + "/",
                async route =>
                {
                    var response = await route.FetchAsync();
                    var body = await response.TextAsync();
                    await Assert.That(body).Contains(BlazorScript);
                    body = body.Replace(
                        BlazorScript,
                        $$"""
                          <script src="_framework/blazor.webassembly.js" autostart="false"></script>
                          <script>Blazor.start({ configureRuntime: dotnet => { {{variables}} } });</script>
                          """
                    );
                    await route.FulfillAsync(new() { Response = response, Body = body, });
                }
            );
        }

        var browserOutput = new List<string>();
        page.Console += (_, message) => browserOutput.Add($"{message.Type}: {message.Text}");
        page.PageError += (_, error) => browserOutput.Add($"page error: {error}");

        await page.GotoAsync(fixture.Uri + "/");
        try
        {
            await page.WaitForSelectorAsync("#hosted-service:not(:text('waiting'))", new() { Timeout = 60_000, });
        }
        catch (TimeoutException ex)
        {
            throw new TimeoutException($"The sample did not render. Browser output:{Environment.NewLine}{string.Join(Environment.NewLine, browserOutput)}", ex);
        }

        return page;
    }

    private static string Quote(string value) => System.Text.Json.JsonSerializer.Serialize(value);
}
#endif
