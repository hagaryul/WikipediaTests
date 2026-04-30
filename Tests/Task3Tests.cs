using Microsoft.Playwright.NUnit;
using WikipediaTests.Pages;
using WikipediaTests.Reporting;

namespace WikipediaTests.Tests;

[CollectResults]
public class Task3Tests : PageTest
{
    private WikipediaPage _wikipediaPage = null!;

    [SetUp]
    public async Task SetUp()
    {
        _wikipediaPage = new WikipediaPage(Page);
        await _wikipediaPage.NavigateAsync(Constants.PlaywrightPageName);
    }

    [Test]
    public async Task SelectingDarkColorShouldChangeTheme()
    {
        await _wikipediaPage.SelectColorThemeAsync(Constants.LightThemeInputId);
        var initialTheme = await _wikipediaPage.GetHtmlThemeClassAsync();
        Assert.That(initialTheme, Is.EqualTo(Constants.LightThemeClass),
            "Could not set initial Light theme");

        await _wikipediaPage.SelectColorThemeAsync(Constants.DarkThemeInputId);
        var themeClass = await _wikipediaPage.GetHtmlThemeClassAsync();

        Assert.That(themeClass, Is.EqualTo(Constants.DarkThemeClass),
            $"Expected '{Constants.DarkThemeClass}' but got '{themeClass}'");
    }
}