# Rocket Surgeons - Conventions

# Status

<!-- badges -->

[![github-release-badge]][github-release]
[![github-license-badge]][github-license]
[![codecov-badge]][codecov]

<!-- badges -->

<!-- history badges -->

| GitHub Actions                    |
| --------------------------------- |
| [![github-badge]][github]         |
| [![github-history-badge]][github] |

<!-- history badges -->

<!-- nuget packages -->

| Package | NuGet |
| ------- | ----- |

<!-- nuget packages -->

# Contributing

The .NET SDK and the extra .NET 10 runtime come from `mise` (`.config/mise.toml`), not `global.json`. Run `mise install` before building.

> [!NOTE]
> On macOS, the first `mise install` of a new .NET SDK can overwrite the shared `~/.local/share/mise/dotnet-root/dotnet` host in place, and macOS then kills every `dotnet` command (SIGKILL). Replace the host with a fresh copy to fix it:
>
> ```bash
> cd ~/.local/share/mise/dotnet-root && cp -p dotnet dotnet.fresh && mv -f dotnet.fresh dotnet
> ```

# Whats next?

TBD

<!-- generated references -->

[github-release]: https://github.com/RocketSurgeonsGuild/Conventions/releases/latest
[github-release-badge]: https://img.shields.io/github/release/RocketSurgeonsGuild/Conventions.svg?logo=github&style=flat 'Latest Release'
[github-license]: https://github.com/RocketSurgeonsGuild/Conventions/blob/master/LICENSE
[github-license-badge]: https://img.shields.io/github/license/RocketSurgeonsGuild/Conventions.svg?style=flat 'License'
[codecov]: https://codecov.io/gh/RocketSurgeonsGuild/Conventions
[codecov-badge]: https://img.shields.io/codecov/c/github/RocketSurgeonsGuild/Conventions.svg?color=E03997&label=codecov&logo=codecov&logoColor=E03997&style=flat 'Code Coverage'
[github]: https://github.com/RocketSurgeonsGuild/Conventions/actions?query=workflow%3Aci
[github-badge]: https://img.shields.io/github/workflow/status/RocketSurgeonsGuild/Conventions/ci.svg?label=github&logo=github&color=b845fc&logoColor=b845fc&style=flat 'GitHub Actions Status'
[github-history-badge]: https://buildstats.info/github/chart/RocketSurgeonsGuild/Conventions?includeBuildsFromPullRequest=false 'GitHub Actions History'

<!-- generated references -->

<!-- nuke-data
github:
  owner: RocketSurgeonsGuild
  repository: Conventions
-->
