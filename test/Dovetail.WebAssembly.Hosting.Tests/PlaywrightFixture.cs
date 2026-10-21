using System.Reflection;
using Microsoft.Playwright;
using TUnit.Core.Interfaces;
#if NET11_0_OR_GREATER
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
#else
using System.Runtime.ExceptionServices;
using Dovetail.WebAssembly.Hosting.Tests.DevServer;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sample.BlazorWasm;
#endif

namespace Dovetail.WebAssembly.Hosting.Tests;

public sealed class PlaywrightFixture : IAsyncInitializer, IAsyncDisposable
{
    public string Uri { get; set; } = null!;
    public IBrowser? Browser { get; set; } = null!;
    private IPlaywright PlaywrightInstance { get; set; } = null!;

    public async Task InitializeAsync()
    {
        Uri = await StartServerAsync();
        PlaywrightInstance = await Playwright.CreateAsync();
        Browser = await PlaywrightInstance.Chromium.LaunchAsync(new() { Headless = true, });
    }

    public async ValueTask DisposeAsync()
    {
        await StopServerAsync();
        if (Browser is { })
            await Browser.DisposeAsync();
        PlaywrightInstance?.Dispose();
    }

#if NET11_0_OR_GREATER
    private Process? _gateway;

    private async Task<string> StartServerAsync()
    {
        var gatewayDll = GetMetadata("BlazorGatewayDll");
        var intermediateOutputPath = GetMetadata("SampleIntermediateOutputPath");
        var uri = $"http://127.0.0.1:{GetFreePort()}";

        // The same arguments the Gateway's own build targets use for `dotnet run`.
        var startInfo = new ProcessStartInfo(Environment.ProcessPath is { } path && Path.GetFileNameWithoutExtension(path) == "dotnet" ? path : "dotnet")
        {
            ArgumentList =
            {
                gatewayDll,
                "--environment", "Development",
                "--staticWebAssets", Path.Combine(intermediateOutputPath, "staticwebassets.development.json"),
                "--ClientApps:app:EndpointsManifest", Path.Combine(intermediateOutputPath, "staticwebassets.build.endpoints.json"),
                "--ClientApps:app:PathPrefix", "",
                "--urls", uri,
            },
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };
        _gateway = Process.Start(startInfo) ?? throw new InvalidOperationException("Could not start the Blazor Gateway");
        _gateway.OutputDataReceived += static (_, _) => { };
        _gateway.ErrorDataReceived += static (_, _) => { };
        _gateway.BeginOutputReadLine();
        _gateway.BeginErrorReadLine();

        using var http = new HttpClient();
        var deadline = DateTime.UtcNow.AddSeconds(30);
        while (true)
        {
            if (_gateway.HasExited) throw new InvalidOperationException($"The Blazor Gateway exited with code {_gateway.ExitCode}");
            try
            {
                using var response = await http.GetAsync($"{uri}/alive");
                if (response.IsSuccessStatusCode) return uri;
            }
            catch (HttpRequestException) { }

            if (DateTime.UtcNow > deadline) throw new TimeoutException("Timed out waiting for the Blazor Gateway to start");
            await Task.Delay(200);
        }
    }

    private async Task StopServerAsync()
    {
        if (_gateway is not { HasExited: false, }) return;
        _gateway.Kill(true);
        await _gateway.WaitForExitAsync();
        _gateway.Dispose();
    }

    private static string GetMetadata(string key) =>
        typeof(PlaywrightFixture).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>().FirstOrDefault(x => x.Key == key)?.Value
     ?? throw new InvalidOperationException($"{key} not found");

    private static int GetFreePort()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        return ( (IPEndPoint)listener.LocalEndpoint ).Port;
    }
#else
    private static void RunInBackgroundThread(Action action)
    {
        var isDone = new ManualResetEvent(false);

        ExceptionDispatchInfo? edi = null;
        new Thread(
            () =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    edi = ExceptionDispatchInfo.Capture(ex);
                }

                isDone.Set();
            }
        ).Start();

        if (!isDone.WaitOne(TimeSpan.FromSeconds(10))) throw new TimeoutException("Timed out waiting for: " + action);

        if (edi != null) throw edi.SourceException;
    }

    public IHost Host { get; set; } = null!;
    public string ContentRoot { get; private set; } = null!;

    private Task<string> StartServerAsync()
    {
        ContentRoot = typeof(App)
                     .Assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
                     .FirstOrDefault(x => x.Key == "ContentRootPath")
                    ?.Value
         ?? throw new InvalidOperationException("ContentRootPath not found");

        var args = new List<string>
        {
            "--urls", "http://127.0.0.1:0",
            "--contentroot", ContentRoot,
            "--applicationpath", typeof(App).Assembly.Location,
        };

        Host = DevHostServerProgram.BuildWebHost([.. args]);
        RunInBackgroundThread(Host.Start);

        return Task.FromResult(
            Host
               .Services.GetRequiredService<IServer>()
               .Features
               .Get<IServerAddressesFeature>()!
               .Addresses.Single()
        );
    }

    private async Task StopServerAsync()
    {
        await Host.StopAsync();
        Host.Dispose();
    }
#endif
}
