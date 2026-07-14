---
title: Defining Dovetails
description: How to create and register your own conventions, including ordering and host type filtering.
---

# Defining Dovetails

As stated [previously](./defining-conventions.md) conventions are nothing more than a class that implements <xref:Dovetail.Infrastructure.IDovetailJoint>.

## Implementing Multiple Dovetails

A defined convention can implement as many conventions as you want. Configuration, Services, Command Line, Custom Dovetails can all be be handled by one convention.

```c#
public class MultipleDovetail : IServiceJoint, IConfigurationJoint
{
    public void Register(IDovetailContext context, IConfiguration configuration, IServiceCollection services) { }
    public void Register(IDovetailContext context, IConfiguration configuration, IConfigurationBuilder builder) { }
}
```

## Dovetail Ordering

By default the order of conventions is [non-deterministic](https://en.m.wikipedia.org/wiki/Nondeterministic_algorithm) and not controlled. Dovetails are "left in place" such that once the list of conventions is created it is no changed between convention runs.

Sometimes you want to ensure that one convention runs before another, that can be done using different attributes. Delegate
conventions _cannot_ be ordered in this manner and must be sorted manually.

> [!NOTE]
> Sorting is done using a [Topological sort](https://en.wikipedia.org/wiki/Topological_sorting) and if you define a cycle (A requires B, B requires A)
> then a `NotSupportedException` will be thrown.

### [[BeforeDovetail]](xref:Dovetail.BeforeJointAttribute) / [[DependentOfDovetail]](xref:Dovetail.DependentOfJointAttribute)

This attribute can be used to ensure that your convention is called before another convention. The order is still non-deterministic but
Dovetail will ensure that the defined convention is run before the convention type defined by the attribute.

> [!NOTE]
> Multiple dependencies can be defined.

```c#
// A before B
[BeforeDovetail(typeof(DovetailB))]
public class DovetailA
{
}

public class DovetailB
{
}
```

### [[AfterDovetail]](xref:Dovetail.AfterJointAttribute) / [[DependsOnDovetail]](xref:Dovetail.DependsOnJointAttribute)

This attribute can be used to ensure that your convention is called after another convention. The order is still non-deterministic but
Dovetail will ensure that the defined convention is run after the convention type defined by the attribute.

> [!NOTE]
> Multiple dependencies can be defined.

```c#
// B before A
[AfterDovetail(typeof(DovetailB))]
public class DovetailA
{
}

public class DovetailB
{
}
```

### Custom Attributes

If you want you can define your own custom attribute for use with convention ordering. Your attribute must implement <xref:Dovetail.IDovetailDependency>
and specify the <xref:Dovetail.DependencyDirection>.

## Dovetail Host Type

You can define a convention that only runs for a given <xref:Dovetail.HostType>. The HostType can be defined on the
<xref:Dovetail.DovetailContextBuilder> using `Set<DovetailHostType>(DovetailHostType.UnitTest)`. Using the `DovetailHostType` an assembly
can define different behaviors given the context.

A good example of this is designing a system that uses [NodaTime](https://nodatime.org/). You can have a [[LiveDovetail]](xref:Dovetail.LiveJointAttribute) that registers
`IClock` using the expected calendar system, and another [[UnitTestDovetail]](xref:Dovetail.UnitTestJointAttribute) that registers a `FakeClock`
with a predefined starting date and time.

> [!NOTE]
> Currently only three host types are defined

- `Undefined` - The default host type, means the convention runs everywhere.
- `Live` - This convention applies in a live running application
- `UnitTest` - This convention applies to unit tests.

> [!NOTE]
> There is no attribute for the `DovetailHostType.Undefined`.

### [[LiveDovetail]](xref:Dovetail.LiveJointAttribute)

This attribute ensures a convention that only runs when the `DovetailHostType` is `Live`.

```c#
[LiveDovetail]
public class DovetailA
{
}
```

### [[UnitTestDovetail]](xref:Dovetail.UnitTestJointAttribute)

This attribute ensures a convention that only runs when the `DovetailHostType` is `UnitTest`.

```c#
[UnitTestDovetail]
public class DovetailA
{
}
```

## Injecting dependencies

By default conventions are a simple class that can be created using `Activator.CreateInstance` however the goal of conventions was to allow "bootstrap time"
configuration to be provided. The <xref:Dovetail.DovetailContextBuilder> acts as a bag of properties using [`indexer`](xref:Dovetail.DovetailContextBuilder#Dovetail_DovetailContextBuilder_Item_System_Object_) or [`Properties`](xref:Dovetail.DovetailContextBuilder#Dovetail_DovetailContextBuilder_Properties).

Any type defined in the context is injectable into any defined convention. This can be used for any number of purposes such as...

- allow consumers to provide configuration to your convention
- allow conventions to communicate or share common state

Under the covers the [`Properties`](xref:Dovetail.DovetailContextBuilder#Dovetail_DovetailContextBuilder_Properties) implements `IServiceProvider`
and we simply activate conventions using `ActivatorUtilities.CreateInstance`.

> [!NOTE]
> All dependencies must be registered into the builder, as activation happens while <xref:Dovetail.IDovetailContext> is built.

---

```c#
public class InjectableDovetail : IServiceJoint
{
    private readonly IInjectData _convention;

    public InjectableDovetail(IInjectData convention) => _convention = convention;

    public void Register(IDovetailContext context, IConfiguration configuration, IServiceCollection services)
    {
        //...
    }
}

public class OptionalInjectableDovetail : IServiceJoint
{
    private readonly IInjectData? _convention;

    public OptionalInjectableDovetail(IInjectData? convention = null) => _convention = convention;

    public void Register(IDovetailContext context, IConfiguration configuration, IServiceCollection services)
    {
        if (_convention is { })
        {
            //...
        }
    }
}
```
