using SdkSample.Greetings;
using SdkSample.Web;

var builder = WebApplication.CreateBuilder(args);
var app = await builder.ConfigureDovetail();

app.MapGet("/", (IGreeter greeter) => greeter.Greet("SdkSample.Web"));

await app.RunAsync();
