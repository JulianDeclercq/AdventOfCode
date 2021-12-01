namespace AdventOfCode2021.days;

public class Day1
{
    private string[] _lines = Array.Empty<string>();
    private string[] Lines
    {
        get
        {
            if (_lines.Length == 0)
                _lines = File.ReadAllLines(@"..\..\..\input\day1.txt");

            return _lines;
        }
    }

    public void Part1()
    {
        int last = int.Parse(Lines[0]), ctr = 0;
            foreach (var line in Lines)
        {
            var lineValue = int.Parse(line); 
            if (lineValue > last)
                ctr++;

            last = lineValue;
        }
        Console.WriteLine($"Day 1 part 1: {ctr}");
    }

}