using System.Text.RegularExpressions;

namespace AdventOfCode2021.days;

public class Day22
{
    private static readonly Regex CommandPattern =
        new Regex(@"(on|off) x=(-?\d+)..(-?\d+),y=(-?\d+)..(-?\d+),z=(-?\d+)..(-?\d+)"); 
    
    public class Command
    {
        public bool On { get; init; }
        public Point XRange { get; init; }
        public Point YRange { get; init; }
        public Point ZRange { get; init; }
        public bool ExceedsLimits = false;

        public override string ToString() =>
            $"On: {On}, XRange: {XRange}, YRange {YRange}, ZRange {ZRange}, ExceedsLimits: {ExceedsLimits}";
    }
    
    public void Part1()
    {
        var lines = File.ReadAllLines(@"..\..\..\input\day22.txt");
        var commands = lines.Select(ParseCommand).Where(c => !c.ExceedsLimits);


        var dim = 101; // -50 to 50 + 0 itself
        // TODO: calculate width and height based on the origin and endpoint (endpoint is the opposite of origin, the absolute max x, y point)
        var grid3D = new NegativeGrid3D<char>(dim, dim, dim, -50, -50, -50, '.');
        foreach (var command in commands)
        {
            ExecuteCommand(grid3D, command);
        }
        
        Console.WriteLine($"Day 22 part 1: {grid3D.All().Count(c => c == '#')}");
    }

    private static Command ParseCommand(string line)
    {
        var groups = CommandPattern.Match(line).Groups;

        var xMin = int.Parse(groups[2].Value);
        var xMax = int.Parse(groups[3].Value);
        var yMin = int.Parse(groups[4].Value);
        var yMax = int.Parse(groups[5].Value);
        var zMin = int.Parse(groups[6].Value);
        var zMax = int.Parse(groups[7].Value);
        
        return new Command()
        {
            On = groups[1].ToString().Equals("on"),
            XRange = new Point(xMin, xMax),
            YRange = new Point(yMin, yMax),
            ZRange = new Point(zMin, zMax),
            ExceedsLimits = new List<int>(){xMin, xMax, yMin, yMax, zMax, zMax}
                .Any(x => !Helpers.InRangeInclusive(-50, 50, x))
        };
    }

    private static void ExecuteCommand(NegativeGrid3D<char> grid, Command command)
    {
        for (var x = command.XRange.X; x <= command.XRange.Y; ++x)
        {
            for (var y = command.YRange.X; y <= command.YRange.Y; ++y)
            {
                for (var z = command.ZRange.X; z <= command.ZRange.Y; ++z)
                {
                    grid.Set(x, y, z, command.On ? '#' : '.');
                    //Console.WriteLine($"Setting ({x}, {y}, {z}) to {command.On}");
                }
            }
        }
    }

}