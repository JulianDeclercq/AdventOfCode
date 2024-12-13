using System.Text.RegularExpressions;
using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day13
{
    private class ClawMachine
    {
        public Point AOffset { get; init; }
        public Point BOffset { get; init; }
        public Point Prize { get; init; }

        public override string ToString()
        {
            return $"A {AOffset}, B {BOffset}, Prize {Prize}";
        }
    }
    
    public static void Solve()
    {
        var clawMachines = ParseInput("day13e.txt");
        
        foreach (var machine in clawMachines)
            Console.WriteLine(machine);
    }

    private static List<ClawMachine> ParseInput(string fileName)
    {
        var lines = File.ReadAllLines($"input/{fileName}");
        var offset = 0;
        var regex = new RegexHelper(new Regex(@".+: X.(\d+), Y.(\d+)"), "x", "y");
        List<ClawMachine> clawMachines = [];
        
        while (offset < lines.Length)
        {
            var clawDescription = lines.Skip(offset).TakeWhile(l => !string.IsNullOrWhiteSpace(l)).ToList();
            regex.Match(clawDescription[0]);
            var aOffset = new Point(regex.GetInt("x"), regex.GetInt("y"));
            
            regex.Match(clawDescription[1]);
            var bOffset = new Point(regex.GetInt("x"), regex.GetInt("y"));
            
            regex.Match(clawDescription[2]);
            var prize = new Point(regex.GetInt("x"), regex.GetInt("y"));
            var clawMachine = new ClawMachine
            {
                AOffset = aOffset,
                BOffset = bOffset,
                Prize = prize
            };
            clawMachines.Add(clawMachine);
            offset += clawDescription.Count + 1;
        }
        
        return clawMachines;
    }
}