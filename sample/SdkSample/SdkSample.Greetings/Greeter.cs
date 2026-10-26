using Microsoft.Extensions.Options;

namespace SdkSample.Greetings;

internal sealed class Greeter(IOptions<GreetingsOptions> greetings, IOptions<FarewellOptions> farewell) : IGreeter
{
    public string Greet(string name)
    {
        // No fallback on purpose: the message only exists if the SDK's generated YAML configuration joint
        // loaded SdkSample.Greetings.yaml, so a silent default would hide a broken pipeline.
        var options = greetings.Value;
        if (string.IsNullOrEmpty(options.Message))
            throw new InvalidOperationException("'Greetings:Message' is not configured - SdkSample.Greetings.yaml was not loaded into the host configuration.");

        return string.Join(" ", Enumerable.Repeat($"{options.Message}, {name}!", options.Repeat));
    }

    public string Farewell(string name)
    {
        var options = farewell.Value;
        return $"{options.Message}, {name}{new string('!', options.Exclamations)}";
    }
}
