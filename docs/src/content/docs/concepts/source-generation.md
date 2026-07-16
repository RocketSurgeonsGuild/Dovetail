---
title: Source Generation
description: How the Roslyn source generator resolves and exports joints at build time.
tags: [source-generator, aot]
---

# Source Generation and Dovetails [New]

We have built a Source Generator to use with Dovetail that allows us to export joints and import joints with easy, in a way that is statically compiled.

# Exporting Dovetails

Any joint marked with `[assembly: Dovetail(typeof(MyDovetail))]` will automatically be added to a public partial static class named `Exports` that will exist in the root namespace of the assembly being built.

> [!TIP]
> This class is partial if you want to add any custom methods to it for consumers.

# Importing Dovetails

You can import all the joints for a given assembly by adding the `[assembly: ImportCoventions]` attribute which will create a `Imports` class inside the assembly's root namespace.

> [!TIP]
> This class is partial if you want to add any custom methods to it for consumers.

You can also import the `GetDovetails` method onto a given class by using the `[ImportDovetails]` attribute on the class.
