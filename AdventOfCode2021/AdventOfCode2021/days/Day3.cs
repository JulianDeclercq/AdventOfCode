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

    public char MostCommonBit(IEnumerable<string> entries, int index)
    {
        var ctr = entries.Sum(line => (line[index] == '1') ? 1 : -1);
        return ctr >= 0 ? '1' : '0'; // if ctr is positive, '1' was more common than '0'
    }
    
    public char LeastCommonBit(IEnumerable<string> entries, int index)
    {
        var ctr = entries.Sum(line => (line[index] == '1') ? 1 : -1);
        return ctr < 0 ? '1' : '0'; // if ctr is negative, '1' was less common than '0'
    }
    
    public void Part2()
    {
        var length = Lines[0].Length;

        // oxygen generator rating is tied to most common bit (=> gamma)
        var possibilities = Lines.ToList();
        for (var i = 0; i < length; ++i)
        {
            var idx = i;
            var mostCommonBit = MostCommonBit(possibilities, i);
            possibilities = possibilities.Where(x => x[idx] == mostCommonBit).ToList();
            if (possibilities.Count == 1)
                break;
        }

        var bOGR = possibilities[0];
        
        // CO2 scrubber rating is tied to least common bit (=> epsilon)
        possibilities = Lines.ToList();
        for (var i = 0; i < length; ++i)
        {
            var idx = i;
            var leastCommonBit = LeastCommonBit(possibilities, i);
            possibilities = possibilities.Where(x => x[idx] == leastCommonBit).ToList();
            if (possibilities.Count == 1)
                break;
        }

        var bCO2SR = possibilities[0];
        var OGR = Convert.ToInt16(bOGR, 2);
        var CO2SR = Convert.ToInt16(bCO2SR, 2);
        
        Console.WriteLine($"Day 3 part 2: {OGR * CO2SR}");
    }
}