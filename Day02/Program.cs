// See https://aka.ms/new-console-template for more information

using Report = System.Collections.Generic.IList<int>;

var contents = File.ReadLines(@"..\..\..\input.txt");

var reports = contents.Select(line => line.Split(" ").Select(s => Convert.ToInt32(s)).ToList()).ToList();

var safeReports = reports.Where(IsSafe);
Console.WriteLine(safeReports.Count());

var safeMakableReports = reports.Where(CanBeMadeSafe);
Console.WriteLine(safeMakableReports.Count());

return;

bool IsSafe(Report report)
{
    var differences = report
        .Zip(report.Skip(1), Tuple.Create)
        .Select(t => t.Item2 - t.Item1)
        .ToList();
    
    return differences.All(d => Math.Abs(d) is >= 1 and <= 3 && Math.Sign(d) == Math.Sign(differences.First()));
}

bool CanBeMadeSafe(Report report)
{
    return report
        .Select((_, index) => report.Where((_, i) => i != index).ToList())
        .Any(IsSafe);
}
