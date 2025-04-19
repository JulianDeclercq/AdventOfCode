using AdventOfCode2024;
using Xunit;

namespace AdventOfCode2024Tests;

public static class Day15Tests
{
    [Fact]
    private static void Robot_shouldnt_move_into_wall()
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
    private static void Robot_should_move_into_free_space()
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
    private static void Robot_should_move_single_box_into_space()
    {
        var day = new Day15("input/day15e1.txt");
        
        for (var i = 0; i < 4; ++i)
            day.Step();

        var grid = day.GetGrid();
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
    private static void Robot_should_move_double_box_into_space()
    {
        var day = new Day15("input/day15e1.txt");
        
        for (var i = 0; i < 5; ++i)
            day.Step();

        var grid = day.GetGrid();
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
}