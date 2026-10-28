// Ported from dotnet/runtime (MIT): src/libraries/Common/src/SourceGenerators/SourceWriter.cs.

using System.Text;

namespace Joinery.Dovetail.Support.Configuration.Binding;

/// <summary>Writes indented source text.</summary>
internal sealed class SourceWriter
{
    private readonly StringBuilder _builder = new();

    public int Indentation
    {
        get;
        set => field = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value));
    }

    /// <summary>Writes each line of <paramref name="text" /> at the current indentation.</summary>
    public void WriteLine(string text)
    {
        if (Indentation == 0)
        {
            _builder.Append(text).Append('\n');
            return;
        }

        foreach (var line in text.Split('\n'))
        {
            var content = line.TrimEnd('\r');
            if (content.Length > 0)
            {
                _builder.Append(' ', 4 * Indentation).Append(content);
            }

            _builder.Append('\n');
        }
    }

    public void WriteLine() => _builder.Append('\n');

    public override string ToString() => _builder.ToString();
}
