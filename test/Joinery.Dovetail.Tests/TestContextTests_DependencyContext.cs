using Rocket.Surgery.Extensions.Testing;

using Serilog.Events;


using ILogger = Serilog.ILogger;

namespace Joinery.Dovetail.Tests;

public class TestContextTests_DependencyContext
    () : LoggerTest<TestRecord>(TestRecord.Create(LogEventLevel.Information))
{
    [Test]
    public async Task Builder_Should_Create_Host()
    {
        DovetailContextBuilder a() => DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []).Set(Logger);
        var context = await Assert.That(a).ThrowsNothing();
        await context.Get<ILogger>().Should().BeSameReferenceAs(Logger);
    }

    [Test]
    public async Task Builder_Should_Create_Host_ByType()
    {
        DovetailContextBuilder a() => DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []).Set(Logger)
            ;
        await Assert.That(a).ThrowsNothing();
    }
}
