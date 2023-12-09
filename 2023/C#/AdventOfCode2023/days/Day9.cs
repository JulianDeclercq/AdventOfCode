namespace AdventOfCode2023.days;

public class Day9
{
    public void Part1()
    {
        var input = File.ReadAllLines("../../../input/Day9.txt");
        var answer = 0;
        foreach (var set in input)
        {
            var diffs = set.Split(' ').Select(int.Parse).ToList();
            var saved = new List<List<int>> { diffs };

            while (diffs.Any(d => !d.Equals(0)))
            {
                diffs = CalcDiffs(diffs);
                saved.Add(diffs);
            }

            saved.Reverse(); // bottom up

            for (var i = 0; i < saved.Count - 1; ++i)
                saved[i + 1].Add(saved[i + 1].Last() + saved[i].Last());

            answer += saved.Last().Last();
        }
        Console.WriteLine(answer);
    }

    private static List<int> CalcDiffs(List<int> input)
    {
        var diffs = new List<int>();
        for (var i = 0; i < input.Count - 1; ++i)
            diffs.Add(input[i + 1] - input[i]);

        return diffs;
    }
}