# P3: binder, factory and registration joint

Status: done. The generator side (binder, factory entry points, joint, exports wiring) and the runtime registration API are implemented; the parity tests pass on both binder versions.

## How the binder is produced

Dovetail ports Microsoft's configuration binding source generator rather than reimplementing it: the parser, the type specs and the `CoreBindingHelpers` emitter from dotnet/runtime (MIT), at tags `v10.0.13` and `v11.0.0-rc.2.26504.105`. The port lives in `src/Dovetail.Analyzers/Support/Configuration/Binding/`.

It can't call Microsoft's generator: a generator never sees another generator's output, so calls in Dovetail's generated code are never intercepted (S5).

The port drops:

- call-site interception;
- `BinderOptions`, with `ValidateConfigurationKeys` and the key caches. Dovetail never passes binder options;
- `GetCore`, `GetValueCore` and `BindCoreMain`'s type switch. The entry points are per type.

Framework types are fully qualified, because the binder is emitted into the consuming project's namespace.

The rules follow the major version of the compilation's `Microsoft.Extensions.Configuration.Binder` reference (`BinderVersion`; version 11 when there's none). The differences between 10 and 11 are the ones in the two generators' source:

|                                                   | 10                                  | 11                                          |
| ------------------------------------------------- | ----------------------------------- | ------------------------------------------- |
| `Nullable<T>` set to `""`                         | value kept                          | null                                        |
| string/object ctor parameter missing              | throws                              | null                                        |
| value-type ctor parameter set to `""`             | default                             | throws (the parse fails)                    |
| nullable or reference-type ctor parameter missing | throws                              | null (structs still throw)                  |
| parameter with a default, set to null             | null                                | the default                                 |
| properties bound through the constructor          | bound again by `BindCore`           | not bound again (`boundThroughConstructor`) |
| null get-only complex member                      | a new instance is bound and dropped | skipped                                     |
| `[ConfigurationIgnore]`                           | not honoured                        | honoured, in the schema too                 |

Some version 10 output doesn't compile: unescaped keyword identifiers, `Name = Name` for a parameter named `name`, and `Items = Items` self-assignments (CS1717). Those use the version 11 form in both modes; none of them changes behaviour.

## Entry points and registration

Each `[DovetailConfiguration]` assembly gets `Dovetail_ConfigurationOptions.g.cs`. It holds:

- a private `DovetailConfigurationOptions : IServiceJoint`, nested in the assembly's exports class. The exports list it first, so configuration binds before other joints configure options in code. `Imports` includes the assembly's own exports when it has the joint;
- a file-local `DovetailConfigurationBinder` with one `Create…` entry point per root type.

Every root has one entry point, `Create…(configuration)`, and the joint registers every type through it. It is built two ways:

- **Types the options pipeline can create**, those with a public parameterless constructor: `var instance = new T(); Bind…(configuration, instance); return instance;`. The `Bind…` helper is private. It's `BindCoreMain`: a section with neither a value nor children leaves the instance alone; otherwise `BindCore(…, defaultValueIfNotFound: false)`, as `services.Configure<T>(section)` does.
- **The rest**, constructor parameters, init-only or `required` members. It's `GetCore`: `Initialize…`, then `BindCore(…, defaultValueIfNotFound: true)`. The difference is that it still creates an instance for an empty section, where `Get<T>()` returns null: `IOptions<T>.Value` can't be null. A missing required constructor parameter therefore throws when the options are first read.

There is one public overload because everything is created through generated create code, with no reflection anywhere. The joint calls `AddDovetailConfigurationOptions<T>(services, path, create)`, in path then type name order, and `ValidateOnStart` when asked. Every `[OptionsValidator]` class in the assembly that validates a registered type is added with `TryAddEnumerable`. The runtime API (`src/Dovetail/DovetailConfigurationOptionsExtensions.cs`):

