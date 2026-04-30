namespace WikipediaTests.Clients;

public interface IWikipediaApiClient : IDisposable
{
    Task<string> GetSectionTextAsync(string pageName, string sectionAnchor);
}