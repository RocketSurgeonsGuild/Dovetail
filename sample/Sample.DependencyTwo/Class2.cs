using Dovetail.Attributes;
using Dovetail.Infrastructure;
using FluentValidation;

namespace Sample.DependencyTwo;

public static class Nested
{
    [DovetailExport]
    public class Class2 : IDovetailJoint;
}

public static class Example2
{
    public record Request(string A, double B);

    private class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.A).NotEmpty();
            RuleFor(x => x.B).GreaterThan(0);
        }
    }
}
