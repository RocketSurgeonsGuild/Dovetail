#if NET11_0_OR_GREATER
using System.Text.RegularExpressions;
using Microsoft.Playwright;

namespace Joinery.Dovetail.WebAssembly.Hosting.Tests;

/// <summary>
///     The Blazor WebAssembly half of the LaunchPad.Telemetry parity check (docs: concepts/opentelemetry-launchpad-parity): what the
///     sample's OpenTelemetry parity page (<c>/telemetry-parity</c>) shows after the browser has run it. Tracing and metrics are in
///     <see cref="BrowserHostingTests" />.
/// </summary>
[ClassDataSource<PlaywrightFixture>(Shared = SharedType.PerClass)]
public class BrowserOpenTelemetryTests(PlaywrightFixture fixture)
{
    /// <summary>
    ///     The application's <c>ILogger</c> output reaches the sample's log exporter through the OpenTelemetry logging provider, which
    ///     the sample's logger joint configures, with the category, level and state of the record and the resource its resource joint
    ///     describes.
    /// </summary>
    [Test]
    public async Task Should_Export_OpenTelemetry_Logs_Through_The_Logging_Provider()
    {
        var page = await OpenParityPageAsync();

        await Assert.That(await page.Locator("#log").TextContentAsync()).IsEqualTo("exported Sample.BlazorWasm.Parity|Warning|parity {Name}|browser from browser");
    }

    /// <summary>
    ///     LaunchPad's WebAssembly default adds the telemetry SDK attributes to the resource. The SDK's default resource already has them
    ///     (and the environment variable detector, which has nothing to read in the browser), on every signal.
    /// </summary>
    [Test]
    [Arguments("#resource-tracing")]
    [Arguments("#resource-metrics")]
    [Arguments("#resource-logging")]
    public async Task Should_Have_The_Default_Resource_On_Every_Provider(string selector)
    {
        var page = await OpenParityPageAsync();

        var resource = await page.Locator(selector).TextContentAsync();

        await Assert.That(resource).Contains("telemetry.sdk.language=dotnet");
        await Assert.That(resource).Contains("telemetry.sdk.name=opentelemetry");
        await Assert.That(resource).Contains("service.name=unknown_service");
        await Assert.That(resource).Contains("sample.host=browser");
        await Assert.That(Regex.IsMatch(resource!, @"telemetry\.sdk\.version=\d+\.\d+")).IsTrue();
    }

    private async Task<IPage> OpenParityPageAsync()
    {
        var page = await fixture.Browser!.NewPageAsync();
        var browserOutput = new List<string>();
        page.Console += (_, message) => browserOutput.Add($"{message.Type}: {message.Text}");
        page.PageError += (_, error) => browserOutput.Add($"page error: {error}");

        await page.GotoAsync(fixture.Uri + "/telemetry-parity");
        try
        {
            await page.WaitForSelectorAsync("#log:not(:text('waiting'))", new() { Timeout = 60_000, });
        }
        catch (TimeoutException ex)
        {
            throw new TimeoutException($"The parity page did not render. Browser output:{Environment.NewLine}{string.Join(Environment.NewLine, browserOutput)}", ex);
        }

        return page;
    }
}
#endif
