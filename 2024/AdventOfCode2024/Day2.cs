namespace AdventOfCode2024;

public class Day2
{
    public static void Solve()
    {
        var reports = File.ReadAllLines("input/day2.txt")
            .Select(s => s.Split(' ')
                .Select(int.Parse)
                .ToArray())
            .ToArray();

        var answer = reports.Count(IsValidReport);
        Console.WriteLine(answer);
        
        // Part 2
        var answer2 = reports.Sum(report => Variations(report).Any(IsValidReport) ? 1 : 0);
        Console.WriteLine(answer2);
    }

    private static bool IsValidReportAscending(int[] report)
    {
        for (var i = 0; i < report.Length - 1; i++)
        {
            var diff = report[i + 1] - report[i];
            if (diff is < 1 or > 3)
                return false;
        }

        return true;
    }
    
    private static bool IsValidReportDescending(int[] report)
    {
        for (var i = 0; i < report.Length - 1; i++)
        {
            var diff = report[i] - report[i + 1];
            if (diff is < 1 or > 3)
                return false;
        }

        return true;
    }
    
    private static bool IsValidReport(int[] report)
    {
        return IsValidReportAscending(report) || IsValidReportDescending(report);
    }

    private static List<int[]> Variations(int[] report)
    {
        List<int[]> possibilities = [];
        for (var i = 0; i < report.Length; ++i)
        {
            var copy = report.ToList();
            copy.RemoveAt(i);
            possibilities.Add(copy.ToArray());
        }

        return possibilities;
    }
}