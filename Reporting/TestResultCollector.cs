using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace WikipediaTests.Reporting;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class CollectResultsAttribute : Attribute, ITestAction
{
    private static readonly ConcurrentDictionary<string, Stopwatch> Timers = new();

    public void BeforeTest(ITest test)
    {
        if (test.IsSuite) return;
        Timers[test.FullName] = Stopwatch.StartNew();
    }

    public void AfterTest(ITest test)
    {
        if (test.IsSuite) return;

        Timers.TryGetValue(test.FullName, out var sw);
        sw?.Stop();

        var context = TestContext.CurrentContext;
        ReportRunner.Results.Add(new TestResult
        {
            Name = context.Test.Name,
            Outcome = context.Result.Outcome.Status.ToString(),
            Duration = sw != null ? $"{sw.Elapsed.TotalSeconds:F2}s" : "N/A"
        });
    }

    public ActionTargets Targets => ActionTargets.Test;
}