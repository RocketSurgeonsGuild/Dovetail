---
title: ZeroAlloc
description: ZeroAlloc.Mediator and ZeroAlloc.Validation register themselves through joints Dovetail writes into every project that runs their source generators. Trimming and Native AOT safe.
tags: [mediator, validation, aot]
---

# ZeroAlloc

[ZeroAlloc.Mediator](https://mediator.zeroalloc.net/) and [ZeroAlloc.Validation](https://validation.zeroalloc.net/) generate
their dependency injection registration into the project that uses them: `AddMediator()`, `AddZeroAllocValidators()` and
`AddZeroAllocAspNetCoreValidation()` are written into each assembly by the libraries' source generators, for that assembly's
handlers and validators. A package can't call a method that only exists in the project, so Joinery.Dovetail writes a service
[joint](./defining-joints) into the project instead, and that joint calls it.

## Turning it on

Reference the ZeroAlloc packages as their documentation says. There is nothing to call and no Dovetail package to add:
`Joinery.Dovetail` sees which ZeroAlloc generators the project runs and writes the matching joints when it builds.

| The project runs                                                                       | Joint                                        | It calls                                      |
| -------------------------------------------------------------------------------------- | -------------------------------------------- | --------------------------------------------- |
| `ZeroAlloc.Mediator.Generator`                                                         | `DovetailZeroAllocMediatorJoint`             | `services.AddMediator()` and the bridges      |
| `ZeroAlloc.Validation.Inject`                                                          | `DovetailZeroAllocValidatorsJoint`           | `services.AddZeroAllocValidators()`           |
| `ZeroAlloc.Validation.AspNetCore`'s generator, with a direct reference to that package | `DovetailZeroAllocAspNetCoreValidationJoint` | `services.AddZeroAllocAspNetCoreValidation()` |

The generator is what turns a joint on, not the runtime package: `ZeroAlloc.Mediator` on its own has no `AddMediator()`.

Every project that runs a generator gets its joint, libraries included. ZeroAlloc registers handlers and validators one
assembly at a time, so a library's joint registers the library's own, and it is exported to the applications that reference
the library like any other joint. The application only calls `ConfigureDovetail()`.

### Mediator bridges

The mediator joint turns on each ZeroAlloc.Mediator bridge package the project references, by calling its `With...()` method
on the builder `AddMediator()` returns:

| Package                         | Call               |
| ------------------------------- | ------------------ |
| `ZeroAlloc.Mediator.Validation` | `WithValidation()` |
| `ZeroAlloc.Mediator.Cache`      | `WithCache()`      |
| `ZeroAlloc.Mediator.Resilience` | `WithResilience()` |
| `ZeroAlloc.Mediator.Telemetry`  | `WithTelemetry()`  |

`ZeroAlloc.Mediator.Authorization` is left out: `WithAuthorization(...)` takes the application's policies, so call it from a
joint of your own.

With `ZeroAlloc.Mediator.Validation` and `ZeroAlloc.Validation.Inject` in the project that has the handlers, a request is
validated before its handler runs, with nothing registered by hand.

### ASP.NET Core

`AddZeroAllocAspNetCoreValidation()` registers the validators too and adds an MVC action filter. Each call adds the filter
again, and the package's generator also runs in the projects that only get it through another project, so only a project that
references `ZeroAlloc.Validation.AspNetCore` itself gets that joint.

## Turning it off

Set `DovetailEnableZeroAllocMediator` or `DovetailEnableZeroAllocValidation` to `false` to leave that registration to the
project. Neither turns the ZeroAlloc generators off.
