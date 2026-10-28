# Dovetail

- Package: `Dovetail`
- Version: `0.2.2`
- Target framework: `net10.0`
- NuGet: [Dovetail](https://www.nuget.org/packages/Dovetail)

## Namespaces

### Dovetail

- [DovetailCategory](/Dovetail/v0.2.2/api/dovetail/dovetailcategory.md) -- `class` `sealed` -- The category of a given convention
- [DovetailConfigurationContextExtensions](/Dovetail/v0.2.2/api/dovetail/dovetailconfigurationcontextextensions.md) -- `class` `static` -- Extension members for IDovetailContext configuration access.
- [DovetailConfigurationOptionsExtensions](/Dovetail/v0.2.2/api/dovetail/dovetailconfigurationoptionsextensions.md) -- `class` `static` -- Registers strongly-typed configuration classes with the standard `Extensions.Options` pipeline.
- [DovetailContext](/Dovetail/v0.2.2/api/dovetail/dovetailcontext.md) -- `class` `sealed` -- Base convention context that allows for stashing items in index keys Implements the [IDovetailContext](/Dovetail/v0.2.2/api/dovetail/idovetailcontext.md)
- [DovetailContextBuilder](/Dovetail/v0.2.2/api/dovetail/dovetailcontextbuilder.md) -- `class` `sealed` -- Builder that can be used to create a context.
- [DovetailContextExtensions](/Dovetail/v0.2.2/api/dovetail/dovetailcontextextensions.md) -- `class` `static` -- Base convention extensions
- [DovetailEmbeddedConfiguration](/Dovetail/v0.2.2/api/dovetail/dovetailembeddedconfiguration.md) -- `class` `static` -- Loads configuration files that the Dovetail SDK embedded into an assembly as manifest resources, for hosts that have no file system to read them from (mobile and other app-package hosts).
- [DovetailExceptionPolicy](/Dovetail/v0.2.2/api/dovetail/dovetailexceptionpolicy.md) -- `class` `static` -- A policy that determines how to handle exceptions thrown by conventions
- [DovetailExceptionPolicyDelegate](/Dovetail/v0.2.2/api/dovetail/dovetailexceptionpolicydelegate.md) -- `delegate` -- A policy delegate that can be used to determine if a given exception should be ignored or not
- [DovetailHostBuilderExtensions](/Dovetail/v0.2.2/api/dovetail/dovetailhostbuilderextensions.md) -- `class` `static` -- Base convention extensions
- [DovetailHostType](/Dovetail/v0.2.2/api/dovetail/dovetailhosttype.md) -- `enum` -- The underlying host type (currently only checked for test host) This is so that conventions can avoid adding behaviors that might cause issues with unit testing such as writing to the console or debug pipes, or ensuring that something is configured in a correct way for testing. Careful consideration is needed to make sure that your system doesn't behave extremely differently in a live scenario vs a testing scenario
- [IDovetailContext](/Dovetail/v0.2.2/api/dovetail/idovetailcontext.md) -- `interface` -- The base context marker interface to define this as a context
- [ImportHelpers](/Dovetail/v0.2.2/api/dovetail/importhelpers.md) -- `class` `static` -- This class is used by the source generator to allow the conventions provided to be overridden by the caller.

### Dovetail.Attributes

- [AfterJointAttribute](/Dovetail/v0.2.2/api/dovetail/afterjointattribute.md) -- `class` `sealed` -- An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)
- [AfterJointAttribute<T>](/Dovetail/v0.2.2/api/dovetail/afterjointattribute-1.md) -- `class` `sealed` -- An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)
- [BeforeJointAttribute](/Dovetail/v0.2.2/api/dovetail/beforejointattribute.md) -- `class` `sealed` -- An attribute that ensures the convention runs before the given [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)
- [BeforeJointAttribute<T>](/Dovetail/v0.2.2/api/dovetail/beforejointattribute-1.md) -- `class` `sealed` -- An attribute that ensures the convention runs before the given [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)
- [ConfigurationKeyStyle](/Dovetail/v0.2.2/api/dovetail/configurationkeystyle.md) -- `enum` -- How [DovetailConfigurationAttribute](/Dovetail/v0.2.2/api/dovetail/dovetailconfigurationattribute.md) spells the configuration keys of an options type and of every object type nested in it.
- [DependentOfJointAttribute](/Dovetail/v0.2.2/api/dovetail/dependentofjointattribute.md) -- `class` `sealed` -- An attribute that ensures the convention runs before the given [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)
- [DependentOfJointAttribute<T>](/Dovetail/v0.2.2/api/dovetail/dependentofjointattribute-1.md) -- `class` `sealed` -- An attribute that ensures the convention runs before the given [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)
- [DependsOnJointAttribute](/Dovetail/v0.2.2/api/dovetail/dependsonjointattribute.md) -- `class` `sealed` -- An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)
- [DependsOnJointAttribute<T>](/Dovetail/v0.2.2/api/dovetail/dependsonjointattribute-1.md) -- `class` `sealed` -- An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)
- [DovetailCategoryAttribute](/Dovetail/v0.2.2/api/dovetail/dovetailcategoryattribute.md) -- `class` `sealed` -- Defines the category of a given convention
- [DovetailConfigurationAttribute](/Dovetail/v0.2.2/api/dovetail/dovetailconfigurationattribute.md) -- `class` `sealed` -- Declares the configuration section of an options class. Dovetail's source generator describes the class as JSON Schema, which is merged into the application's `appsettings.schema.json` for editor completion and validation, and registers it with the options pipeline.
- [DovetailConfigurationAttribute<T>](/Dovetail/v0.2.2/api/dovetail/dovetailconfigurationattribute-1.md) -- `class` `sealed` -- Declares the configuration section of an options type `T` from the assembly, for types you don't own.
- [DovetailExportAttribute](/Dovetail/v0.2.2/api/dovetail/dovetailexportattribute.md) -- `class` `sealed` -- Defines this convention as one that is exported to the Dovetail source generator
- [DovetailHostTypeAttribute](/Dovetail/v0.2.2/api/dovetail/dovetailhosttypeattribute.md) -- `class` `sealed` -- Defines the host type of a given convention
- [LiveJointAttribute](/Dovetail/v0.2.2/api/dovetail/livejointattribute.md) -- `class` `sealed` -- Defines this convention as one that only runs during live usage to avoid unit tests
- [UnitTestJointAttribute](/Dovetail/v0.2.2/api/dovetail/unittestjointattribute.md) -- `class` `sealed` -- Defines this convention as one that only runs during a unit test run

