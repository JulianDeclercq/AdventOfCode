namespace AdventOfCode2021.days;

public class Day1
{
    private int[] _depths = Array.Empty<int>();
    private int[] Depths
    {
        get
        {
            if (_depths.Length == 0)
                _depths = File.ReadAllLines(@"..\..\..\input\day1.txt").Select(int.Parse).ToArray();

            return _depths;
        }
    }

    public void Part1()
    {
        var ctr = 0;
        for (var i = 0; i < Depths.Length - 1; ++i)
        {
            if (Depths[i + 1] > Depths[i])
                ctr++;
        }
        Console.WriteLine($"Day 1 part 1: {ctr}");
    }

    public void Part2()
    {
        var windows = new List<int>();
        for (var i = 0; i < Depths.Length - 2; ++i)
            windows.Add(Depths[i] + Depths[i+1] + Depths[i+2]);

        var ctr = 0;
        for (var i = 0; i < windows.Count - 1; ++i)
        {
            if (windows[i + 1] > windows[i])
                ctr++;
        }
        Console.WriteLine($"Day 1 part 2: {ctr}");
    }
}