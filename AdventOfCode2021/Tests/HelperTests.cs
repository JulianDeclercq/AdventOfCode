using AdventOfCode2021;
using Xunit;

namespace Tests;

public class HelperTests
{
    [Theory]
    [InlineData(27, -3, 20, 30, -10, -5, false)]
    [InlineData(28, -7, 20, 30, -10, -5, true)]
    public void Point_should_be_in_range(int px, int py, int xMin, int xMax, int yMin, int yMax, bool expected)
    {
        var p = new Point(px, py);
        var inRange = p.InArea(new (xMin, xMax), new (yMin, yMax));
        Assert.Equal(inRange, expected);
    }
}