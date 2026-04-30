using Microsoft.Playwright;

namespace WikipediaTests.Pages;

public class WikipediaPage
{
    private readonly IPage _page;

    public WikipediaPage(IPage page)
    {
        _page = page;
    }

    public async Task NavigateAsync(string pageName)
    {
        await _page.GotoAsync($"{Constants.WikipediaBaseUrl}/wiki/{pageName}");
    }

    public async Task<string> GetSectionTextAsync(string sectionId)
    {
        var text = await _page.EvaluateAsync<string>(
            @"(sectionId) => {
                const heading = document.getElementById(sectionId);
                if (!heading) return '';
                const headingDiv = heading.closest('.mw-heading');
                if (!headingDiv) return '';
                let result = '';
                let current = headingDiv.nextElementSibling;
                while (current && !current.classList.contains('mw-heading')) {
                    const clone = current.cloneNode(true);
                    clone.querySelectorAll('.mw-editsection').forEach(el => el.remove());
                    clone.querySelectorAll('sup').forEach(el => el.remove());
                    result += clone.innerText + ' ';
                    current = current.nextElementSibling;
                }
                return result;
            }",
            sectionId
        );
        return text ?? string.Empty;
    }

    public async Task<string[]> GetNonLinkItemsFromTableAsync(string tableTitle)
    {
        var nonLinks = await _page.EvaluateAsync<string[]>(
            @"(tableTitle) => {
                const targetTable = Array.from(document.querySelectorAll('table'))
                    .find(table => table.innerText.includes(tableTitle));

                if (!targetTable) return [];

                return Array.from(targetTable.querySelectorAll('li'))
                    .filter(item => item.querySelector('a') === null)
                    .map(item => item.innerText.trim());
            }",
            tableTitle
        );
        return nonLinks ?? Array.Empty<string>();
    }

    public async Task SelectColorThemeAsync(string themeInputId)
    {
        await _page.Locator($"#{themeInputId}").ClickAsync();
    }

    public async Task<string> GetHtmlThemeClassAsync()
    {
        return await _page.EvaluateAsync<string>(@"() => {
            const classes = document.documentElement.className;
            const match = classes.match(/skin-theme-clientpref-(day|night|os)/);
            return match ? match[0] : '';
        }") ?? string.Empty;
    }
}