using System.Globalization;
using System.Text;

namespace Banking.Core;

public static class StringExtensions
{
    public static string? RemoveDiacritics(this string? self)
    {
        if (self is null) return null;
        var normalizedString = self.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory == UnicodeCategory.NonSpacingMark) continue;
            stringBuilder.Append(c);
        }

        return stringBuilder
               .ToString()
               .Normalize(NormalizationForm.FormC);
    }

    public static string? Truncate(this string? self, int size)
    {
        return (self?.Length ?? 0) > size ? self![..size] : self;
    }
}