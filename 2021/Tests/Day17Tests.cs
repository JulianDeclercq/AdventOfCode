using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2021;
using AdventOfCode2021.days;
using Xunit;
using Xunit.Abstractions;

namespace Tests;

public class Day17Tests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Day17Tests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private const string ExampleInput = "target area: x=20..30, y=-10..-5";
    [Theory]
    //[InlineData(23, -10)]
    [InlineData(6, 0)]
    private void Initial_velocity_should_be_valid(int xVel, int yVel)
    {
        _testOutputHelper.WriteLine("test");
        var day = new Day17();
        day.ParseInput(ExampleInput);
        Assert.True(day.IsValidInitialVelocity(new Point(xVel, yVel), out var h));
    }

    [Fact]
    private void Verify_valid_answer_for_example_part2()
    {
        var expected = new List<Point>()
        {
            new (23,-10), new (25,-9), new (27,-5), new (29,-6),  new (22,-6),   new (21,-7),   new (9,0),     new (27,-7),   new (24,-5),
            new (25,-7),  new (26,-6), new (25,-5), new (6,8),    new (11,-2),   new (20,-5),   new (29,-10),  new (6,3),     new (28,-7),
            new (8,0),    new (30,-6), new (29,-8), new (20,-10), new (6,7),     new (6,4),     new (6,1),     new (14,-4),   new (21,-6),
            new (26,-10), new (7,-1),  new (7,7),   new (8,-1),   new (21,-9),   new (6,2),     new (20,-7),   new (30,-10),  new (14,-3),
            new (20,-8),  new (13,-2), new (7,3),   new (28,-8),  new (29,-9),   new (15,-3),   new (22,-5),   new (26,-8),   new (25,-8),
            new (25,-6),  new (15,-4), new (9,-2),  new (15,-2),  new (12,-2),   new (28,-9),   new (12,-3),   new (24,-6),   new (23,-7),
            new (25,-10), new (7,8),   new (11,-3), new (26,-7),  new (7,1),     new (23,-9),   new (6,0),     new (22,-10),  new (27,-6),
            new (8,1),    new (22,-8), new (13,-4), new (7,6),    new (28,-6),   new (11,-4),   new (12,-4),   new (26,-9),   new (7,4),
            new (24,-10), new (23,-8), new (30,-8), new (7,0),    new (9,-1),    new (10,-1),   new (26,-5),   new (22,-9),   new (6,5),
            new (7,5),    new (23,-6), new (28,-10),new (10,-2),  new (11,-1),   new (20,-9),   new (14,-2),   new (29,-7),   new (13,-3),
            new (23,-5),  new (24,-8), new (27,-9), new (30,-7),  new (28,-5),   new (21,-10),  new (7,9),     new (6,6),     new (21,-5),
            new (27,-10), new (7,2),   new (30,-9), new (21,-8),  new (22,-7),   new (24,-9),   new (20,-6),   new (6,9),     new (29,-5),
            new (8,-2),   new (27,-8), new (30,-5), new (24,-7),
        };

        var day = new Day17();
        day.ParseInput(ExampleInput);
        var plausibleVelocities = day.GeneratePlausibleVelocities();
        var valid = new List<Point>();
        foreach (var v in plausibleVelocities)
        {
            if (day.IsValidInitialVelocity(v, out var h))
                valid.Add(v);
        }

        var test = expected.Except(valid);
        var test2 = valid.Except(expected);
        Assert.Empty(test);
        Assert.Empty(test2);
    }
}