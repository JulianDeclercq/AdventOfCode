using System.Text;

namespace AdventOfCode2018.days;

public class Day5
{
    public void Part1()
    {
        var input = File.ReadAllLines(@"..\..\..\input\day5.txt").Single();
        var answer = FullyReact(input);
        Console.WriteLine($"Day 5 part 1: {answer.Length}");
    }
    
    public void Part2()
    {
        var input = File.ReadAllLines(@"..\..\..\input\day5.txt").Single();
        var units = input.ToLower().ToHashSet();

        var inputs = new List<string>();
        foreach (var unit in units)
            inputs.Add(input.Where(c => unit != char.ToLower(c)).Str());

        var answer = inputs.Select(FullyReact).MinBy(s => s.Length);
        
        Console.WriteLine($"Day 5 part 2: {answer!.Length}");
    }

    private static string FullyReact(string input)
    {
        var previous = "";
        while (input != previous)
        {
            previous = input;
            input = RemovePolarities(input);
        }

        return input;
    }

    private static string RemovePolarities(string input)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < input.Length; i++)
        {
            if (i == input.Length - 1)
            {
                sb.Append(input[i]);
                break;
            }
            
            // if the current one does not react with the next one they can stay
            if (!React(input[i], input[i + 1]))
            {
                sb.Append(input[i]);
                continue;
            }

            i++;
        }

        return sb.ToString();
    }

    private static bool React(char a, char b)
    {
        if (char.IsLower(a))
        {
            if (char.IsLower(b))
                return false;

            if (char.ToLower(b) == a)
                return true;
        }
        else // if a is upper
        {
            if (char.IsLower(b))
            {
                if (char.ToLower(a) == b)
                    return true;
            }    
        }
        return false;
    }
}