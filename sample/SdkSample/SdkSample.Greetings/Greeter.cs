namespace SdkSample.Greetings;

internal sealed class Greeter(string message) : IGreeter
{
    public string Greet(string name) => $"{message}, {name}!";
}
