using System.Text.RegularExpressions;

namespace AdventOfCode2019.days;

public class Day4
{
    private const string input = "165432-707912";
    public void Part1()
    {
        //Console.WriteLine(Enumerable.Range(165432, 707912 - 165431).Count(Valid));

        var test = Valid3(112233);
        var test2 = Valid3(123444);
        var test3 = Valid3(111122);

        //Console.WriteLine(Enumerable.Range(165432, 707912 - 165431).Count(Valid2));
        int ctr = 0;
        foreach (var haha in Enumerable.Range(165432, 707912 - 165431))
        {
            if (Valid3(haha))
            {
                //Console.WriteLine($"valid: {haha}");
                Console.WriteLine(haha);
                ctr++;
            }
        }
        Console.WriteLine(ctr);
    }

    private bool Valid(int nr)
    {
        var str = nr.ToString();
        var hasAdjacent = false;
        for (var i = 0; i < str.Length - 1; ++i)
        {
            if (str[i] == str[i + 1])
            {
                hasAdjacent = true;
                break;
            }
        }

        if (!hasAdjacent)
            return false;

        var numbers = str.Select(x => int.Parse($"{x}")).ToArray();
        var ordered = numbers.Order().ToArray();
        return numbers.SequenceEqual(ordered);
    }
    
    private bool Valid2(int nr)
    {
        var str = nr.ToString();

        var equalParts = new List<string>();
        var builder = $"{str[0]}";
        var previous = str[0];
        for (var i = 1; i < str.Length; ++i)
        {
            var current = str[i];
            if (i == str.Length - 1) // handle last
            {
                if (previous == current)
                {
                    builder += current;
                    equalParts.Add(builder);    
                }
                break;
            }
            
            if (current != previous)
            {
                equalParts.Add(builder);
                builder = "";
            }
            builder += current;
            previous = current;
        }
        
        if (equalParts.All(ep => ep.Length != 2))
            return false;

        var numbers = str.Select(x => int.Parse($"{x}")).ToArray();
        var ordered = numbers.Order().ToArray();
        return numbers.SequenceEqual(ordered);
    }
    
    private bool Valid3(int nr)
    {
        var str = nr.ToString();

        var reg = new Regex("(.)\\1+");
        if (reg.Matches(str).All(ep => ep.Length != 2))
            return false;

        var numbers = str.Select(x => int.Parse($"{x}")).ToArray();
        var ordered = numbers.Order().ToArray();
        return numbers.SequenceEqual(ordered);
    }
}