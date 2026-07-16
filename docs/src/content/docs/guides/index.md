---
title: Getting Started
description: Get up and running with Dovetail.
tags: [getting-started]
---

import { CardGrid, LinkCard } from '@astrojs/starlight/components';

Dovetail is a joint-driven application wiring library for .NET. It uses a Roslyn source generator to resolve and order joints at build time — eliminating runtime reflection and making your app AOT and trimming safe.

> [!NOTE]
> Dovetail requires .NET 8 or .NET 10 and a compatible Roslyn-based build.

<CardGrid>
  <LinkCard title="Custom Dovetails" href="/guides/custom-joints/" description="Learn how to create and distribute your own joints." />
</CardGrid>
