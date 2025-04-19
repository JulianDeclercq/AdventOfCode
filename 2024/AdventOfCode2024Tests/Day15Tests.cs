using AdventOfCode2024;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2024Tests;

public class Day15Tests(ITestOutputHelper testOutputHelper)
{
    [Fact]
    private void Robot_shouldnt_move_into_wall()
    {
        var day = new Day15("input/day15e1.txt");

        day.Step();

        var grid = day.GetGrid();
        Assert.Equal(Day15.Robot, grid.At(2, 2));

        /*
         * ########
           #..O.O.#
           ##@.O..#
           #...O..#
           #.#.O..#
           #...O..#
           #......#
           ########
         */
    }

    [Fact]
    private void Robot_should_move_into_free_space()
    {
        var day = new Day15("input/day15e1.txt");

        for (var i = 0; i < 2; ++i)
            day.Step();

        var grid = day.GetGrid();
        Assert.Equal(Day15.Empty, grid.At(2, 2));
        Assert.Equal(Day15.Robot, grid.At(2, 1));

        /*
         * ########
           #.@O.O.#
           ##..O..#
           #...O..#
           #.#.O..#
           #...O..#
           #......#
           ########
         */
    }

    [Fact]
    private void Robot_should_move_single_box_into_free_space()
    {
        var day = new Day15("input/day15e1.txt");

        for (var i = 0; i < 4; ++i)
        {
            day.Step();
            testOutputHelper.WriteLine(day.GetGrid().ToString());
        }

        var grid = day.GetGrid();
        Assert.Equal(Day15.Empty, grid.At(2, 1));
        Assert.Equal(Day15.Robot, grid.At(3, 1));
        Assert.Equal(Day15.Box, grid.At(4, 1));

        /*
         *  ########
            #..@OO.#
            ##..O..#
            #...O..#
            #.#.O..#
            #...O..#
            #......#
            ########
         */
    }

    [Fact]
    private void Robot_should_move_double_box_into_free_space()
    {
        var day = new Day15("input/day15e1.txt");

        for (var i = 0; i < 5; ++i)
            day.Step();

        var grid = day.GetGrid();
        Assert.Equal(Day15.Empty, grid.At(3, 1));
        Assert.Equal(Day15.Robot, grid.At(4, 1));
        Assert.Equal(Day15.Box, grid.At(5, 1));
        Assert.Equal(Day15.Box, grid.At(6, 1));

        /*
         *  ########
            #...@OO#
            ##..O..#
            #...O..#
            #.#.O..#
            #...O..#
            #......#
            ########
         */
    }

    [Fact]
    private void Robot_should_move_box_row_into_free_space()
    {
        var day = new Day15("input/day15e1.txt");

        for (var i = 0; i < 7; ++i)
        {
            day.Step();
            testOutputHelper.WriteLine(day.GetGrid().ToString());
        }

        var grid = day.GetGrid();
        Assert.Equal(Day15.Empty, grid.At(4, 1));
        Assert.Equal(Day15.Robot, grid.At(4, 2));
        Assert.Equal(Day15.Box, grid.At(4, 6));

        /*
         *  ########
            #....OO#
            ##..@..#
            #...O..#
            #.#.O..#
            #...O..#
            #...O..#
            ########
         */
    }

    [Fact]
    private void Short_example_should_be_correct()
    {
        var day = new Day15("input/day15e1.txt");
        day.Solve();

        var grid = day.GetGrid();
        Assert.Equal(Day15.Box, grid.At(3, 4));
        Assert.Equal(Day15.Robot, grid.At(4, 4));
        Assert.Equal(Day15.Empty, grid.At(5, 4));
        Assert.Equal(Day15.Box, grid.At(4, 5));
        Assert.Equal(Day15.Box, grid.At(4, 6));

        /*
         * ########
           #....OO#
           ##.....#
           #.....O#
           #.#O@..#
           #...O..#
           #...O..#
           ########
         */
    }
}