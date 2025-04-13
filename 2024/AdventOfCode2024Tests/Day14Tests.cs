using AdventOfCode2024;
using Xunit;

namespace AdventOfCode2024Tests;

public static class Day14Tests
{
    /* INITIAL STATE
       ...........
       ...........
       ...........
       ...........
       ..1........
       ...........
       ........... */
    [Fact]
    private static void Robot_step_1_should_move()
    {
        var day = new Day14();
        day.Initialize(["p=2,4 v=2,-3"], example: true);

        day.Step();
        var grid = day.GetGrid();
        Assert.Empty(grid.At(2, 4)!);
        Assert.NotEmpty(grid.At(4, 1)!);

        /* AFTER 1 STEP
           ...........
           ....1......
           ...........
           ...........
           ...........
           ...........
           ........... */
    }

    [Fact]
    private static void Robot_step_2_should_teleport_from_north_to_south()
    {
        var day = new Day14();
        day.Initialize(["p=2,4 v=2,-3"], example: true);

        for (var i = 0; i < 2; ++i)
            day.Step();

        var grid = day.GetGrid();
        Assert.Empty(grid.At(4, 1)!);
        Assert.NotEmpty(grid.At(6, 5)!);

        /* AFTER 2 STEPS
           ...........
           ...........
           ...........
           ...........
           ...........
           ......1....
           ........... */
    }

    [Fact]
    private static void Robot_step_5_should_teleport_from_east_to_west()
    {
        var day = new Day14();
        day.Initialize(["p=2,4 v=2,-3"], example: true);

        for (var i = 0; i < 5; ++i)
            day.Step();

        var grid = day.GetGrid();
        Assert.Empty(grid.At(6, 6)!);
        Assert.NotEmpty(grid.At(1, 3)!);

        /* AFTER 5 STEPS
           ...........
           ...........
           ...........
           .1.........
           ...........
           ...........
           ........... */
    }

    [Fact]
    private static void Initial_grid_should_match_given_example()
    {
        var day = new Day14();
        day.Initialize([
                "p=0,4 v=3,-3", "p=6,3 v=-1,-3", "p=10,3 v=-1,2", "p=2,0 v=2,-1", "p=0,0 v=1,3",
                "p=3,0 v=-2,-2", "p=7,6 v=-1,-3", "p=3,0 v=-1,-2", "p=9,3 v=2,3", "p=7,3 v=-1,2",
                "p=2,4 v=2,-3", "p=9,5 v=-3,-3"
            ], example: true
        );

        var grid = day.GetGrid();
        Assert.Single(grid.At(0, 0)!);
        Assert.Empty(grid.At(1, 0)!);
        Assert.Single(grid.At(2, 0)!);
        Assert.Equal(2, grid.At(3, 0)!.Count);

        /*
           1.12.......
           ...........
           ...........
           ......11.11
           1.1........
           .........1.
           .......1...
         */
    }

    [Fact]
    private static void After_100_steps_grid_should_match_given_example()
    {
        var day = new Day14();
        day.Initialize([
                "p=0,4 v=3,-3", "p=6,3 v=-1,-3", "p=10,3 v=-1,2", "p=2,0 v=2,-1", "p=0,0 v=1,3",
                "p=3,0 v=-2,-2", "p=7,6 v=-1,-3", "p=3,0 v=-1,-2", "p=9,3 v=2,3", "p=7,3 v=-1,2",
                "p=2,4 v=2,-3", "p=9,5 v=-3,-3"
            ], example: true
        );

        for (var i = 0; i < 100; ++i)
            day.Step();

        var grid = day.GetGrid();
        Assert.Equal(2, grid.At(6, 0)!.Count);
        Assert.Single(grid.At(9, 0)!);

        /*
            ......2..1.
            ...........
            1..........
            .11........
            .....1.....
            ...12......
            .1....1....
         */
    }
}