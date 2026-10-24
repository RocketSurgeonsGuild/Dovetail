### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|-------
DT0006 | Dovetail | Warning | DT0006: the project's C# version is older than 12, so the generator emits nothing instead of code that wouldn't compile
DT0007 | Dovetail | Error | DT0007: a [DovetailConfiguration] type is abstract, static, an interface or an open generic type
DT0008 | Dovetail | Error | DT0008: a [DovetailConfiguration] section path is empty, has empty segments, or can't be derived from the type name
DT0009 | Dovetail | Warning | DT0009: a configuration member has a type configuration binding doesn't support, so it's left out of the schema
DT0010 | Dovetail | Warning | DT0010: two members of a configuration type bind the same case-insensitive key
DT0011 | Dovetail | Error | DT0011: a configuration type can't be created from configuration (ambiguous constructors, or a parameter without a matching property)
DT0012 | Dovetail | Warning | DT0012: a configuration type contains itself, so the schema stops describing it at the repetition
DT0013 | Dovetail | Warning | DT0013: an annotation (e.g. a regular expression or a non-numeric range) can't be expressed in JSON Schema
