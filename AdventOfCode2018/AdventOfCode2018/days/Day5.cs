using System.Text;

namespace AdventOfCode2018.days;

public class Day5
{
    public void Part1()
    {
        var input = File.ReadAllLines(@"..\..\..\input\day5.txt").Single();
        var previous = "";
        while (input != previous)
        {
            previous = input;
            input = RemovePolarities(input);
        }
        
        Console.WriteLine($"Day 5 part 1: {input.Length}");
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
        
        // if a is upper
        if (char.IsLower(b))
        {
            if (char.ToLower(a) == b)
                return true;
        }

        return false;
    }
}