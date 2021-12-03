using System.Text;

namespace AdventOfCode2021.days;

public class Day3
{
    private string[] _lines = Array.Empty<string>();
    private string[] Lines
    {
        get
        {
            if (_lines.Length == 0)
                _lines = File.ReadAllLines(@"..\..\..\input\day3.txt");
            
            return _lines;
        }
    }
       
    public void Part1()
    {
        var length = Lines[0].Length;
        var bGamma = new StringBuilder(Lines[0]);
        var bEpsilon = new StringBuilder(Lines[0]);

        for (var i = 0; i < length; ++i)
        {
            var ctr = 0;
            foreach (var line in Lines)
                ctr += (line[i] == '1') ? 1 : -1;

            bGamma[i] = ctr > 0 ? '1' : '0'; // if ctr is positive, '1' was more common than '0'
            bEpsilon[i] = ctr < 0 ? '1' : '0'; // if ctr is negative, '1' was less common than '0'
        }

        var gamma = Convert.ToInt16(bGamma.ToString(), 2);
        var epsilon = Convert.ToInt16(bEpsilon.ToString(), 2);
        
        Console.WriteLine($"Day 3 part 1: {gamma * epsilon}");
    }
}