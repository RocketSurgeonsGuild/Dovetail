using Microsoft.Extensions.Options;

namespace SdkSample.Storage;

internal sealed class Store(IOptions<StorageOptions> storage, IOptions<ArchiveOptions> archive) : IStore
{
    public string Describe()
    {
        var (root, maxFileSizeMb, compress) = storage.Value;
        var options = archive.Value;
        var extensions = options.AllowedExtensions.Length == 0 ? "any extension" : string.Join(" ", options.AllowedExtensions);
        return $"Store '{options.Name}' in {root}: files up to {maxFileSizeMb} MB{(compress ? ", compressed" : "")}, kept {options.RetentionDays} days, accepts {extensions}.";
    }

    public string Check(string fileName, int sizeMb)
    {
        var maxFileSizeMb = storage.Value.MaxFileSizeMb;
        var extensions = archive.Value.AllowedExtensions;
        if (sizeMb > maxFileSizeMb) return $"{fileName} ({sizeMb} MB) is rejected: over the {maxFileSizeMb} MB limit.";
        if (extensions.Length > 0 && !extensions.Contains(Path.GetExtension(fileName), StringComparer.OrdinalIgnoreCase))
            return $"{fileName} is rejected: '{Path.GetExtension(fileName)}' is not an allowed extension.";

        return $"{fileName} ({sizeMb} MB) is accepted.";
    }
}
