using System.Text.RegularExpressions;

namespace AdventOfCode2021.days;

public class Day17
{
    private readonly RegexHelper _regexHelper =
        new(new Regex(@"target area: x=(\d+)..(\d+), y=(-?\d+)..(-?\d+)"), "xMin", "xMax", "yMin", "yMax");
    
    private const string testInput = "target area: x=20..30, y=-10..-5";
    private const string input = "target area: x=257..286, y=-101..-57";
    public void Part1()
    {
        Helpers.Verbose = false;
        
        if (!_regexHelper.Match(input))
            throw new Exception($"Couldn't parse {input}");

        (int min, int max) xBounds = new(_regexHelper.GetInt("xMin"), _regexHelper.GetInt("xMax"));
        (int min, int max) yBounds = new(_regexHelper.GetInt("yMin"), _regexHelper.GetInt("yMax"));

        var highestY = int.MinValue;
        var initialVelocityWithHighestY = new Point(0, 0);
        const int maxVelocity = 1000;
        for (var i = 0; i < maxVelocity; ++i)
        {
            for (var j = 0; j < maxVelocity; ++j)
            {
                var highestYReached = int.MinValue;
                var position = new Point(0, 0);
                var velocity = new Point(i, j);
                
                while (position.X <= xBounds.max && position.Y >= yBounds.max) // not overshot
                {
                    position += velocity;
                    velocity = ApplyPhysics(velocity);
                    
                    if (position.Y > highestYReached)
                        highestYReached = position.Y;
                    
                    if (position.InArea(xBounds, yBounds))
                    {
                        var validInitialVelocity = new Point(i, j);
                        if (highestYReached > highestY)
                        {
                            highestY = highestYReached;
                            initialVelocityWithHighestY = validInitialVelocity;
                        }
                        Helpers.WriteLine($"Valid initial velocity found: {validInitialVelocity}", true);
                        break;
                    }
                }
            }
        }
        Helpers.WriteLine($"Day 17 part 1: {initialVelocityWithHighestY} {highestY}");
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
    
    public void Part2()
    {
        
    }
}