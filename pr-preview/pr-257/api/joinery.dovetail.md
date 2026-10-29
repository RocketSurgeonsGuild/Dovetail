# Joinery.Dovetail

- Package: `Joinery.Dovetail`
- Version: `0.2.4-pr.14`
- Target framework: `net10.0`
- NuGet: [Joinery.Dovetail](https://www.nuget.org/packages/Joinery.Dovetail)

## Namespaces

### Joinery.Dovetail

- [DovetailCategory](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailcategory.md) -- `class` `sealed` -- The category of a given convention
- [DovetailConfigurationContextExtensions](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailconfigurationcontextextensions.md) -- `class` `static` -- Extension members for IDovetailContext configuration access.
- [DovetailConfigurationOptionsExtensions](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailconfigurationoptionsextensions.md) -- `class` `static` -- Registers strongly-typed configuration classes with the standard `Extensions.Options` pipeline.
- [DovetailContext](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailcontext.md) -- `class` `sealed` -- Base convention context that allows for stashing items in index keys Implements the [IDovetailContext](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/idovetailcontext.md)
- [DovetailContextBuilder](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailcontextbuilder.md) -- `class` `sealed` -- Builder that can be used to create a context.
- [DovetailContextExtensions](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailcontextextensions.md) -- `class` `static` -- Base convention extensions
- [DovetailEmbeddedConfiguration](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailembeddedconfiguration.md) -- `class` `static` -- Loads configuration files that the Dovetail SDK embedded into an assembly as manifest resources, for hosts that have no file system to read them from (mobile and other app-package hosts).
- [DovetailExceptionPolicy](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailexceptionpolicy.md) -- `class` `static` -- A policy that determines how to handle exceptions thrown by conventions
- [DovetailExceptionPolicyDelegate](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailexceptionpolicydelegate.md) -- `delegate` -- A policy delegate that can be used to determine if a given exception should be ignored or not
- [DovetailHostBuilderExtensions](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailhostbuilderextensions.md) -- `class` `static` -- Base convention extensions
- [DovetailHostType](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailhosttype.md) -- `enum` -- The underlying host type (currently only checked for test host) This is so that conventions can avoid adding behaviors that might cause issues with unit testing such as writing to the console or debug pipes, or ensuring that something is configured in a correct way for testing. Careful consideration is needed to make sure that your system doesn't behave extremely differently in a live scenario vs a testing scenario
- [IDovetailContext](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/idovetailcontext.md) -- `interface` -- The base context marker interface to define this as a context
- [ImportHelpers](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/importhelpers.md) -- `class` `static` -- This class is used by the source generator to allow the conventions provided to be overridden by the caller.

### Joinery.Dovetail.Attributes

- [AfterJointAttribute](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/afterjointattribute.md) -- `class` `sealed` -- An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/idovetailjoint.md)
- [AfterJointAttribute<T>](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/afterjointattribute-1.md) -- `class` `sealed` -- An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/idovetailjoint.md)
- [BeforeJointAttribute](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/beforejointattribute.md) -- `class` `sealed` -- An attribute that ensures the convention runs before the given [IDovetailJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/idovetailjoint.md)
- [BeforeJointAttribute<T>](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/beforejointattribute-1.md) -- `class` `sealed` -- An attribute that ensures the convention runs before the given [IDovetailJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/idovetailjoint.md)
- [ConfigurationKeyStyle](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/configurationkeystyle.md) -- `enum` -- How [DovetailConfigurationAttribute](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailconfigurationattribute.md) spells the configuration keys of an options type and of every object type nested in it.
- [DependentOfJointAttribute](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dependentofjointattribute.md) -- `class` `sealed` -- An attribute that ensures the convention runs before the given [IDovetailJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/idovetailjoint.md)
- [DependentOfJointAttribute<T>](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dependentofjointattribute-1.md) -- `class` `sealed` -- An attribute that ensures the convention runs before the given [IDovetailJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/idovetailjoint.md)
- [DependsOnJointAttribute](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dependsonjointattribute.md) -- `class` `sealed` -- An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/idovetailjoint.md)
- [DependsOnJointAttribute<T>](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dependsonjointattribute-1.md) -- `class` `sealed` -- An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/idovetailjoint.md)
- [DovetailCategoryAttribute](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailcategoryattribute.md) -- `class` `sealed` -- Defines the category of a given convention
- [DovetailConfigurationAttribute](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailconfigurationattribute.md) -- `class` `sealed` -- Declares the configuration section of an options class. Dovetail's source generator describes the class as JSON Schema, which is merged into the application's `appsettings.schema.json` for editor completion and validation, and registers it with the options pipeline.
- [DovetailConfigurationAttribute<T>](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailconfigurationattribute-1.md) -- `class` `sealed` -- Declares the configuration section of an options type `T` from the assembly, for types you don't own.
- [DovetailExportAttribute](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailexportattribute.md) -- `class` `sealed` -- Defines this convention as one that is exported to the Dovetail source generator
- [DovetailHostTypeAttribute](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailhosttypeattribute.md) -- `class` `sealed` -- Defines the host type of a given convention
- [LiveJointAttribute](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/livejointattribute.md) -- `class` `sealed` -- Defines this convention as one that only runs during live usage to avoid unit tests
- [UnitTestJointAttribute](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/unittestjointattribute.md) -- `class` `sealed` -- Defines this convention as one that only runs during a unit test run

