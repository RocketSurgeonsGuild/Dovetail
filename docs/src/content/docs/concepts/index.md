---
title: Concepts
description: Core concepts behind Dovetail — how joint-driven wiring works in .NET.
---

import { CardGrid, LinkCard } from '@astrojs/starlight/components';

Dovetail replaces runtime reflection-based application wiring with compile-time source generation. These pages explain the core ideas behind the library.

<CardGrid>
  <LinkCard title="Introduction" href="/concepts/introduction/" description="What Dovetail is and why it exists." />
  <LinkCard title="Defining Dovetails" href="/concepts/defining-joints/" description="How to create and register your own joints." />
  <LinkCard title="Dovetail Context" href="/concepts/joint-context/" description="The context object passed to every joint." />
  <LinkCard title="Source Generation" href="/concepts/source-generation/" description="How the Roslyn generator resolves joints at build time." />
  <LinkCard title="Unit Tests" href="/concepts/unit-tests/" description="Testing joints in isolation." />
  <LinkCard title="Configuration with Optionary" href="/concepts/configuration-with-optionary/" description="Typed options and a merged JSON Schema from Optionary, registered by a Dovetail joint." />
  <LinkCard title="Configuration Files" href="/concepts/configuration-files/" description="Ship a library's appsettings files in JSON, YAML or TOML to every application that references it." />
  <LinkCard title="ZeroAlloc" href="/concepts/zeroalloc/" description="ZeroAlloc.Mediator and ZeroAlloc.Validation register themselves through joints written into the project." />
</CardGrid>
