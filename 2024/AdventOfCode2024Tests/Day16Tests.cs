using AdventOfCode2024;
using Xunit;

namespace AdventOfCode2024Tests;

public class Day16Tests
{
    [Fact]
    public void Part1_Example()
    {
        var day = new Day16("input/example/day16e.txt");
        var part1 = day.Solve(part2: false);
        Assert.Equal(7036, part1);
    }
    
    [Fact]
    public void Part1_Example2()
    {
        var day = new Day16("input/example/day16e2.txt");
        var part1 = day.Solve(part2: false);
        Assert.Equal(11048, part1);
    }
    
    [Fact]
    public void Part2_Example()
    {
        var day = new Day16("input/example/day16e.txt");
        var part2 = day.Solve(part2: true);
        Assert.Equal(45, part2);
    }
    
    [Fact]
    public void Part2_Example2()
    {
        var day = new Day16("input/example/day16e2.txt");
        var part2 = day.Solve(part2: true);
        Assert.Equal(64, part2);
    }
}