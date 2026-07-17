---
title: Custom Dovetails
description: Step-by-step guide to creating and distributing your own Dovetails.
---

# Creating a custom joint

The essence of a joint is a method, with any number of parameters, that can be implemented to do some work.

Lets say you want to create a joint for handling database configuration.

Goal: Distribute database configuration across assemblies

Given this fake interface for configuring the database.

[!code-c#[IDatabaseConfigurator](../../sample/Sample.Core/Databases/IDatabaseConfigurator.cs?name=codeblock)]

## Define your interface and optional delegate

Now we will define our interface and delegate. The delegate and extenison methods are optional but makes it easier for consumers to create adhoc joints.

[!code-c#[IDatabaseConfigurator](../../sample/Sample.Core/Databases/DatabaseDovetail.cs?name=codeblock)]
[!code-c#[IDatabaseConfigurator](../../sample/Sample.Core/Databases/IDatabaseDovetail.cs?name=codeblock)]
[!code-c#[IDatabaseConfigurator](../../sample/Sample.Core/Databases/DatabaseDovetailContextBuilderExtensions.cs?name=codeblock)]

## Create your application method

We have defined our joints now we need to be able to apply our joints.

There are two ways to do this, if for example your database configuration happens during service registration (perhaps via `IOptions`) then you can implement this
a joint that will run during the services joint. The other way additional way is to create an extension method that takes takes the joint and applies it.

In this example we'll support both. The manual way using an extension method and a joint that will do it "automagically".

[!code-c#[IDatabaseConfigurator](../../sample/Sample.Core/Databases/DatabaseConfiguratorExtensions.cs?name=codeblock)]
[!code-c#[IDatabaseConfigurator](../../sample/Sample.Core/Databases/DatabaseServiceJoint.cs?name=codeblock)]
