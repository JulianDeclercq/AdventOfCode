namespace AdventOfCode2021.days;

public class Day1
{
    private string[] _lines = Array.Empty<string>();
    private string[] Lines
    {
        get
        {
            if (_lines.Length == 0)
            {
                //_lines = File.ReadAllLines(@"..\..\..\input\day1_example.txt");
                _lines = File.ReadAllLines(@"..\..\..\input\day1.txt");
            }

            return _lines;
        }
    }

    public void Part1()
    {
        var ctr = 0;
        for (var i = 0; i < Lines.Length - 1; ++i)
        {
            if (int.Parse(Lines[i + 1]) > int.Parse(Lines[i]))
                ctr++;
        }
        Console.WriteLine($"Day 1 part 1: {ctr}");
    }

    public void Part2()
    {
        var windows = new List<int>();
        for (var i = 0; i < Lines.Length - 2; ++i)
            windows.Add(int.Parse(Lines[i]) + int.Parse(Lines[i+1]) + int.Parse(Lines[i+2]));

        var ctr = 0;
        for (var i = 0; i < windows.Count - 1; ++i)
        {
            if (windows[i + 1] > windows[i])
                ctr++;
        }
        Console.WriteLine($"Day 1 part 2: {ctr}");
    }

}