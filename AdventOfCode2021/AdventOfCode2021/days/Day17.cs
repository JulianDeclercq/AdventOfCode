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
        Helpers.Verbose = false;
        ParseInput("target area: x=257..286, y=-101..-57");
        
        var totalHighestY = int.MinValue;
        var initialVelocityWithHighestY = new Point(0, 0);
        const int maxVelocity = 1000;
        for (var i = 0; i < maxVelocity; ++i)
        {
            for (var j = 0; j < maxVelocity * 2; ++j)
            {
                var velocity = new Point(i, -maxVelocity + j);
                if (!IsValidInitialVelocity(velocity, out var highest)) 
                    continue;

                if (highest <= totalHighestY) 
                    continue;
                
                totalHighestY = highest;
                initialVelocityWithHighestY = velocity;
            }
        }
        Helpers.WriteLine($"Day 17 part 1: {totalHighestY} ({initialVelocityWithHighestY})");
    }
    
    public void Part2()
    {
        Helpers.Verbose = false;
        ParseInput("target area: x=257..286, y=-101..-57");

        var totalValid = 0;
        const int maxVelocity = 1000;
        for (var i = 0; i < maxVelocity; ++i)
        {
            for (var j = 0; j < maxVelocity * 2; ++j)
            {
                var velocity = new Point(i, -maxVelocity + j);
                if (IsValidInitialVelocity(velocity, out var highest))
                    totalValid++;
            }
        }
        Helpers.WriteLine($"Day 17 part 2: {totalValid}");
    }
    
    public bool IsValidInitialVelocity(Point velocity, out int highestY)
    {
        var position = new Point(0, 0);
        highestY = int.MinValue;

        while (position.X <= _xBounds.max && position.Y >= _yBounds.min) // stop after overshooting
        {
            position += velocity;
            velocity = ApplyPhysics(velocity);
            
            Helpers.WriteLine($"Position: {position}, velocity: {velocity}", true);

            highestY = Math.Max(highestY, position.Y);

            if (!position.InArea(_xBounds, _yBounds))
                continue;
            
            Helpers.WriteLine($"Valid initial velocity found: {velocity}", true);
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