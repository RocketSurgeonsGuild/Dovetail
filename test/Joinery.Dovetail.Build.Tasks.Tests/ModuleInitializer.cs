using System.Runtime.CompilerServices;
using DiffEngine;

namespace Joinery.Dovetail.Build.Tasks.Tests;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        DiffRunner.Disabled = true;
        DerivePathInfo(
            (sourceFile, projectDirectory, type, method) =>
            {
                static string GetTypeName(Type type) => type.IsNested ? $"{type.ReflectedType!.Name}.{type.Name}" : type.Name;

                var path = Path.Combine(Path.GetDirectoryName(sourceFile)!, "snapshots");
                return new(path, GetTypeName(type), method.Name);
            }
        );
    }
}
