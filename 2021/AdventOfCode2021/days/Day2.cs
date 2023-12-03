using System.Text.RegularExpressions;

namespace AdventOfCode2021.days;

public class Day2
{
    public static void Solve()
    {
        int horizontal = 0, depth = 0;
        int horizontal2 = 0, depth2 = 0, aim = 0;

        var commands = File.ReadAllLines(@"..\..\..\input\day2.txt");
        foreach (var command in commands)
        {
            var r = new Regex(@"(\w+) (\d+)");
            var match = r.Match(command);
            if (!match.Success)
            {
                Console.WriteLine($"Invalid command {command}");
                continue;
            }

            var cmd = match.Groups[1].ToString();
            var value = int.Parse(match.Groups[2].ToString());
            ExecuteCommandPart1(cmd, value, ref horizontal, ref depth);
            ExecuteCommandPart2(cmd, value, ref horizontal2, ref depth2, ref aim);
        }
        Console.WriteLine($"Day 2 part 1: {depth * horizontal}");
        Console.WriteLine($"Day 2 part 2: {depth2 * horizontal2}");
    }
    
    private static void ExecuteCommandPart1(string command, int value, ref int horizontal, ref int depth)
    {
        switch (command[0])
        {
            // forward
            case 'f':
                horizontal += value;
                break;
                
            // down
            case 'd':
                depth += value;
                break;
                
            // up
            case 'u':
                depth -= value;
                break;
        }
    }
    
    private static void ExecuteCommandPart2(string command, int value, ref int horizontal, ref int depth, ref int aim)
    {
        switch (command[0])
        {
            // forward
            case 'f':
                horizontal += value;
                depth += aim * value;
                break;
                
            // down
            case 'd':
                aim += value;
                break;
                
            // up
            case 'u':
                aim -= value;
                break;
        }
    }
}