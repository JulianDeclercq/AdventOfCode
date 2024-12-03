using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public class Day3
{
    public static void Solve()
    {
        var lines = File.ReadAllLines("input/day3.txt");
        var input = string.Join("", lines);
        Console.WriteLine(ProcessValidMultipliers(input)); // part 1

        var active = true;
        var part2 = 0;
        var current = input;
        for (;;)
        {
            var doIdx = current.IndexOf("do", StringComparison.Ordinal);
            var dontIdx = current.IndexOf("don't", StringComparison.Ordinal);

            if (doIdx == -1 && dontIdx == -1)
            {
                // process end
                if (active)
                    part2 += ProcessValidMultipliers(current);

                break;
            }

            // if doidx is equal to dontidx then it's a don't (since do is a substring of don't)
            if (doIdx == dontIdx || doIdx == -1 || (dontIdx != -1 && dontIdx < doIdx))
            {
                if (active)
                {
                    var process = current[..dontIdx];
                    part2 += ProcessValidMultipliers(process);
                }
                
                // update
                active = false;
                current = current[(dontIdx + "don't".Length)..];
                continue;
            }

            if (dontIdx == -1 || (doIdx != -1 && doIdx < dontIdx))
            {
                if (active)
                {
                    var process = current[..doIdx];
                    part2 += ProcessValidMultipliers(process);
                }
                
                // update
                active = true;
                current = current[(doIdx + "do".Length)..];
            }
        }
        Console.WriteLine(part2);
    }

    private static int ProcessValidMultipliers(string input)
    {
        var regex = new Regex(@"mul\((\d+),(\d+)\)");
        return regex.Matches(input).Aggregate(0, (total, next) =>
            total + int.Parse(next.Groups[1].Value) * int.Parse(next.Groups[2].Value));
    }
}