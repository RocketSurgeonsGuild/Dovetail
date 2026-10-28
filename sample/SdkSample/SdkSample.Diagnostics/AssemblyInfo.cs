using Joinery.Dovetail.Attributes;
using Microsoft.Extensions.Hosting;

// Types this library doesn't own: Dovetail binds them from these sections and describes them in the merged schema, using
// the XML documentation that ships with Microsoft.Extensions.Hosting.
[assembly: DovetailConfiguration<HostOptions>("Host")]
[assembly: DovetailConfiguration<ConsoleLifetimeOptions>("Console")]
