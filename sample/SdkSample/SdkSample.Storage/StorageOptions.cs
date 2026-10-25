using System.ComponentModel.DataAnnotations;
using Dovetail.Attributes;

namespace SdkSample.Storage;

/// <summary>
///     Where and how files are stored. A positional record: Dovetail calls its constructor with the values under the
///     <c>Storage</c> section, and a parameter that has a default keeps it when its key is missing.
/// </summary>
/// <param name="Root">The directory the store keeps its files in.</param>
/// <param name="MaxFileSizeMb">The largest file the store accepts, in megabytes.</param>
/// <param name="Compress">Whether files are compressed before they are written.</param>
[DovetailConfiguration]
public sealed record StorageOptions(string Root = "data", [property: Range(1, 1024)] int MaxFileSizeMb = 64, bool Compress = false);