- reads only its own section from the application's complete `IConfiguration`, resolved from the container when the options are first created, like `BindConfiguration`. Only the default options are registered; another options name is created from the same section;
- adds one `IOptionsFactory<T>` (`OptionsFactory<T>` subclass), with `TryAdd`: an existing factory wins, whether the application's or an earlier registration's, so only the first registration for a type counts, and a factory the application registers itself wins over ours. Configure, post-configure and validators still run on the created instance, in the base class, for the default options. The factory has no fallback to the base class's own creation, so the `DynamicallyAccessedMembers` annotation stays only because `OptionsFactory<T>` declares it (the trim analyzer reports IL2091 without it);
- watches reloads with Microsoft's `ConfigurationChangeTokenSource<T>` for the default name, over the application's `IConfiguration`, so `IOptionsMonitor<T>` (including `OnChange`) and `IOptionsSnapshot<T>` see them. Named-options support (a per-name configuration provider and reload tracking) was removed to keep the scope to the default options.

There is no `Order` and no stacking, de-duplication or merging: a type has one section (DT0015 rejects a class attribute plus an assembly `DovetailConfiguration<T>` for one type), each library consumes its own section and the pipeline does the rest.

## Where Dovetail differs from Microsoft's generator, on purpose

1. **Object-initializer types.** Microsoft's generator never sets init-only members of a type with a parameterless constructor, and can't create one with `required` members at all (CS9035). Dovetail creates those types through an `Initialize…` method with an object initializer (`ObjectSpec.InitializerOnly`), then binds the rest; `BindCore` doesn't bind them again, in either version. A missing key keeps the member's own initial value, read from a throwaway instance, as the reflection binder does. Required members have no initial value and get `default`.
2. **Required members that can't be bound** (an unsupported type, or `[ConfigurationIgnore]`) are set to `default!` wherever Dovetail creates the type, so the generated code compiles. DT0009 already reports the member.
3. **Empty sections** still produce an instance for factory types (see above).
4. **No `NotSupportedException`s** and no `BindNonPublicProperties` (S5).
5. **Self-expanding generics** (`class Node<T> { Node<Node<T>> Next }`) would make the analysis run forever, Microsoft's included. Past a generic nesting depth of 10, or 1,000 types, a type is treated as unsupported and isn't bound; the schema reports DT0012 for the same graphs.

The schema follows the binder's view of a type: it leaves out `[ConfigurationIgnore]` members from version 11, and lists static properties the binder sets and the declared properties of interfaces and abstract classes (the binder fills an existing instance of those). A root the binder can't create reports DT0011.

The binder reads the keys of the section's `KeyStyle` (as declared by default, or Pascal, camel, dash or underscore case, with `[ConfigurationKeyName]` winning); because the binder is generated once per type, a type whose effective keys differ between two sections is DT0016.

Microsoft's own quirks are kept. For example:

- a positional record's extra init-only property is set to `default` when its key is missing, overriding its initializer;
- version 10 binds constructor-bound collections twice.

These belong in the managed-configuration docs (P5).

## Verification

- **Analyzer tests** (`ConfigurationOptionsTests`): snapshots of the generated file for a mutable options class and for records, init-only and constructor types, under both binder versions; the generated code compiles; joint registration order (path, then type name), validators, `ValidateOnStart` and opting out; `[ConfigurationIgnore]` by version; and caching.
- **Parity tests** (`test/Dovetail.Configuration.Parity.Tests`, 266 per target framework): the same types and data bound three ways:
    - Microsoft's generator (`EnableConfigurationBindingGenerator`, `Configure<T>` → `IOptions<T>` for mutable types, `Get<T>()` for factory types);
    - the reflection binder (through a generic helper the generator can't intercept);
    - Dovetail's joint → `IOptions<T>`.

    They run on net10.0 (Binder 10) and net11.0 (Binder 11), from the S5 case matrix plus reload, collections of records and init-only types. Microsoft's generator is the oracle except for the differences above, which are asserted against the reflection binder or the documented behaviour.

- **Runtime tests** (`test/Dovetail.Configuration.Runtime.Tests`): registration (two registrations of one type from different assemblies: the first factory wins; an application-registered factory wins), factories, validators, `ValidateOnStart`, reload, with hand-written delegates.
- **AOT** (`test/Dovetail.Configuration.Aot.Tests`): the real generated joint and binder for a class, a record and an init-only type compile with IL2026, IL3050 and IL2091 as errors, and bind.
