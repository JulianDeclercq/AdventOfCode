using AdventOfCode2024;
using Xunit;

namespace AdventOfCode2024Tests;

public class Day17Tests
{
    [Fact]
    public void Part1_Example()
    {
        var day = new Day17("input/example/day17e.txt");
        var part1 = day.Part1();
        Assert.Equal("4,6,3,5,6,3,5,2,1,0", part1);
    }
    
    [Fact]
    public void Part1_Real()
    {
        var day = new Day17("input/real/day17.txt");
        var part1 = day.Part1();
        Assert.Equal("1,3,5,1,7,2,5,1,6", part1);
    }

    [Fact]
    public void Part2_Example()
    {
        var day = new Day17("input/example/day17e2.txt");
        var part2 = day.Part2();
        Assert.Equal(117440, part2);
    }
}