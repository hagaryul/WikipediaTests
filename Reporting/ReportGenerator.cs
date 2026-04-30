using System.IO;

namespace WikipediaTests.Reporting;

public static class ReportGenerator
{
    private static readonly string TemplatesDir = Path.Combine(
     AppContext.BaseDirectory, "..", "..", "..", "Reporting", "Templates"
 );
    private static readonly string OutputDir = Path.Combine(
        AppContext.BaseDirectory, "..", "..", "..", "TestResults"
    );

    public static void Generate(List<TestResult> results)
    {
        var summary = BuildSummary(results);
        var templateHtml = File.ReadAllText(Path.Combine(TemplatesDir, "report.html"));
        var templateCss = File.ReadAllText(Path.Combine(TemplatesDir, "report.css"));

        var filledCss = templateCss.Replace("{{PASS_RATE}}", summary.PassRate.ToString());
        var filledHtml = FillTemplate(templateHtml, results, summary);

        Directory.CreateDirectory(OutputDir);
        File.WriteAllText(Path.Combine(OutputDir, "report.html"), filledHtml);
        File.WriteAllText(Path.Combine(OutputDir, "report.css"), filledCss);

    }

    private static TestSummary BuildSummary(List<TestResult> results)
    {
        var total = results.Count;
        var passed = results.Count(r => r.Outcome == "Passed");
        var failed = total - passed;

        return new TestSummary
        {
            Total = total,
            Passed = passed,
            Failed = failed,
            Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
            PassRate = total > 0 ? (int)Math.Round((double)passed / total * 100) : 0
        };
    }

    private static string FillTemplate(string template, List<TestResult> results, TestSummary summary)
    {
        var rows = string.Join("\n", results.Select((r, i) =>
        {
            var icon = r.Outcome == "Passed" ? "✓" : "✗";
            var statusClass = r.Outcome == "Passed" ? "passed" : "failed";
            return $"""
                <tr class="fade-in" style="animation-delay: {i * 0.1}s">
                    <td><span class="icon {statusClass}">{icon}</span></td>
                    <td>{r.Name}</td>
                    <td><span class="badge {statusClass}">{r.Outcome}</span></td>
                    <td>{r.Duration}</td>
                </tr>
            """;
        }));

        return template
            .Replace("{{TOTAL}}", summary.Total.ToString())
            .Replace("{{PASSED}}", summary.Passed.ToString())
            .Replace("{{FAILED}}", summary.Failed.ToString())
            .Replace("{{PASS_RATE}}", summary.PassRate.ToString())
            .Replace("{{DATE}}", summary.Date)
            .Replace("{{ROWS}}", rows);
    }
}

public class TestSummary
{
    public int Total { get; set; }
    public int Passed { get; set; }
    public int Failed { get; set; }
    public int PassRate { get; set; }
    public string Date { get; set; } = "";
}