# Defining Dovetails

# Defining Dovetail Joints

As stated previously joints are nothing more than a class that implements <xref:Joinery.Dovetail.Infrastructure.IDovetailJoint>.

## Implementing Multiple Dovetail Joints

A defined joint can implement as many joints as you want. Configuration, Services, Command Line, Custom Dovetail Joints can all be handled by one joint.

```c#
public class MultipleDovetail : IServiceJoint, IConfigurationJoint
{
    public void Register(IDovetailContext context, IConfiguration configuration, IServiceCollection services) { }
    public void Register(IDovetailContext context, IConfiguration configuration, IConfigurationBuilder builder) { }
}
```

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

## Injecting dependencies

By default joints are a simple class that can be created using `Activator.CreateInstance` however the goal of joints was to allow "bootstrap time"
configuration to be provided. The <xref:Joinery.Dovetail.DovetailContextBuilder> acts as a bag of properties using [`indexer`](xref:Joinery.Dovetail.DovetailContextBuilder#Joinery_Dovetail_DovetailContextBuilder_Item_System_Object_) or [`Properties`](xref:Joinery.Dovetail.DovetailContextBuilder#Joinery_Dovetail_DovetailContextBuilder_Properties).

Any type defined in the context is injectable into any defined joint. This can be used for any number of purposes such as...

- allow consumers to provide configuration to your joint
- allow joints to communicate or share common state

Under the covers the [`Properties`](xref:Joinery.Dovetail.DovetailContextBuilder#Joinery_Dovetail_DovetailContextBuilder_Properties) implements `IServiceProvider`
and we simply activate joints using `ActivatorUtilities.CreateInstance`.

> [!NOTE]
> All dependencies must be registered into the builder, as activation happens while <xref:Joinery.Dovetail.IDovetailContext> is built.

---

```c#
public class InjectableDovetail : IServiceJoint
{
    private readonly IInjectData _joint;

    public InjectableDovetail(IInjectData joint) => _joint = joint;

    public void Register(IDovetailContext context, IConfiguration configuration, IServiceCollection services)
    {
        //...
    }
}

public class OptionalInjectableDovetail : IServiceJoint
{
    private readonly IInjectData? _joint;

    public OptionalInjectableDovetail(IInjectData? joint = null) => _joint = joint;

    public void Register(IDovetailContext context, IConfiguration configuration, IServiceCollection services)
    {
        if (_joint is { })
        {
            //...
        }
    }
}
```