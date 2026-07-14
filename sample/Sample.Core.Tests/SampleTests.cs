namespace Sample.Core.Tests;

#region codeblock

public class SampleTests
{
    [Test]
    public async Task Should_Register_Services()
    {
        var context = await DovetailContext.FromAsync(_builder);

        // var services = ( await new ServiceCollection().ApplyJointAsync(context) ).BuildServiceProvider();
        // await Assert.That(services.GetRequiredService<IService>().GetString()).IsEqualTo("TestService");
    }

    public SampleTests() => _builder = DovetailContextBuilder.Create([], new Dictionary<object, object>(), []).Set(DovetailHostType.UnitTest);

    private readonly DovetailContextBuilder _builder;
}

#endregion
