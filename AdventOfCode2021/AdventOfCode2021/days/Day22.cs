using System.Text.RegularExpressions;

namespace AdventOfCode2021.days;

public class Day22
{
    private static readonly Regex CommandPattern =
        new Regex(@"(on|off) x=(-?\d+)..(-?\d+),y=(-?\d+)..(-?\d+),z=(-?\d+)..(-?\d+)"); 
    
    public class Command
    {
        public bool TargetState { get; init; }
        public Point XRange { get; init; }
        public Point YRange { get; init; }
        public Point ZRange { get; init; }
        public bool ExceedsLimits = false;

        public override string ToString() =>
            $"On: {TargetState}, XRange: {XRange}, YRange {YRange}, ZRange {ZRange}, ExceedsLimits: {ExceedsLimits}";
    }

    private Dictionary<(int x, int y, int z), bool> CubeStates = new();

    public void Part1()
    {
        var lines = File.ReadAllLines(@"..\..\..\input\day22.txt");
        var commands = lines.Select(ParseCommand).Where(c => !c.ExceedsLimits);

        foreach (var command in commands)
            ExecuteCommand(command);
        
        Console.WriteLine($"Day 22 part 1: {CubeStates.Count(c => c.Value)}");
    }

    private static Command ParseCommand(string line)
    {
        var groups = CommandPattern.Match(line).Groups;

        int xMin = int.Parse(groups[2].Value), xMax = int.Parse(groups[3].Value);
        int yMin = int.Parse(groups[4].Value), yMax = int.Parse(groups[5].Value);
        int zMin = int.Parse(groups[6].Value), zMax = int.Parse(groups[7].Value);
        
        return new Command()
        {
            TargetState = groups[1].ToString().Equals("on"),
            XRange = new Point(xMin, xMax),
            YRange = new Point(yMin, yMax),
            ZRange = new Point(zMin, zMax),
            ExceedsLimits = new List<int>(){xMin, xMax, yMin, yMax, zMax, zMax}
                .Any(x => !Helpers.InRangeInclusive(-50, 50, x))
        };
    }

    private void ExecuteCommand(Command command)
    {
        for (var x = command.XRange.X; x <= command.XRange.Y; ++x)
        {
            for (var y = command.YRange.X; y <= command.YRange.Y; ++y)
            {
                for (var z = command.ZRange.X; z <= command.ZRange.Y; ++z)
                {
                    CubeStates[(x, y, z)] = command.TargetState; 
                }
            }
        }
    }

}