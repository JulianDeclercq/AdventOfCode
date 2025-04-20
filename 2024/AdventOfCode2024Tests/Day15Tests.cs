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

    [Fact]
    private void Large_example_should_be_correct()
    {
        var day = new Day15("input/day15e2.txt");
        day.Solve();

        var grid = day.GetGrid();
        testOutputHelper.WriteLine(grid.ToString());
        Assert.Equal(Day15.Box, grid.At(2, 1));
        Assert.Equal(Day15.Box, grid.At(2, 4));
        Assert.Equal(Day15.Robot, grid.At(3, 4));
        Assert.Equal(Day15.Empty, grid.At(4, 4));
        Assert.Equal(Day15.Empty, grid.At(3, 5));

        /*
         * ##########
           #.O.O.OOO#
           #........#
           #OO......#
           #OO@.....#
           #O#.....O#
           #O.....OO#
           #O.....OO#
           #OO....OO#
           ##########
         */
    }

    [Fact]
    private void Large_example_gps_sum_should_be_correct()
    {
        var day = new Day15("input/day15e2.txt");
        day.Solve();

        Assert.Equal(10092, day.GpsSum());
    }

    [Fact]
    private void Part1_gps_sum_should_be_correct()
    {
        var day = new Day15("input/day15.txt");
        day.Solve();

        Assert.Equal(1441031, day.GpsSum());
    }

    [Fact]
    private void Part2_example_should_have_wide_grid()
    {
        var day = new Day15("input/day15e3.txt", part: 2);
        var grid = day.GetGrid();
        testOutputHelper.WriteLine(grid.ToString());

        Assert.Equal(Day15.BoxLeft, grid.At(8, 3));
        Assert.Equal(Day15.BoxRight, grid.At(9, 3));
        Assert.Equal(Day15.BoxLeft, grid.At(6, 3));
        Assert.Equal(Day15.BoxRight, grid.At(7, 3));
        Assert.Equal(Day15.Robot, grid.At(10, 3));

        /*
            ##############
            ##......##..##
            ##..........##
            ##....[][]@.##
            ##....[]....##
            ##..........##
            ##############
         */
    }

    [Fact]
    private void Part2_should_push_wide_boxes_west()
    {
        var day = new Day15("input/day15e3.txt", part: 2);
        var grid = day.GetGrid();
        testOutputHelper.WriteLine(grid.ToString());

        day.Step(part: 2);
        testOutputHelper.WriteLine(grid.ToString());

        Assert.Equal(Day15.BoxLeft, grid.At(7, 3));
        Assert.Equal(Day15.BoxRight, grid.At(8, 3));
        Assert.Equal(Day15.BoxLeft, grid.At(5, 3));
        Assert.Equal(Day15.BoxRight, grid.At(6, 3));
        Assert.Equal(Day15.Robot, grid.At(9, 3));

        /*
            ##############
            ##......##..##
            ##..........##
            ##...[][]@..##
            ##....[]....##
            ##..........##
            ##############
         */
    }
}