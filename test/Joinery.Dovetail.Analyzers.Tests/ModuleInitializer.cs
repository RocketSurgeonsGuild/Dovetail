using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using DiffEngine;
using Rocket.Surgery.Extensions.Testing.SourceGenerators;

namespace Joinery.Dovetail.Analyzers.Tests;

public static class ModuleInitializer
{
    [ModuleInitializer]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Init()
    {
        VerifyGeneratorTextContext.Initialize(Customizers.Default);

        DiffRunner.Disabled = true;
        DerivePathInfo(
            (sourceFile, projectDirectory, type, method) =>
            {
                static string GetTypeName(Type type) => type.IsNested ? $"{type.ReflectedType!.Name}.{type.Name}" : type.Name;

                var typeName = GetTypeName(type);

                var path = Path.Combine(Path.GetDirectoryName(sourceFile)!, "snapshots");
                return new(path, typeName, method.Name);
            }
        );

        VerifierSettings.SortJsonObjects();
        VerifierSettings.ScrubInlineGuids();
        VerifierSettings.ScrubLinesWithReplace(
            s =>
            {
                return s.Contains(
                        "DovetailConfigurationData.AssemblyProvider.",
                        StringComparison.OrdinalIgnoreCase
                    )
                    ? s[..( s.IndexOf('"', s.IndexOf('"') + 1) + 2 )] + "\"\")]"
                    : s;
            }
        );
        VerifierSettings.AddScrubber(
            (builder, counter) =>
            {
                // The generator's versions, read from the typed dependency information rather than its assembly.
                if (AssemblyInfo.Dependencies.Joinery_Dovetail_Analyzers.Attributes.FileVersion is { Length: > 0, } version)
                {
                    builder.Replace(version, "version");
                }

                if (AssemblyInfo.Dependencies.Joinery_Dovetail_Analyzers.Attributes.Version is { Length: > 0, } version2)
                {
                    builder.Replace(version2, "version");
                }
                // regex to replace the version number in this string Version=12.0.0.0,
                var regex = new Regex("Version=(.*?),", RegexOptions.Compiled);
                var result = regex.Replace(builder.ToString(), "Version=version,");
                builder.Clear();
                builder.Append(result);
            }
        );
    }
}
