using AdventOfCode2024;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2024Tests;

public class Day17Tests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Day17Tests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

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

    [Fact]
    public void Part2_Sandbox()
    {
        var day = new Day17("input/real/day17.txt");
        for (var i = 0; i < 7; ++i)
        {
            var sandbox = day.TestRegisterA(i);
            _testOutputHelper.WriteLine($"Reg A with 3bit {i} outputs: {sandbox}");
        }
        // Program: 2,4,1,3,7,5,4,7,0,3,1,5,5,5,3,0
    }

    [Fact]
    public void Test()
    {
        var day = new Day17("input/example/day17e2.txt");
        day.Test();
    }
}