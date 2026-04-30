using System.Text.RegularExpressions;

namespace WikipediaTests.Helpers;

public static class TextHelper
{
    public static HashSet<string> GetUniqueWords(string text)
    {
        var normalized = text.ToLower();
        normalized = normalized.Replace("\u00A0", " ");
        normalized = Regex.Replace(normalized, @"[^a-z0-9\s]", " ");
        var words = normalized.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        return new HashSet<string>(words);
    }
}