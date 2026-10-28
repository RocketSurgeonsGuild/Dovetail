using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Joinery.Dovetail.Configuration.Parity.Tests;

/// <summary>
///     Renders a bound value, or the exception binding threw, deterministically: strings quoted, collections with their runtime
///     type, objects as their public properties. Two binders agree when their renderings are equal.
/// </summary>
internal static class Render
{
    public static string Outcome(Func<object?> bind)
    {
        try
        {
            return Value(bind());
        }
        catch (Exception exception)
        {
            return Exception(exception);
        }
    }

    public static string Value(object? value, int depth = 0)
    {
        if (depth > 6)
        {
            return "...";
        }

        switch (value)
        {
            case null:
                return "null";
            case string text:
                return Quote(text);
            case char character:
                return character < ' ' ? $"'\\u{(int)character:x4}'" : "'" + character + "'";
            case bool flag:
                return flag ? "true" : "false";
            case Enum enumValue:
                return enumValue.GetType().Name + "." + enumValue;
            case Uri uri:
                return $"Uri({( uri.IsAbsoluteUri ? "abs" : "rel" )} {Quote(uri.OriginalString)})";
            case CultureInfo culture:
                return $"Culture({Quote(culture.Name)})";
            case byte[] bytes:
                return $"byte[{string.Join(",", bytes)}]";
            case DateTime dateTime:
                return dateTime.ToString("O", CultureInfo.InvariantCulture) + "(" + dateTime.Kind + ")";
            case DateTimeOffset dateTimeOffset:
                return dateTimeOffset.ToString("O", CultureInfo.InvariantCulture);
            case Version version:
                return "Version(" + version + ")";
            case IConfigurationSection section:
                return $"Section({section.Path}={Quote(section.Value)})";
            case IFormattable formattable when value.GetType().IsPrimitive || value is decimal or TimeSpan or Guid or DateOnly or TimeOnly or Half or Int128:
                return formattable.ToString(null, CultureInfo.InvariantCulture);
            case IDictionary dictionary:
                return TypeName(value.GetType())
                  + "{"
                  + string.Join(", ", dictionary.Keys.Cast<object>().Select(z => Value(z, depth + 1) + ": " + Value(dictionary[z], depth + 1)))
                  + "}";
            case IEnumerable enumerable:
                return TypeName(value.GetType()) + "[" + string.Join(", ", enumerable.Cast<object?>().Select(z => Value(z, depth + 1))) + "]";
            default:
                break;
        }

        var type = value.GetType();
        var parts = type
                   .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                   .Where(z => z.GetIndexParameters().Length == 0 && z.GetMethod is not null && z.Name != "EqualityContract")
                   .OrderBy(z => z.MetadataToken)
                   .Select(
                        z =>
                        {
                            try
                            {
                                return z.Name + "=" + Value(z.GetValue(value), depth + 1);
                            }
                            catch (TargetInvocationException exception)
                            {
                                return z.Name + "=<getter threw " + exception.InnerException?.GetType().Name + ">";
                            }
                        }
                    );
        return TypeName(type) + "{" + string.Join(", ", parts) + "}";
    }

    public static string Exception(Exception exception)
    {
        var text = new StringBuilder("EXC ");
        for (var current = exception; current is not null; current = current.InnerException)
        {
            if (!ReferenceEquals(current, exception))
            {
                text.Append(" --> ");
            }

            text.Append(current.GetType().FullName).Append(": ").Append(current.Message);
        }

        return text.ToString();
    }

    private static string Quote(string? text) => text is null ? "null" : "\"" + text.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";

    private static string TypeName(Type type)
    {
        if (type.IsArray)
        {
            return TypeName(type.GetElementType()!) + "[]";
        }

        if (!type.IsGenericType)
        {
            return type.Name;
        }

        var name = type.Name;
        var tick = name.IndexOf('`', StringComparison.Ordinal);
        return ( tick >= 0 ? name[..tick] : name ) + "<" + string.Join(",", type.GetGenericArguments().Select(TypeName)) + ">";
    }
}
