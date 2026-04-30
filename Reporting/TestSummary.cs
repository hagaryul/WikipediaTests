namespace WikipediaTests.Reporting;

public class TestSummary
{
    public int Total { get; set; }
    public int Passed { get; set; }
    public int Failed { get; set; }
    public int PassRate { get; set; }
    public string Date { get; set; } = "";
}