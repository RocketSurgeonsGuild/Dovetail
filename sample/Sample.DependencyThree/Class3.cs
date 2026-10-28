using FluentValidation;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Infrastructure;
using Sample.DependencyOne;

namespace Sample.DependencyThree;

[DovetailExport]
public class Class3 : IDovetailJoint
{
    public Class1? Class1 { get; set; }
}

public static class Example3
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