### Dovetail.Diagnostics

- [DiagnosticExtensions](/Dovetail/v0.2.2/api/dovetail/diagnosticextensions.md) -- `class` `static` -- Provides extension methods for retrieving metadata from Dovetail context builders and contexts.
- [DovetailDiagnosticMetadataJoint](/Dovetail/v0.2.2/api/dovetail/dovetaildiagnosticmetadatajoint.md) -- `record` -- Represents a metadata item for a Dovetail joint, including its name, host type, category, and dependencies.
- [DovetailDiagnosticMetadataJointDependency](/Dovetail/v0.2.2/api/dovetail/dovetaildiagnosticmetadatajointdependency.md) -- `record` `sealed` -- Represents a metadata dependency for a Dovetail joint, including its name, host type, category, dependencies, and direction.
- [DovetailMetadataDiagnosticExtensions](/Dovetail/v0.2.2/api/dovetail/dovetailmetadatadiagnosticextensions.md) -- `class` `static` -- Extension methods for [IDovetailJointMetadata](/Dovetail/v0.2.2/api/dovetail/idovetailjointmetadata.md) to provide additional diagnostic information.

### Dovetail.Infrastructure

- [ConnectsJointAttribute<T>](/Dovetail/v0.2.2/api/dovetail/connectsjointattribute-1.md) -- `class` `sealed` -- A marker interface to indicate a type is a convention and has a name
- [DependencyDirection](/Dovetail/v0.2.2/api/dovetail/dependencydirection.md) -- `enum` -- Defines the dependency direction of the given type
- [DovetailContextBuilderExtensions](/Dovetail/v0.2.2/api/dovetail/dovetailcontextbuilderextensions.md) -- `class` `static` -- Dovetail Context build extensions.
- [DovetailContextBuilderFactory](/Dovetail/v0.2.2/api/dovetail/dovetailcontextbuilderfactory.md) -- `delegate` -- A delegate that can be used to create a [DovetailContextBuilder](/Dovetail/v0.2.2/api/dovetail/dovetailcontextbuilder.md).
- [DovetailExecutor](/Dovetail/v0.2.2/api/dovetail/dovetailexecutor.md) -- `class` -- A class to help with executing conventions
- [DovetailJointMetadata](/Dovetail/v0.2.2/api/dovetail/dovetailjointmetadata.md) -- `class` `sealed` -- Container for conventions
- [IDovetailDependency](/Dovetail/v0.2.2/api/dovetail/idovetaildependency.md) -- `interface` -- A dependency for a given convention
- [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md) -- `interface` -- A marker interface to indicate a type is a joint
- [IDovetailJointDelegate](/Dovetail/v0.2.2/api/dovetail/idovetailjointdelegate.md) -- `interface` -- A marker interface to indicate a type is a joint delegate
- [IDovetailJointMetadata](/Dovetail/v0.2.2/api/dovetail/idovetailjointmetadata.md) -- `interface` -- Declares a convention result with it's dependencies pre-computed
- [JointNameAttribute](/Dovetail/v0.2.2/api/dovetail/jointnameattribute.md) -- `class` `sealed` -- A marker interface to indicate a type is a convention and has a name
- [LoggingConnector](/Dovetail/v0.2.2/api/dovetail/loggingconnector.md) -- `class` `sealed` -- A joint that registers logging services into the context. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.
- [ServiceProviderDictionaryExtensions](/Dovetail/v0.2.2/api/dovetail/serviceproviderdictionaryextensions.md) -- `class` `static` -- Base convention extensions
- [ServiceProviderFactoryAdapter](/Dovetail/v0.2.2/api/dovetail/serviceproviderfactoryadapter.md) -- `delegate` -- A factory that provides a service provider factory from the convention context

