---
title: Defining Dovetails
description: How to create and register your own joints, including ordering and host type filtering.
---

# Defining Dovetail Joints

As stated previously joints are nothing more than a class that implements <xref:Joinery.Dovetail.Infrastructure.IDovetailJoint>.

## Declaring a new kind of joint

Everything below is about writing joints for kinds that already exist, such as `IServiceJoint` or `ISerilogJoint`. A package or application
adds a new kind, a typed extension point other code can contribute to, with a single `<DovetailJoint>` item in its project file; the build
generates the interface, the delegates and the `Apply` and `DovetailContextBuilder` methods for it. The
[`DovetailJoint` reference](../../reference/dovetail-joint/) lists everything the item accepts. The entry points that run the joints come from a
[`DovetailHost`](../../reference/dovetail-host/) item, which the SDK declares for most projects.

## Implementing Multiple Dovetail Joints

A defined joint can implement as many joints as you want. Configuration, Services, Command Line, Custom Dovetail Joints can all be handled by one joint.

```c#
public class MultipleDovetail : IServiceJoint, IConfigurationJoint
{
    public void Register(IDovetailContext context, IServiceCollection services) { }
    public void Register(IDovetailContext context, IConfigurationBuilder builder) { }
}
```

### What `Register` takes

The build generates each joint interface from its [`<DovetailJoint>`](../../reference/dovetail-joint/) declaration, so what `Register` takes
depends on the kind: the context first, then whatever the kind hands over.

| Kind          | Interface                  | `Register`                                                          |
| ------------- | -------------------------- | ------------------------------------------------------------------- |
| Setup         | `ISetupJoint`              | `Register(IDovetailContext context)`                                |
| Service       | `IServiceJoint`            | `Register(IDovetailContext context, IServiceCollection services)`   |
| Configuration | `IConfigurationJoint`      | `Register(IDovetailContext context, IConfigurationBuilder builder)` |
| Logging       | `ILoggingJoint`            | `Register(IDovetailContext context, ILoggingBuilder builder)`       |
| Host created  | `IHostCreatedJoint<THost>` | `Register(IDovetailContext context, THost host)`                    |

Every kind has an async twin, such as `IServiceAsyncJoint`, whose `Register` returns a `ValueTask` and takes a trailing
`CancellationToken cancellationToken = default`.

