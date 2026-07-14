---
title: Concepts
description: Core concepts behind Dovetail — how convention-driven wiring works in .NET.
---

import { CardGrid, LinkCard } from '@astrojs/starlight/components';

Dovetail replaces runtime reflection-based application wiring with compile-time source generation. These pages explain the core ideas behind the library.

<CardGrid>
  <LinkCard title="Introduction" href="/concepts/introduction/" description="What Dovetail is and why it exists." />
  <LinkCard title="Defining Dovetails" href="/concepts/defining-conventions/" description="How to create and register your own conventions." />
  <LinkCard title="Dovetail Context" href="/concepts/convention-context/" description="The context object passed to every convention." />
  <LinkCard title="Source Generation" href="/concepts/source-generation/" description="How the Roslyn generator resolves conventions at build time." />
  <LinkCard title="Unit Tests" href="/concepts/unit-tests/" description="Testing conventions in isolation." />
  <LinkCard title="Managed Configuration" href="/concepts/managed-configuration/" description="Author library configuration once and have it packaged, typed, and wired into any host application." />
</CardGrid>