### Dovetail.Joints

- [ConfigurationAsyncJointDelegate](/Dovetail/v0.2.2/api/dovetail/configurationasyncjointdelegate.md) -- `delegate` -- Async delegate for the Configuration part.
- [ConfigurationHostBuilderExtensions](/Dovetail/v0.2.2/api/dovetail/configurationhostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying Configuration parts to a [DovetailContextBuilder](/Dovetail/v0.2.2/api/dovetail/dovetailcontextbuilder.md).
- [ConfigurationJointDelegate](/Dovetail/v0.2.2/api/dovetail/configurationjointdelegate.md) -- `delegate` -- Delegate for the Configuration part.
- [ConfigurationJointExtensions](/Dovetail/v0.2.2/api/dovetail/configurationjointextensions.md) -- `class` `static` -- Extension methods for applying Configuration parts.
- [HostCreatedAsyncJointDelegate<THost>](/Dovetail/v0.2.2/api/dovetail/hostcreatedasyncjointdelegate-1.md) -- `delegate` -- Async delegate for the HostCreated part.
- [HostCreatedHostBuilderExtensions](/Dovetail/v0.2.2/api/dovetail/hostcreatedhostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying HostCreated parts to a [DovetailContextBuilder](/Dovetail/v0.2.2/api/dovetail/dovetailcontextbuilder.md).
- [HostCreatedJointDelegate<THost>](/Dovetail/v0.2.2/api/dovetail/hostcreatedjointdelegate-1.md) -- `delegate` -- Delegate for the HostCreated part.
- [HostCreatedJointExtensions](/Dovetail/v0.2.2/api/dovetail/hostcreatedjointextensions.md) -- `class` `static` -- Extension methods for applying HostCreated parts.
- [IConfigurationAsyncJoint](/Dovetail/v0.2.2/api/dovetail/iconfigurationasyncjoint.md) -- `interface` -- Async part interface for Configuration. Implements the [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)
- [IConfigurationJoint](/Dovetail/v0.2.2/api/dovetail/iconfigurationjoint.md) -- `interface` -- Joint interface for Configuration. Implements the [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)
- [IHostCreatedAsyncJoint<THost>](/Dovetail/v0.2.2/api/dovetail/ihostcreatedasyncjoint-1.md) -- `interface` -- Async part interface for HostCreated. Implements the [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)
- [IHostCreatedJoint<THost>](/Dovetail/v0.2.2/api/dovetail/ihostcreatedjoint-1.md) -- `interface` -- Joint interface for HostCreated. Implements the [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)
- [ILoggingAsyncJoint](/Dovetail/v0.2.2/api/dovetail/iloggingasyncjoint.md) -- `interface` -- Async part interface for Logging. Implements the [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)
- [ILoggingJoint](/Dovetail/v0.2.2/api/dovetail/iloggingjoint.md) -- `interface` -- Joint interface for Logging. Implements the [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)
- [IServiceAsyncJoint](/Dovetail/v0.2.2/api/dovetail/iserviceasyncjoint.md) -- `interface` -- Async part interface for Service. Implements the [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)
- [IServiceJoint](/Dovetail/v0.2.2/api/dovetail/iservicejoint.md) -- `interface` -- Joint interface for Service. Implements the [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)
- [ISetupAsyncJoint](/Dovetail/v0.2.2/api/dovetail/isetupasyncjoint.md) -- `interface` -- Async part interface for Setup. Implements the [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)
- [ISetupJoint](/Dovetail/v0.2.2/api/dovetail/isetupjoint.md) -- `interface` -- Joint interface for Setup. Implements the [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)
- [LoggingAsyncJointDelegate](/Dovetail/v0.2.2/api/dovetail/loggingasyncjointdelegate.md) -- `delegate` -- Async delegate for the Logging part.
- [LoggingHostBuilderExtensions](/Dovetail/v0.2.2/api/dovetail/logginghostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying Logging parts to a [DovetailContextBuilder](/Dovetail/v0.2.2/api/dovetail/dovetailcontextbuilder.md).
- [LoggingJointDelegate](/Dovetail/v0.2.2/api/dovetail/loggingjointdelegate.md) -- `delegate` -- Delegate for the Logging part.
- [LoggingJointExtensions](/Dovetail/v0.2.2/api/dovetail/loggingjointextensions.md) -- `class` `static` -- Extension methods for applying Logging parts.
- [ServiceAsyncJointDelegate](/Dovetail/v0.2.2/api/dovetail/serviceasyncjointdelegate.md) -- `delegate` -- Async delegate for the Service part.
- [ServiceHostBuilderExtensions](/Dovetail/v0.2.2/api/dovetail/servicehostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying Service parts to a [DovetailContextBuilder](/Dovetail/v0.2.2/api/dovetail/dovetailcontextbuilder.md).
- [ServiceJointDelegate](/Dovetail/v0.2.2/api/dovetail/servicejointdelegate.md) -- `delegate` -- Delegate for the Service part.
- [ServiceJointExtensions](/Dovetail/v0.2.2/api/dovetail/servicejointextensions.md) -- `class` `static` -- Extension methods for applying Service parts.
- [SetupAsyncJointDelegate](/Dovetail/v0.2.2/api/dovetail/setupasyncjointdelegate.md) -- `delegate` -- Async delegate for the Setup part.
- [SetupHostBuilderExtensions](/Dovetail/v0.2.2/api/dovetail/setuphostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying Setup parts to a [DovetailContextBuilder](/Dovetail/v0.2.2/api/dovetail/dovetailcontextbuilder.md).
- [SetupJointDelegate](/Dovetail/v0.2.2/api/dovetail/setupjointdelegate.md) -- `delegate` -- Delegate for the Setup part.
- [SetupJointExtensions](/Dovetail/v0.2.2/api/dovetail/setupjointextensions.md) -- `class` `static` -- Extension methods for applying Setup parts.
