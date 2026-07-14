using System.Linq.Expressions;
using FluentValidation;
using Rocket.Surgery.Extensions.Testing.SourceGenerators;

namespace Dovetail.Analyzers.Tests;

public static class GenerationHelpers
{
    public static async Task<GeneratorTestResults[]> CreateDeps(GeneratorTestContextBuilder rootBuilder, CancellationToken cancellationToken)
    {
        var baseBuilder = rootBuilder.AddReferences(typeof(IValidator).Assembly, typeof(Expression<>).Assembly);
        var c1 = await Class1(baseBuilder, cancellationToken);
        var c2 = await Class2(baseBuilder, cancellationToken);
        var c3 = await Class3(baseBuilder, c1, cancellationToken);
        return [c1, c2, c3,];
    }

    public static async Task<GeneratorTestResults[]> CreateGenericDeps(GeneratorTestContextBuilder rootBuilder, CancellationToken cancellationToken)
    {
        var baseBuilder = rootBuilder;
        var c1 = await GenericClass1(baseBuilder, cancellationToken);
        var c2 = await GenericClass2(baseBuilder, cancellationToken);
        var c3 = await GenericClass3(baseBuilder, c1, cancellationToken);
        return [c1, c2, c3,];
    }

    public static Task<GeneratorTestResults> Class1(GeneratorTestContextBuilder builder, CancellationToken cancellationToken)
    {
        return builder
              .WithProjectName("SampleDependencyOne")
              .AddDovetailConfiguration(importNamespace: "SampleDependencyOne", exportNamespace: "Dep1", exportClassName: "Dep1Exports")
              .AddReferences(typeof(Expression<>))
              .AddSources(
                   @"using Dovetail;
using Dovetail.Attributes;
using Dovetail.Infrastructure;
using Sample.DependencyOne;
using FluentValidation;


namespace Sample.DependencyOne;

[DovetailExport]
public class Class1 : IDovetailJoint
{
}

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

"
               )
              .Build()
              .GenerateAsync(cancellationToken);
    }

    public static Task<GeneratorTestResults> Class2(GeneratorTestContextBuilder builder, CancellationToken cancellationToken)
    {
        return builder
              .WithProjectName("SampleDependencyTwo")
              .AddDovetailConfiguration(importNamespace: "SampleDependencyTwo", exportNamespace: "", exportClassName: "Dep2Exports")
              .AddReferences(typeof(Expression<>))
              .AddSources(
                   @"using Dovetail;
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
"
               )
              .Build()
              .GenerateAsync(cancellationToken);
    }

    public static Task<GeneratorTestResults> Class3(GeneratorTestContextBuilder builder, GeneratorTestResults class1, CancellationToken cancellationToken)
    {
        return builder
              .WithProjectName("SampleDependencyThree")
              .AddDovetailConfiguration(importNamespace: "SampleDependencyThree", exportNamespace: "SampleDependencyThree.Dovetails")
              .AddReferences(typeof(Expression<>))
              .AddCompilationReferences(class1)
              .AddSources(
                   @"using Dovetail;
using Dovetail.Attributes;
using Dovetail.Infrastructure;
using Sample.DependencyOne;
using Sample.DependencyThree;
using FluentValidation;

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

"
               )
              .Build()
              .GenerateAsync(cancellationToken);
    }

    public static Task<GeneratorTestResults> GenericClass1(GeneratorTestContextBuilder builder, CancellationToken cancellationToken)
    {
        return builder
              .WithProjectName("SampleDependencyOne")
              .AddDovetailConfiguration(importNamespace: "SampleDependencyOne", exportNamespace: "Dep1", exportClassName: "Dep1Exports")
              .AddReferences(typeof(Expression<>))
              .AddSources(
                   @"using Dovetail;
using Dovetail.Attributes;
using Dovetail.Infrastructure;
using Sample.DependencyOne;


namespace Sample.DependencyOne;

[DovetailExport]
public class Class1 : IDovetailJoint
{
}
"
               )
              .Build()
              .GenerateAsync(cancellationToken);
    }

    public static Task<GeneratorTestResults> GenericClass2(GeneratorTestContextBuilder builder, CancellationToken cancellationToken)
    {
        return builder
              .WithProjectName("SampleDependencyTwo")
              .AddDovetailConfiguration(importNamespace: "SampleDependencyTwo", exportNamespace: "", exportClassName: "Dep2Exports")
              .AddReferences(typeof(Expression<>))
              .AddSources(
                   @"using Dovetail;
using Dovetail.Attributes;
using Dovetail.Infrastructure;
using Sample.DependencyTwo;

namespace Sample.DependencyTwo;

[DovetailExport]
public class Class2 : IDovetailJoint
{
}"
               )
              .Build()
              .GenerateAsync(cancellationToken);
    }

    public static Task<GeneratorTestResults> GenericClass3(GeneratorTestContextBuilder builder, GeneratorTestResults class1, CancellationToken cancellationToken)
    {
        return builder
              .WithProjectName("SampleDependencyThree")
              .AddDovetailConfiguration(importNamespace: "SampleDependencyThree", exportNamespace: "SampleDependencyThree.Dovetails")
              .AddReferences(typeof(Expression<>))
              .AddCompilationReferences(class1)
              .AddSources(
                   @"using Dovetail;
using Dovetail.Attributes;
using Dovetail.Infrastructure;
using Sample.DependencyOne;
using Sample.DependencyThree;

namespace Sample.DependencyThree;

[DovetailExport]
public class Class3 : IDovetailJoint
{
    public Class1? Class1 { get; set; }
}
"
               )
              .Build()
              .GenerateAsync(cancellationToken);
    }
}
