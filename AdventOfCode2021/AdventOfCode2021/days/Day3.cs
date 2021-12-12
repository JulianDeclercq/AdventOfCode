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
    
    private static char MostCommonBit(IEnumerable<string> entries, int index)
    {
        var ctr = entries.Sum(line => (line[index] == '1') ? 1 : -1);
        return ctr >= 0 ? '1' : '0'; // if ctr is positive, '1' was more common than '0'
    }

    private static char LeastCommonBit(IEnumerable<string> entries, int index)
    {
        var ctr = entries.Sum(line => (line[index] == '1') ? 1 : -1);
        return ctr < 0 ? '1' : '0'; // if ctr is negative, '1' was less common than '0'
    }
    
    private int FindRating(Func<IEnumerable<string>, int, char> criteria)
    {
        var possibilities = Lines.ToList();
        var length = Lines[0].Length;
        for (var i = 0; i < length; ++i)
        {
            // calculate the possibilities based on the criteria
            possibilities = possibilities.Where(x => x[i] == criteria(possibilities, i)).ToList();
            
            // if there is only a single possibility left, this is the answer
            if (possibilities.Count == 1)
                return Convert.ToInt16(possibilities[0], 2);
        }

        // failed to find a rating
        return int.MinValue;
    }
       
    public void Part1()
    {
        var length = Lines[0].Length;
        var bGamma = new StringBuilder(Lines[0]);
        var bEpsilon = new StringBuilder(Lines[0]);

        for (var i = 0; i < length; ++i)
        {
            bGamma[i] = MostCommonBit(Lines, i);
            bEpsilon[i] = LeastCommonBit(Lines, i);
        }

        var gamma = Convert.ToInt16(bGamma.ToString(), 2);
        var epsilon = Convert.ToInt16(bEpsilon.ToString(), 2);
        
        Console.WriteLine($"Day 3 part 1: {gamma * epsilon}");
    }

    public void Part2()
    {
        var ogr = FindRating(MostCommonBit);
        var co2Sr = FindRating(LeastCommonBit);
        Console.WriteLine($"Day 3 part 2: {ogr * co2Sr}");
    }
}