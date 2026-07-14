---
title: API Reference
description: Auto-generated API reference for all public Dovetail packages.
---

import { CardGrid, LinkCard } from '@astrojs/starlight/components';

API reference documentation is auto-generated from XML doc comments in the compiled assemblies, grouped by
namespace. Build the solution (so each project's `bin/` output includes its XML docs) before running the docs
site — see `mise run docs:build`.

<CardGrid>
	<LinkCard title="Core" href="/api/dovetail/" description="Core convention contracts, context, and runtime wiring — IDovetailJointt, IDovetailContext, DovetailContextBuilder." />
	<LinkCard title="Hosting" href="/api/dovetail/hosting/" description="Generic host and WebAssembly hosting integration APIs." />
	<LinkCard title="Configuration" href="/api/dovetail/configuration/" description="JSON, YAML, and TOML configuration convention APIs." />
	<LinkCard title="Aspire" href="/api/dovetail/aspire/" description=".NET Aspire hosting conventions, including testing integration." />
	<LinkCard title="Autofac" href="/api/dovetail/autofac/" description="Autofac container integration APIs." />
	<LinkCard title="DryIoc" href="/api/dovetail/dryioc/" description="DryIoc container integration APIs." />
	<LinkCard title="Serilog" href="/api/dovetail/serilog/" description="Serilog logging convention APIs." />
</CardGrid>
