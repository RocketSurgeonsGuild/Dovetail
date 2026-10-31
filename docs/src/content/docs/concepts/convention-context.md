---
title: Dovetail Context
description: The context object passed to every joint — how to create, configure, and query it.
---

# Dovetail Contexts

The joint context is the the result of collecting all the known joints for a given set of assembly. How those assemblies are collected can diff
depending on on the joint context is created. Generally the joint context is created using the <xref:Joinery.Dovetail.DovetailContextBuilder>
however you can implement your own <xref:Joinery.Dovetail.IDovetailContext> if you wish.

## Builder

A joint context is created from a <xref:Joinery.Dovetail.DovetailContextBuilder>. This class is used during the bootstrapping phase of your application.

You can add joints manually, you can add them via attribute scanning, you can disable attribute scanning if you wish as well.

> [!NOTE]
> The assemblies used during scanning can be added by using an AppDomain, DependencyContext, or List of assemblies.

## Creating the context

A context can be created from a <xref:Joinery.Dovetail.DovetailContextBuilder> by using [DovetailContext.From](xref:Joinery.Dovetail.DovetailContext#Joinery_Dovetail_DovetailContext_From_Dovetail_DovetailContextBuilder_).

## Categories

Every joint has a category. `DovetailCategory.Core` is for infrastructure joints (serializers, logging), and
`DovetailCategory.Application` is the default for everything else.

`DovetailContextBuilder.Create(joints, properties, categories)` takes a set of categories as well, and **the context loads the
joints whose category is in that set, plus the `Core` joints, which are always loaded**. An empty set loads the `Application`
joints and the `Core` joints, which is what the generated `Imports.Joints()` and `DovetailContextBuilder.Create()` pass when you
give them no categories. A joint in any other category is loaded only when you name its category.

| `categories`                                            | Joints in the context                                                             |
| ------------------------------------------------------- | --------------------------------------------------------------------------------- |
| `[]`                                                    | The `Application` and the `Core` joints                                           |
| `[DovetailCategory.Core]`                               | Only the `Core` joints                                                            |
| `[DovetailCategory.Application]`                        | The `Application` and the `Core` joints                                           |
| `[DovetailCategory.Core, DovetailCategory.Application]` | The `Application` and the `Core` joints                                           |
| `["Reporting"]`                                         | The joints in `Reporting` and the `Core` joints, but not the `Application` joints |

The set is available as `Categories` on the builder and on the context. It is the set that is loaded: the set you passed in with
`Core` added, or `Application` and `Core` when you passed none.

## Using the context

Once the context is created you can use the context to find out all sorts of information.

Useful properties:

- [`AssemblyProvider`](xref:Joinery.Dovetail.IDovetailContext#Joinery_Dovetail_DovetailContext_AssemblyProvider) - The type provider can be used to get a list of assemblies
- [`Logger`](xref:Joinery.Dovetail.IDovetailContext#Joinery_Dovetail_DovetailContext_Logger) - This is a diagnostic logger that can be used for logging details. If a logger is provided to the builder it will be used here.
- [`Properties`](xref:Joinery.Dovetail.IDovetailContext#Joinery_Dovetail_DovetailContext_Properties) - Contains all the properties provided to the builder. This implements `IServiceProvider` and can be used with `ActivatorExtensions

Useful methods / extension methods:

- [`Get<T>([string name])`](xref:Joinery.Dovetail.DovetailContextExtensions#Joinery_Dovetail_DovetailContextExtensions_Get__1_Dovetail_IDovetailContext_) - Get a given type from the `Properties` dictionary.
- [`GetHostType()`](xref:Joinery.Dovetail.DovetailContextExtensions#Joinery_Dovetail_DovetailContextExtensions_GetHostType_Dovetail_IDovetailContext_) - Get's the given host type, as defined in the builder.
- [`IsUnitTestHost()`](xref:Joinery.Dovetail.DovetailContextExtensions#Joinery_Dovetail_DovetailContextExtensions_IsUnitTestHost_Dovetail_IDovetailContext_) - Tests if the builder was setup for unit testing
    - This is handy to ensure different behavior during [unit tests](./unit-tests.md).
