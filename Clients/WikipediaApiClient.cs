using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace WikipediaTests.Clients;

public class WikipediaApiClient : IWikipediaApiClient
{
    private static readonly HttpClient _client = new();

    static WikipediaApiClient()
    {
        _client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; WikipediaTests/1.0)");
    }

    public async Task<string> GetSectionTextAsync(string pageName, string sectionAnchor)
    {
        var sectionIndex = await FindSectionIndexAsync(pageName, sectionAnchor);
        var url = $"{Constants.WikipediaBaseUrl}/w/api.php?action=parse&page={pageName}&prop=text&section={sectionIndex}&format=json";
        var response = await _client.GetStringAsync(url);
        var json = JsonDocument.Parse(response);
        var html = json.RootElement
            .GetProperty("parse")
            .GetProperty("text")
            .GetProperty("*")
            .GetString() ?? string.Empty;

        return CleanHtml(html);
    }

    private static async Task<string> FindSectionIndexAsync(string pageName, string sectionAnchor)
    {
        var url = $"{Constants.WikipediaBaseUrl}/w/api.php?action=parse&page={pageName}&prop=tocdata&format=json";
        var response = await _client.GetStringAsync(url);
        var json = JsonDocument.Parse(response);
        var sections = json.RootElement
            .GetProperty("parse")
            .GetProperty("tocdata")
            .GetProperty("sections");

        foreach (var section in sections.EnumerateArray())
        {
            if (section.GetProperty("anchor").GetString() == sectionAnchor)
                return section.GetProperty("index").GetString() ?? "0";
        }

        throw new InvalidOperationException($"Section '{sectionAnchor}' not found in page '{pageName}'");
    }

    private static string CleanHtml(string html)
    {
        html = RemoveCitationNumbers(html);
        html = RemoveReferencesSection(html);
        html = RemoveSectionHeading(html);
        html = StripHtmlTags(html);
        html = System.Net.WebUtility.HtmlDecode(html);
        return html;
    }

    private static string RemoveCitationNumbers(string html) =>
        Regex.Replace(html, @"<sup[^>]*>.*?</sup>", " ", RegexOptions.Singleline);

    private static string RemoveReferencesSection(string html) =>
        Regex.Replace(html, @"<div[^>]*mw-references-wrap[^>]*>.*?</div>", " ", RegexOptions.Singleline);

    private static string RemoveSectionHeading(string html) =>
        Regex.Replace(html, @"<div[^>]*mw-heading[^>]*>.*?</div>", " ", RegexOptions.Singleline);

    private static string StripHtmlTags(string html) =>
        Regex.Replace(html, @"<[^>]+>", " ");

    public void Dispose() { }
}