namespace AdventOfCode2024;

public class Day18(string inputFilePath)
{
    public int Part1()
    {
        var lines = File.ReadAllLines(inputFilePath);

        // orderby to try to match the longest pattern first (not sure if there's any point to this tbh)
        var patterns = lines[0].Split(", ").OrderByDescending(p => p.Length).ToList();

        // var answer = 0;
        // foreach (var design in lines.Skip(2))
        // {
        //     var possible = IsDesignPossible(patterns, design);
        //     Console.WriteLine($"Design {design}, is possible: {possible}");
        //     if (possible)
        //         ++answer;
        // }

        var answer = lines.Skip(2).Count(l => IsDesignPossible(patterns, l));
        Console.WriteLine(answer);
        return answer;
    }

    private static bool IsDesignPossible(IReadOnlyCollection<string> patterns, string design)
    {
        var workingCopy = $"{design}";
        for (;;)
        {
            var startsWithPattern = patterns.FirstOrDefault(p => workingCopy.StartsWith(p));
            if (startsWithPattern == null)
                return false;

            workingCopy = workingCopy.Remove(0, startsWithPattern.Length);
            if (workingCopy.Length == 0)
                return true;
        }
    }
}