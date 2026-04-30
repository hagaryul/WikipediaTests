# Wikipedia Automation Suite

Automation assignment for Genpact — UI & API testing of the [Playwright (software)](<https://en.wikipedia.org/wiki/Playwright_(software)>) Wikipedia page.

---

## Tech Stack

- **Language:** C#
- **Framework:** Playwright + NUnit
- **Target:** .NET 10

---

## Project Structure

```
WikipediaTests/
├── Clients/
│   ├── IWikipediaApiClient.cs
│   └── WikipediaApiClient.cs
├── Helpers/
│   └── TextHelper.cs
├── Pages/
│   └── WikipediaPage.cs
├── Reporting/
│   ├── ReportGenerator.cs
│   ├── ReportRunner.cs
│   ├── TestResultCollector.cs
│   └── Templates/
│       ├── report.html
│       └── report.css
├── Tests/
│   ├── Task1Tests.cs
│   ├── Task2Tests.cs
│   └── Task3Tests.cs
└── Constants.cs
---
```

## Tests

### Task 1 — UI vs API Text Comparison

Extracts the "Debugging features" section from Wikipedia both via UI (Playwright) and via the MediaWiki API, normalizes both texts, counts unique words, and asserts the counts are equal.

### Task 2 — Microsoft Development Tools Links Validation

Navigates to the "Microsoft development tools" table and validates that every technology name in the table is a hyperlink. If any item is not a link — the test fails.

### Task 3 — Dark Mode Theme Validation

Selects the "Dark" color theme from the Appearance panel and validates that the HTML element's class reflects the change to `skin-theme-clientpref-night`.

---

## How to Run

### Clone the repository

```bash
git clone https://github.com/hagaryul/WikipediaTests.git
cd WikipediaTests
```

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Install Playwright browsers

```bash
export PATH="$PATH:$HOME/.dotnet/tools"
playwright install
```

### Run tests

```bash
dotnet test
```

### View HTML Report

After running the tests, an HTML report is automatically generated at:
TestResults/report.html
Open it with:

```bash
open TestResults/report.html
```

---

## Architecture

- **POM (Page Object Model)** — all UI interactions are encapsulated in `WikipediaPage.cs`
- **Interface-based API client** — `IWikipediaApiClient` allows easy swapping of implementations
- **Constants** — all magic strings live in `Constants.cs`
- **Separation of concerns** — UI, API, helpers, and reporting are fully separated
- **Auto-generated HTML report** — generated automatically after every test run via `ReportRunner`
