using Microsoft.Playwright.NUnit;
using WikipediaTests.Pages;
using WikipediaTests.Helpers;
using WikipediaTests.Clients;
using WikipediaTests.Reporting;

namespace WikipediaTests.Tests;

[CollectResults]
public class Task1Tests : PageTest
{
    private IWikipediaApiClient _apiClient = null!;
    private WikipediaPage _wikipediaPage = null!;

    [SetUp]
    public void SetUp()
    {
        _apiClient = new WikipediaApiClient();
        _wikipediaPage = new WikipediaPage(Page);
    }

    [TearDown]
    public void TearDown()
    {
        _apiClient.Dispose();
    }

    [Test]
    public async Task UniqueWordCountShouldBeEqualInUIAndAPI()
    {
        await _wikipediaPage.NavigateAsync(Constants.PlaywrightPageName);

        var uiText = await _wikipediaPage.GetSectionTextAsync(Constants.DebuggingFeaturesSectionName);
        var apiText = await _apiClient.GetSectionTextAsync(Constants.PlaywrightPageName, Constants.DebuggingFeaturesSectionName);

        var uiWords = TextHelper.GetUniqueWords(uiText);
        var apiWords = TextHelper.GetUniqueWords(apiText);

        Assert.That(uiWords.Count, Is.EqualTo(apiWords.Count),
            $"UI unique words: {uiWords.Count}, API unique words: {apiWords.Count}");
    }
}