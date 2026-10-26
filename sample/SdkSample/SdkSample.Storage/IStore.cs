namespace SdkSample.Storage;

public interface IStore
{
    /// <summary>
    ///     Describes how the store is set up from <see cref="StorageOptions" /> and <see cref="ArchiveOptions" />.
    /// </summary>
    string Describe();

    /// <summary>
    ///     Checks whether the store would accept a file of the given name and size, and says why not when it wouldn't.
    /// </summary>
    string Check(string fileName, int sizeMb);
}
