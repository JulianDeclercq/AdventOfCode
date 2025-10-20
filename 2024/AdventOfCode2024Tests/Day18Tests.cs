using AdventOfCode2024;
using Xunit;

namespace AdventOfCode2024Tests;

public class Day18Tests
{
    [Fact]
    public void Part1_Example()
    {
        var day = new Day18("input/example/day18e.txt");
        var result = day.Part1();
        Assert.Equal(22, result);
    }
    
    [Fact]
    public void Part1_Real()
    {
        var day = new Day18("input/real/day18.txt");
        var result = day.Part1();
        Assert.Equal(288, result);
    }

    [Fact]
    public void Part2_Example()
    {
        var day = new Day18("input/example/day18e.txt");
        var result = day.Part2();
        Assert.Equal("6,1", result);
    }

    [Fact]
    public void Part2_Real()
    {
        var day = new Day18("input/real/day18.txt");
        var result = day.Part2();
        Assert.Equal("52,5", result);
    }
}
