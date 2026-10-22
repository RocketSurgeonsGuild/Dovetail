namespace Sample.Maui;

public static class MauiProgram
{
    // UseDovetail applies the app's (and its references') conventions - configuration from the embedded appsettings files,
    // services, logging - and builds the app. It blocks, because CreateMauiApp can't be async.
    public static MauiApp CreateMauiApp() =>
        MauiApp
           .CreateBuilder()
           .UseMauiApp<App>()
           .ConfigureFonts(
                fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                }
            )
           .UseDovetail();
}
