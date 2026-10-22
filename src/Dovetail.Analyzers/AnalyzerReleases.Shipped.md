; Release tracking: https://github.com/dotnet/roslyn/blob/main/src/RoslynAnalyzers/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md

## Release 0.1.0

### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|-------
DT0001 | Dovetail | Error | DT0001: a joint declares more than one constructor; joints may only have one (or the default) constructor
DT0002 | Dovetail | Error | DT0002: a joint is a generic type, which joints cannot be
DT0003 | Dovetail | Error | DT0003: a joint dependency attribute was given something other than a typeof expression
DT0004 | Dovetail | Warning | DT0004: the generator could not interpret a joint dependency attribute
DT0005 | Dovetail | Error | DT0005: a joint is part of a circular dependency chain, so the joints can't be ordered
