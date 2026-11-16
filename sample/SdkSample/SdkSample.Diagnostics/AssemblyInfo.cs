using Optionary;
using Microsoft.Extensions.Hosting;

// Types this library doesn't own: Optionary binds them from these sections and describes them in the merged schema, using
// the XML documentation that ships with Microsoft.Extensions.Hosting.
[assembly: Optionary<HostOptions>("Host")]
[assembly: Optionary<ConsoleLifetimeOptions>("Console")]
