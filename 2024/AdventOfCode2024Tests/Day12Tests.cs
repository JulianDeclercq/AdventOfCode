using AdventOfCode2024;
using Xunit;

namespace AdventOfCode2024Tests;

public static class Day12Tests
{
    [Fact]
    private static void Part2ExampleAbcd()
    {
        // Using the new method of calculating the per-region price by multiplying the region's area by
        // its number of sides, regions A through E have prices 16, 16, 32, 4, and 12, respectively, for a total price of 80.
        
        var day12 = new Day12("input/day12e.txt");
        Assert.Equal(80, day12.SolvePart(2));
    }

    [Fact]
    private static void Part2ExampleXoxo()
    {
        var day12 = new Day12("input/day12e2.txt");
        Assert.Equal(436, day12.SolvePart(2));
    }
    
    [Fact]
    private static void Part2ExampleEShapedRegion()
    {
        var day12 = new Day12("input/day12e4.txt");
        Assert.Equal(236, day12.SolvePart(2));
    }
    
    [Fact]
    private static void Part2ExampleInsideOutside()
    {
        var day12 = new Day12("input/day12e5.txt");
        Assert.Equal(368, day12.SolvePart(2));
    }

    [Fact]
    private static void Part2ExampleLarge()
    {
        var day12 = new Day12("input/day12e3.txt");
        Assert.Equal(1206, day12.SolvePart(2));
    }
    
    [Fact]
    private static void Part2ExampleOwn()
    {
        var day12 = new Day12("input/day12e6.txt");
        int area = 6*3, corners = 4;
        Assert.Equal(area * corners, day12.SolvePart(2));
    }
    
    [Fact]
    private static void Part1Solution()
    {
        var day12 = new Day12("input/day12.txt");
        Assert.Equal(1456082, day12.SolvePart(1));
    }
    
    [Fact]
    private static void Part2Solution()
    {
        var day12 = new Day12("input/day12.txt");
        Assert.Equal(872382, day12.SolvePart(2));
    }
}