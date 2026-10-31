### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|-------
DT0006 | Dovetail | Warning | DT0006: the project's C# version is older than 12, so the generator emits nothing instead of code that wouldn't compile
DT0007 | Dovetail | Error | DT0007: a [DovetailConfiguration] type is abstract, static, an interface, a collection or an open generic type
DT0008 | Dovetail | Error | DT0008: a [DovetailConfiguration] section path is empty, has empty segments, or can't be derived from the type name
DT0009 | Dovetail | Warning | DT0009: a configuration member has a type configuration binding doesn't support, so it's left out of the schema
DT0010 | Dovetail | Warning | DT0010: two members of a configuration type bind the same case-insensitive key
DT0011 | Dovetail | Error | DT0011: a configuration type can't be created from configuration (ambiguous constructors, or a parameter without a matching property)
DT0012 | Dovetail | Warning | DT0012: a configuration type's graph has more distinct types than the schema describes (e.g. a generic type that nests itself ever deeper)
DT0013 | Dovetail | Warning | DT0013: an annotation (e.g. a regular expression or a non-numeric range) can't be expressed in JSON Schema
DT0014 | Dovetail | Warning | DT0014: the generator failed while describing a configuration type; the type gets no schema, the rest of the build is unaffected
DT0015 | Dovetail | Error | DT0015: an options type is configured by both a [DovetailConfiguration] class attribute and an [assembly: DovetailConfiguration<T>]; the class declaration is used
DT0016 | Dovetail | Error | DT0016: a type is reached from [DovetailConfiguration] sections of different KeyStyles, so the generated binder can't read the right keys for both; the later declaration is ignored
DT0017 | Dovetail | Error | DT0017: a joint has constructors but none that is public or internal, so the generated code can't create it; the joint is left out of the exports
