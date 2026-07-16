---
title: Dovetail Context
description: The context object passed to every joint — how to create, configure, and query it.
---

# Dovetail Contexts

The joint context is the the result of collecting all the known joints for a given set of assembly. How those assemblies are collected can diff
depending on on the joint context is created. Generally the joint context is created using the <xref:Dovetail.DovetailContextBuilder>
however you can implement your own <xref:Dovetail.IDovetailContext> if you wish.

## Builder

A joint context is created from a <xref:Dovetail.DovetailContextBuilder>. This class is used during the bootstrapping phase of your application.

You can add joints manually, you can add them via attribute scanning, you can disable attribute scanning if you wish as well.

> [!NOTE]
> The assemblies used during scanning can be added by using an AppDomain, DependencyContext, or List of assemblies.

## Creating the context

A context can be created from a <xref:Dovetail.DovetailContextBuilder> by using [DovetailContext.From](xref:Dovetail.DovetailContext#Dovetail_DovetailContext_From_Dovetail_DovetailContextBuilder_).

## Using the context

Once the context is created you can use the context to find out all sorts of information.

Useful properties:

- [`AssemblyProvider`](xref:Dovetail.IDovetailContext#Dovetail_DovetailContext_AssemblyProvider) - The type provider can be used to get a list of assemblies
- [`Logger`](xref:Dovetail.IDovetailContext#Dovetail_DovetailContext_Logger) - This is a diagnostic logger that can be used for logging details. If a logger is provided to the builder it will be used here.
- [`Properties`](xref:Dovetail.IDovetailContext#Dovetail_DovetailContext_Properties) - Contains all the properties provided to the builder. This implements `IServiceProvider` and can be used with `ActivatorExtensions

Useful methods / extension methods:

- [`Get<T>([string name])`](xref:Dovetail.DovetailContextExtensions#Dovetail_DovetailContextExtensions_Get__1_Dovetail_IDovetailContext_) - Get a given type from the `Properties` dictionary.
- [`GetHostType()`](xref:Dovetail.DovetailContextExtensions#Dovetail_DovetailContextExtensions_GetHostType_Dovetail_IDovetailContext_) - Get's the given host type, as defined in the builder.
- [`IsUnitTestHost()`](xref:Dovetail.DovetailContextExtensions#Dovetail_DovetailContextExtensions_IsUnitTestHost_Dovetail_IDovetailContext_) - Tests if the builder was setup for unit testing
    - This is handy to ensure different behavior during [unit tests](./unit-tests.md).