None of Dovetail's own kinds hands over an `IConfiguration`. A joint that needs the configuration reads `context.Configuration`. A kind that
is declared with `ProvideConfiguration="true"` does get an `IConfiguration configuration` parameter, after the context; the
[`DovetailJoint` reference](../../reference/dovetail-joint/#the-parameter-lists) lists every parameter a kind can have, in the order they come in.

## Dovetail Ordering

By default the order of joints is [non-deterministic](https://en.m.wikipedia.org/wiki/Nondeterministic_algorithm) and not controlled. Dovetails are "left in place" such that once the list of joints is created it is not changed between runs.

Sometimes you want to ensure that one joint runs before another, that can be done using different attributes. Delegate
joints _cannot_ be ordered in this manner and must be sorted manually.

> [!NOTE]
> Sorting is done using a [Topological sort](https://en.wikipedia.org/wiki/Topological_sorting) and if you define a cycle (A requires B, B requires A)
> then a `NotSupportedException` will be thrown.

### [[BeforeDovetail]](xref:Joinery.Dovetail.BeforeJointAttribute) / [[DependentOfDovetail]](xref:Joinery.Dovetail.DependentOfJointAttribute)

This attribute can be used to ensure that your joint is called before another joint. The order is still non-deterministic but
Dovetail will ensure that the defined joint is run before the joint type defined by the attribute.

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

### [[AfterDovetail]](xref:Joinery.Dovetail.AfterJointAttribute) / [[DependsOnDovetail]](xref:Joinery.Dovetail.DependsOnJointAttribute)

This attribute can be used to ensure that your joint is called after another joint. The order is still non-deterministic but
Dovetail will ensure that the defined joint is run after the joint type defined by the attribute.

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

If you want you can define your own custom attribute for use with joint ordering. Your attribute must implement <xref:Joinery.Dovetail.IDovetailDependency>
and specify the <xref:Joinery.Dovetail.DependencyDirection>.

## Dovetail Host Type

You can define a joint that only runs for a given <xref:Joinery.Dovetail.HostType>. The HostType can be defined on the
<xref:Joinery.Dovetail.DovetailContextBuilder> using `Set<DovetailHostType>(DovetailHostType.UnitTest)`. Using the `DovetailHostType` an assembly
can define different behaviors given the context.

The generated `Imports` class sets the host type only when the build specifies one. The `DovetailHostType` MSBuild property
(`Live` or `UnitTest`) takes precedence; otherwise test projects (`IsTestProject`) get `UnitTest`. Anywhere else the host type
is left unset, and the hosting extensions set it to `Live` if nothing else has.

A good example of this is designing a system that uses [NodaTime](https://nodatime.org/). You can have a [[LiveJoint]](xref:Joinery.Dovetail.LiveJointAttribute) that registers
`IClock` using the expected calendar system, and another [[UnitTestJoint]](xref:Joinery.Dovetail.UnitTestJointAttribute) that registers a `FakeClock`
with a predefined starting date and time.

> [!NOTE]
> Currently only three host types are defined

- `Undefined` - The default host type, means the joint runs everywhere.
- `Live` - This joint applies in a live running application
- `UnitTest` - This joint applies to unit tests.

> [!NOTE]
> There is no shortcut attribute for `DovetailHostType.Undefined`. A joint without a host type attribute already runs everywhere.

### [[LiveJoint]](xref:Joinery.Dovetail.LiveJointAttribute)

This attribute ensures a joint that only runs when the `DovetailHostType` is `Live`.

```c#
[LiveJoint]
public class DovetailA
{
}
```

### [[UnitTestJoint]](xref:Joinery.Dovetail.UnitTestJointAttribute)

This attribute ensures a joint that only runs when the `DovetailHostType` is `UnitTest`.

```c#
[UnitTestJoint]
public class DovetailA
{
}
```

### [DovetailHostType]

The general form of the two attributes above. It takes the host type the joint applies to, for example
`[DovetailHostType(DovetailHostType.Live)]`. It is also the only way to declare `Undefined` explicitly.

### Which attributes are honoured where

A joint reaches a context by one of two paths, and both honour all three attributes the same way:

| Path                                                                                                                                 | `[LiveJoint]` / `[UnitTestJoint]` | `[DovetailHostType]` |
| ------------------------------------------------------------------------------------------------------------------------------------ | --------------------------------- | -------------------- |
| Runtime reflection: `DovetailContextBuilder.AppendJoint(...)` / `PrependJoint(...)` with a joint instance or `AppendJoint<T>()`      | Honoured                          | Honoured             |
| Generated metadata: the joints marked `[DovetailExport]`, which the source generator lists in the generated `Exports` class at build | Honoured                          | Honoured             |

On both paths:

- `[LiveJoint]` and `[UnitTestJoint]` are read before `[DovetailHostType]`, so `[LiveJoint]` wins over a `[DovetailHostType]` on the same joint.
  Declaring both `[LiveJoint]` and `[UnitTestJoint]` on one joint isn't meaningful, and which of them applies is not specified.
- The attributes are inherited. A joint that derives from a `[LiveJoint]` class is a live joint too, and the closest declaration wins.
- A joint that declares nothing has the `Undefined` host type and applies everywhere, apart from the test project default below.
- A joint applies when its host type is `Undefined` or equals the host type of the context.

The generated path adds one default of its own. In a test project (`IsTestProject`), an exported joint that declares no host
type is exported as `UnitTest`. A joint's own attribute always takes precedence over that default, so a `[LiveJoint]` in a test
project stays live-only and `[DovetailHostType(DovetailHostType.Undefined)]` opts a joint out of the default.
The runtime reflection path has no such default.

## Dovetail Category

Every joint belongs to a category, a `DovetailCategory`, which is part of the joint's metadata. A joint declares its category with
`[DovetailCategory]`. The attribute takes a string: `DovetailCategory.Application` and `DovetailCategory.Core` are constants for the two
well-known categories, and any other string is a category of its own.

```c#
[DovetailExport]
[DovetailCategory(DovetailCategory.Core)]
public class TimeJoint : IServiceJoint
{
    //...
}
```

A joint reaches a context by one of two paths, and both give it the same category:

- A joint that declares none is in the `Application` category.
- A joint that declares `[DovetailCategory(X)]` is in category `X`, whether it is added at runtime with `AppendJoint(...)` / `PrependJoint(...)`
  or exported with `[DovetailExport]`. The generated `Exports` class writes the category into each joint's metadata as plain C#, the constant
  for `Application` and `Core` and a string literal for any other, so the exported joint above is described with category `Core`. Nothing is
  reflected on, and a joint that takes constructor parameters (created by a factory) gets its category the same way.
- The attribute is inherited. A joint that derives from a `[DovetailCategory]` class is in that category too, and the closest declaration wins.
  A `[DovetailCategory]` on an interface the joint implements doesn't count, because looking up an attribute on a type reads its base classes
  and not its interfaces.
- The category is independent of the host type above; a joint can declare both.

## Injecting dependencies

By default joints are a simple class that can be created using `Activator.CreateInstance` however the goal of joints was to allow "bootstrap time"
configuration to be provided. The <xref:Joinery.Dovetail.DovetailContextBuilder> acts as a bag of properties using [`indexer`](xref:Joinery.Dovetail.DovetailContextBuilder#Joinery_Dovetail_DovetailContextBuilder_Item_System_Object_) or [`Properties`](xref:Joinery.Dovetail.DovetailContextBuilder#Joinery_Dovetail_DovetailContextBuilder_Properties).

Any type defined in the context is injectable into any defined joint. This can be used for any number of purposes such as...

- allow consumers to provide configuration to your joint
- allow joints to communicate or share common state

Under the covers the [`Properties`](xref:Joinery.Dovetail.DovetailContextBuilder#Joinery_Dovetail_DovetailContextBuilder_Properties) implements `IServiceProvider`,
and a joint's constructor parameters are resolved from it.

> [!NOTE]
> All dependencies must be registered into the builder, as activation happens while <xref:Joinery.Dovetail.IDovetailContext> is built.

---

```c#
public class InjectableDovetail : IServiceJoint
{
    private readonly IInjectData _joint;

    public InjectableDovetail(IInjectData joint) => _joint = joint;

    public void Register(IDovetailContext context, IServiceCollection services)
    {
        //...
    }
}

public class OptionalInjectableDovetail : IServiceJoint
{
    private readonly IInjectData? _joint;

    public OptionalInjectableDovetail(IInjectData? joint = null) => _joint = joint;

    public void Register(IDovetailContext context, IServiceCollection services)
    {
        if (_joint is { })
        {
            //...
        }
    }
}
```

### How constructor parameters are resolved

A joint takes its constructor parameters from the properties of the <xref:Joinery.Dovetail.DovetailContextBuilder> that holds it, in the same way
whether the joint is added with `AppendJoint<T>()` or exported with `[DovetailExport]`:

- **By type.** Each parameter is looked up under its declared type, exactly: a value stored with `Set<IClock>(clock)` satisfies an `IClock`
  parameter and not a `SystemClock` one, and the other way round. `?` on a reference type changes nothing, so `IInjectData?` and
  `IInjectData` find the same value. A nullable value type is looked up as itself: an `int?` parameter is found under `int?`, not `int`.
- **Optional parameters fall back to their default.** A parameter with a default value (`IInjectData? joint = null`, `int retries = 3`) gets that
  default when the properties hold nothing of its type.
- **Required parameters must be there.** A parameter without a default whose type is missing makes creating the context throw an
  `InvalidOperationException`. For an exported joint its message names the missing type, the parameter and the joint. When the context has two or
  more joints it orders them with a comparer, and .NET wraps whatever a comparer throws, so the exception you catch is an
  `InvalidOperationException` that says "Failed to compare two elements in the array." and the one that names the missing type is its `InnerException`.
  With a single joint the exception is not wrapped.
- **At the point the joint is created, not before.** The joint is created the first time something needs it, which is while
  <xref:Joinery.Dovetail.IDovetailContext> is built, so anything you `Set` on the builder up to that point is seen, including after the joint was
  added. Values stored later, for example by another joint while it runs, are too late. `ExceptJoint(Type)` needs the joints too, so it creates
  them when it is called. With two or more joints they are created while the context orders them, before their host types are applied, so a missing
  required value can fail the context even for a joint that would not have applied.
- **One constructor.** An exported joint is created by calling one constructor that is public or internal (`protected internal` counts, the
  generated code lives in the same assembly). A joint with more than one of those is the `DT0001` error. A joint that has constructors but none of
  those, for example a single `private` or `protected` one, is the `DT0017` error: it is reported at build time and left out of the exports. Private and
  protected constructors, and a static constructor, are not counted for `DT0001`.

For an exported joint the generator writes this lookup into the `Exports` class as plain C#: the exported method itself takes no parameters, and
each joint with constructor parameters is described by a factory that the context builder runs against its properties. Nothing is reflected on, so it is
trimming and AOT safe. A joint without constructor parameters is created directly. The same factory is used if you read the `Joint` of that metadata
before any builder has it, with the difference that there are no properties then: optional parameters get their defaults and a required one throws.
(Joints you add with `AppendJoint<T>()` are created with `ActivatorUtilities`, which does use reflection.)

```c#
// LaunchPad: a joint that tunes itself from options the host may or may not provide.
[DovetailExport]
public class TimeJoint(PrimitiveOptions? options = null) : IServiceJoint
{
    private readonly PrimitiveOptions _options = options ?? new PrimitiveOptions();

    public void Register(IDovetailContext context, IServiceCollection services)
    {
        //...
    }
}
```

With this set on the `DovetailContextBuilder`, `TimeJoint` gets this instance; without it, `TimeJoint` gets null and uses its own default:

```c#
builder.Set(new PrimitiveOptions { /* ... */ });
```
