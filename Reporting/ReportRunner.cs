using NUnit.Framework;
using WikipediaTests.Reporting;

namespace WikipediaTests;

[SetUpFixture]
public class ReportRunner
{
    internal static readonly List<TestResult> Results = new();

    [OneTimeTearDown]
    public void GenerateReport()
    {
        ReportGenerator.Generate(Results);
    }
}