using Microsoft.Playwright.NUnit;
using WikipediaTests.Pages;
using WikipediaTests.Reporting;

namespace WikipediaTests.Tests;

[CollectResults]
public class Task2Tests : PageTest
{
    private WikipediaPage _wikipediaPage = null!;

    [SetUp]
    public async Task SetUp()
    {
        _wikipediaPage = new WikipediaPage(Page);
        await _wikipediaPage.NavigateAsync(Constants.PlaywrightPageName);
    }

    [Test]
    public async Task AllMicrosoftDevToolsTechnologyNamesShouldBeLinks()
    {
        var nonLinks = await _wikipediaPage.GetNonLinkItemsFromTableAsync(Constants.MicrosoftDevToolsTableTitle);

        Assert.That(nonLinks, Is.Empty,
            $"The following technologies in '{Constants.MicrosoftDevToolsTableTitle}' are not links:\n" +
            string.Join("\n", nonLinks.Select((item, i) => $"  {i + 1}. {item}")));
    }
}