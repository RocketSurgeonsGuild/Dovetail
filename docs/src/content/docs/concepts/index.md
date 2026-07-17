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
  <LinkCard title="Managed Configuration" href="/concepts/managed-configuration/" description="Author library configuration once and have it packaged, typed, and wired into any host application." />
</CardGrid>
