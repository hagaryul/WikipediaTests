using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace WikipediaTests.Clients;

public class WikipediaApiClient : IWikipediaApiClient
{
    private readonly HttpClient _client;

    public WikipediaApiClient()
    {
        _client = new HttpClient();
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

    private async Task<string> FindSectionIndexAsync(string pageName, string sectionAnchor)
    {
        var url = $"{Constants.WikipediaBaseUrl}/w/api.php?action=parse&page={pageName}&prop=sections&format=json";
        var response = await _client.GetStringAsync(url);
        var json = JsonDocument.Parse(response);
        var sections = json.RootElement
            .GetProperty("parse")
            .GetProperty("sections");

        foreach (var section in sections.EnumerateArray())
        {
            if (section.GetProperty("anchor").GetString() == sectionAnchor)
                return section.GetProperty("index").GetString() ?? "0";
        }

        throw new Exception($"Section '{sectionAnchor}' not found in page '{pageName}'");
    }

    private static string CleanHtml(string html)
    {
        html = Regex.Replace(html, @"<sup[^>]*>.*?</sup>", " ", RegexOptions.Singleline);
        html = Regex.Replace(html, @"<div[^>]*mw-references-wrap[^>]*>.*?</div>", " ", RegexOptions.Singleline);
        html = Regex.Replace(html, @"<div[^>]*mw-heading[^>]*>.*?</div>", " ", RegexOptions.Singleline);
        html = Regex.Replace(html, @"<[^>]+>", " ");
        html = System.Net.WebUtility.HtmlDecode(html);
        return html;
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}