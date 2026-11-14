using System.Runtime.CompilerServices;
using AngleSharp.Dom;
using DiffEngine;
using VerifyTests.AngleSharp;

namespace Joinery.Dovetail.WebAssembly.Hosting.Tests;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        // PlaywrightFixture installs the one browser it launches; Initialize(true) would install all of them, on every target framework.
        VerifyPlaywright.Initialize();
        VerifyAngleSharpDiffing.Initialize();
        HtmlPrettyPrint.All(
            list =>
            {
                list.ScrubAttributes(attr => attr.Name.StartsWith("b-") && attr.Name.Length == 12);
                foreach (var comment in list.DescendantsAndSelf<IComment>().Where(z => z.NodeValue == "!").ToArray())
                {
                    comment.Remove();
                }
            }
        );
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
    }
}
