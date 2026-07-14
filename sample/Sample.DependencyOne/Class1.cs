using Dovetail.Attributes;
using Dovetail.Infrastructure;
using FluentValidation;

namespace Sample.DependencyOne;

[DovetailExport]
public class Class1 : IDovetailJoint;

public static class Example1
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