### Joinery.Dovetail.Diagnostics

- [DiagnosticExtensions](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/diagnosticextensions.md) -- `class` `static` -- Provides extension methods for retrieving metadata from Dovetail context builders and contexts.
- [DovetailDiagnosticMetadataJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetaildiagnosticmetadatajoint.md) -- `record` -- Represents a metadata item for a Dovetail joint, including its name, host type, category, and dependencies.
- [DovetailDiagnosticMetadataJointDependency](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetaildiagnosticmetadatajointdependency.md) -- `record` `sealed` -- Represents a metadata dependency for a Dovetail joint, including its name, host type, category, dependencies, and direction.
- [DovetailMetadataDiagnosticExtensions](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailmetadatadiagnosticextensions.md) -- `class` `static` -- Extension methods for [IDovetailJointMetadata](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/idovetailjointmetadata.md) to provide additional diagnostic information.

### Joinery.Dovetail.Infrastructure

- [ConnectsJointAttribute<T>](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/connectsjointattribute-1.md) -- `class` `sealed` -- A marker interface to indicate a type is a convention and has a name
- [DependencyDirection](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dependencydirection.md) -- `enum` -- Defines the dependency direction of the given type
- [DovetailContextBuilderExtensions](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailcontextbuilderextensions.md) -- `class` `static` -- Dovetail Context build extensions.
- [DovetailContextBuilderFactory](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailcontextbuilderfactory.md) -- `delegate` -- A delegate that can be used to create a [DovetailContextBuilder](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailcontextbuilder.md).
- [DovetailExecutor](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailexecutor.md) -- `class` -- A class to help with executing conventions
- [DovetailJointMetadata](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailjointmetadata.md) -- `class` `sealed` -- Container for conventions
- [IDovetailDependency](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/idovetaildependency.md) -- `interface` -- A dependency for a given convention
- [IDovetailJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/idovetailjoint.md) -- `interface` -- A marker interface to indicate a type is a joint
- [IDovetailJointDelegate](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/idovetailjointdelegate.md) -- `interface` -- A marker interface to indicate a type is a joint delegate
- [IDovetailJointMetadata](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/idovetailjointmetadata.md) -- `interface` -- Declares a convention result with it's dependencies pre-computed
- [JointNameAttribute](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/jointnameattribute.md) -- `class` `sealed` -- A marker interface to indicate a type is a convention and has a name
- [LoggingConnector](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/loggingconnector.md) -- `class` `sealed` -- A joint that registers logging services into the context. This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.
- [ServiceProviderDictionaryExtensions](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/serviceproviderdictionaryextensions.md) -- `class` `static` -- Base convention extensions
- [ServiceProviderFactoryAdapter](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/serviceproviderfactoryadapter.md) -- `delegate` -- A factory that provides a service provider factory from the convention context

### Joinery.Dovetail.Joints

