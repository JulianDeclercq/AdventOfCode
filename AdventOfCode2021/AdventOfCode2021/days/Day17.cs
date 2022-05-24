using System.Text.RegularExpressions;

namespace AdventOfCode2021.days;

public class Day17
{
    private readonly RegexHelper _regexHelper =
        new(new Regex(@"target area: x=(\d+)..(\d+), y=(-?\d+)..(-?\d+)"), "xMin", "xMax", "yMin", "yMax");
    
    private (int min, int max) _xBounds;
    private (int min, int max) _yBounds;

    public void ParseInput(string input)
    {
        if (!_regexHelper.Parse(input))
            throw new Exception($"Couldn't parse {input}");
        
        _xBounds = new(_regexHelper.GetInt("xMin"), _regexHelper.GetInt("xMax"));
        _yBounds = new(_regexHelper.GetInt("yMin"), _regexHelper.GetInt("yMax"));
    }

    public void Part1()
    {
        ParseInput("target area: x=257..286, y=-101..-57");

        (int Y, Point Velocity) highestY = new (int.MinValue, new Point(0, 0));
        var plausibleVelocities = GeneratePlausibleVelocities();
        foreach (var v in plausibleVelocities)
        {
            if (!IsValidInitialVelocity(v, out var highest))
                continue;
            
            if (highest > highestY.Y)
                highestY = new(highest, v);
        }
        Helpers.WriteLine($"Day 17 part 1: {highestY.Y} ({highestY.Velocity})");
    }
    
    public void Part2()
    {
        ParseInput("target area: x=257..286, y=-101..-57");
        Helpers.WriteLine(
            $"Day 17 part 2: {GeneratePlausibleVelocities().Count(v => IsValidInitialVelocity(v, out var h))}");
    }

    // Generates all velocities that have a chance of being valid
    public IEnumerable<Point> GeneratePlausibleVelocities()
    {
        var result = new List<Point>();
        (int x, int y) maxVelocity = new(_xBounds.max + 1, Math.Abs(_yBounds.min) + 1);
        for (var i = 0; i < maxVelocity.x; ++i)
        {
            // J ranges from -maxVelocity.y to maxVelocity.y
            for (var j = 0; j < maxVelocity.y * 2; ++j)
            {
               result.Add(new Point(i, j - maxVelocity.y));
            }
        }
        return result;
    }

    public bool IsValidInitialVelocity(Point velocity, out int highestY)
    {
        var position = new Point(0, 0);
        highestY = int.MinValue;

        // stop after overshooting
        while (position.X <= _xBounds.max && position.Y >= _yBounds.min)
        {
            position += velocity;
            velocity = ApplyPhysics(velocity);
            highestY = Math.Max(highestY, position.Y);

            if (position.InArea(_xBounds, _yBounds))
                return true;
        }
        return false;
    }
    
    private static Point ApplyPhysics(Point velocity)
    {
        var x = velocity.X switch
        {
            < 0 => velocity.X + 1,
            > 0 => velocity.X - 1,
            _ => 0
        };
        return new Point(x, velocity.Y - 1);
    }
    
}