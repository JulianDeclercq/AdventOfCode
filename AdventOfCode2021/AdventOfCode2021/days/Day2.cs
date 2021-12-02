using System.Text.RegularExpressions;

namespace AdventOfCode2021.days;

public class Day2
{
    private string[] _commands = Array.Empty<string>();
    private IEnumerable<string> Commands
    {
        get
        {
            if (_commands.Length == 0)
                _commands = File.ReadAllLines(@"..\..\..\input\day2.txt");

            return _commands;
        }
    }

    public void Part1()
    {
        int horizontal = 0, depth = 0;
        foreach (var command in Commands)
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
            switch (cmd[0])
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
        Console.WriteLine($"Day 2 part 1: {depth * horizontal}");
    }
}