- [ConfigurationAsyncJointDelegate](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/configurationasyncjointdelegate.md) -- `delegate` -- Async delegate for the Configuration part.
- [ConfigurationHostBuilderExtensions](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/configurationhostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying Configuration parts to a [DovetailContextBuilder](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailcontextbuilder.md).
- [ConfigurationJointDelegate](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/configurationjointdelegate.md) -- `delegate` -- Delegate for the Configuration part.
- [ConfigurationJointExtensions](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/configurationjointextensions.md) -- `class` `static` -- Extension methods for applying Configuration parts.
- [HostCreatedAsyncJointDelegate<THost>](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/hostcreatedasyncjointdelegate-1.md) -- `delegate` -- Async delegate for the HostCreated part.
- [HostCreatedHostBuilderExtensions](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/hostcreatedhostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying HostCreated parts to a [DovetailContextBuilder](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailcontextbuilder.md).
- [HostCreatedJointDelegate<THost>](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/hostcreatedjointdelegate-1.md) -- `delegate` -- Delegate for the HostCreated part.
- [HostCreatedJointExtensions](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/hostcreatedjointextensions.md) -- `class` `static` -- Extension methods for applying HostCreated parts.
- [IConfigurationAsyncJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/iconfigurationasyncjoint.md) -- `interface` -- Async part interface for Configuration. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/idovetailjoint.md)
- [IConfigurationJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/iconfigurationjoint.md) -- `interface` -- Joint interface for Configuration. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/idovetailjoint.md)
- [IHostCreatedAsyncJoint<THost>](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/ihostcreatedasyncjoint-1.md) -- `interface` -- Async part interface for HostCreated. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/idovetailjoint.md)
- [IHostCreatedJoint<THost>](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/ihostcreatedjoint-1.md) -- `interface` -- Joint interface for HostCreated. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/idovetailjoint.md)
- [ILoggingAsyncJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/iloggingasyncjoint.md) -- `interface` -- Async part interface for Logging. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/idovetailjoint.md)
- [ILoggingJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/iloggingjoint.md) -- `interface` -- Joint interface for Logging. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/idovetailjoint.md)
- [IServiceAsyncJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/iserviceasyncjoint.md) -- `interface` -- Async part interface for Service. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/idovetailjoint.md)
- [IServiceJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/iservicejoint.md) -- `interface` -- Joint interface for Service. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/idovetailjoint.md)
- [ISetupAsyncJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/isetupasyncjoint.md) -- `interface` -- Async part interface for Setup. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/idovetailjoint.md)
- [ISetupJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/isetupjoint.md) -- `interface` -- Joint interface for Setup. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/idovetailjoint.md)
- [LoggingAsyncJointDelegate](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/loggingasyncjointdelegate.md) -- `delegate` -- Async delegate for the Logging part.
- [LoggingHostBuilderExtensions](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/logginghostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying Logging parts to a [DovetailContextBuilder](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailcontextbuilder.md).
- [LoggingJointDelegate](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/loggingjointdelegate.md) -- `delegate` -- Delegate for the Logging part.
- [LoggingJointExtensions](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/loggingjointextensions.md) -- `class` `static` -- Extension methods for applying Logging parts.
- [ServiceAsyncJointDelegate](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/serviceasyncjointdelegate.md) -- `delegate` -- Async delegate for the Service part.
- [ServiceHostBuilderExtensions](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/servicehostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying Service parts to a [DovetailContextBuilder](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailcontextbuilder.md).
- [ServiceJointDelegate](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/servicejointdelegate.md) -- `delegate` -- Delegate for the Service part.
- [ServiceJointExtensions](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/servicejointextensions.md) -- `class` `static` -- Extension methods for applying Service parts.
- [SetupAsyncJointDelegate](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/setupasyncjointdelegate.md) -- `delegate` -- Async delegate for the Setup part.
- [SetupHostBuilderExtensions](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/setuphostbuilderextensions.md) -- `class` `static` -- Builder-method extensions for applying Setup parts to a [DovetailContextBuilder](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/dovetailcontextbuilder.md).
- [SetupJointDelegate](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/setupjointdelegate.md) -- `delegate` -- Delegate for the Setup part.
- [SetupJointExtensions](/Dovetail/pr-preview/pr-257/api/joinery.dovetail/setupjointextensions.md) -- `class` `static` -- Extension methods for applying Setup parts.
