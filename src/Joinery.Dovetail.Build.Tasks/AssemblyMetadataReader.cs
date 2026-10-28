using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace Joinery.Dovetail.Build.Tasks;

/// <summary>
///     Reads <c>[assembly: AssemblyMetadata(key, value)]</c> attributes straight from an assembly's metadata, without loading it.
/// </summary>
internal static class AssemblyMetadataReader
{
    /// <summary>
    ///     Returns the attributes whose key starts with <paramref name="keyPrefix" />, in metadata order.
    /// </summary>
    /// <exception cref="IOException">The file can't be read.</exception>
    /// <exception cref="BadImageFormatException">The file isn't a PE image.</exception>
    public static IReadOnlyList<KeyValuePair<string, string?>> Read(string path, string keyPrefix)
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
        using var image = new PEReader(stream);
        if (!image.HasMetadata) return [];

        var reader = image.GetMetadataReader();
        if (!reader.IsAssembly) return [];

        var result = new List<KeyValuePair<string, string?>>();
        foreach (var handle in reader.GetAssemblyDefinition().GetCustomAttributes())
        {
            var attribute = reader.GetCustomAttribute(handle);
            if (!IsAssemblyMetadataAttribute(reader, attribute.Constructor)) continue;

            var blob = reader.GetBlobReader(attribute.Value);
            // The prolog, then the two constructor arguments as serialized strings.
            if (blob.Length < 2 || blob.ReadUInt16() != 1) continue;
            var key = blob.ReadSerializedString();
            if (key is null || !key.StartsWith(keyPrefix, StringComparison.Ordinal)) continue;
            result.Add(new(key, blob.ReadSerializedString()));
        }

        return result;
    }

    private static bool IsAssemblyMetadataAttribute(MetadataReader reader, EntityHandle constructor)
    {
        // The attribute type is a reference, or (for an assembly that defines it itself, e.g. a polyfill) a definition.
        var type = constructor.Kind == HandleKind.MemberReference
            ? reader.GetMemberReference((MemberReferenceHandle)constructor).Parent
            : constructor.Kind == HandleKind.MethodDefinition
                ? reader.GetMethodDefinition((MethodDefinitionHandle)constructor).GetDeclaringType()
                : default(EntityHandle);
        if (type.Kind == HandleKind.TypeReference)
        {
            var reference = reader.GetTypeReference((TypeReferenceHandle)type);
            return IsAssemblyMetadataAttribute(reader, reference.Name, reference.Namespace);
        }

        if (type.Kind == HandleKind.TypeDefinition)
        {
            var definition = reader.GetTypeDefinition((TypeDefinitionHandle)type);
            return IsAssemblyMetadataAttribute(reader, definition.Name, definition.Namespace);
        }

        return false;
    }

    private static bool IsAssemblyMetadataAttribute(MetadataReader reader, StringHandle name, StringHandle @namespace) =>
        reader.StringComparer.Equals(name, "AssemblyMetadataAttribute") && reader.StringComparer.Equals(@namespace, "System.Reflection");
}
