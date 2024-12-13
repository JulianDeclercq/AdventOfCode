using System.Text.RegularExpressions;
using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day13
{
    private class ClawMachine
    {
        public Point AOffset { get; init; }
        public Point BOffset { get; init; }
        public LongPoint Prize { get; init; }

        public override string ToString()
        {
            return $"A {AOffset}, B {BOffset}, Prize {Prize}";
        }
    }
    
    public static void Solve(int part)
    {
        if (part != 1 && part != 2)
            throw new Exception($"Invalid part {part}");
                
        Console.WriteLine(
            ParseInput("day13.txt", part)
            .Select(FewestTokensForWin2)
            .Where(x => x is not null)
            .Sum());
    }

    private static int? FewestTokensForWin(ClawMachine clawMachine)
    {
        // Part 1 specifies that buttons should be max pressed 100 so let's go with that for now
        var cheapest = int.MaxValue;
        for (var a = 0; a <= 100; a++)
        {
            for (var b = 0; b <= 100; b++)
            {
                var xOffset = (clawMachine.AOffset.X * a) + (clawMachine.BOffset.X * b);
                var yOffset = (clawMachine.AOffset.Y * a) + (clawMachine.BOffset.Y * b);
                var clawPosition = new Point(xOffset, yOffset);
                if (clawPosition.X == clawMachine.Prize.X && clawPosition.Y == clawMachine.Prize.Y)
                    cheapest = Math.Min(a * 3 + b, cheapest);
            }
        }
        
        return cheapest == int.MaxValue ? null : cheapest;
    }
    
    private static long? FewestTokensForWin2(ClawMachine clawMachine)
    {
        // Great reddit thread explaining the mathematics reddit.com/r/adventofcode/comments/1hd7irq/2024_day_13_an_explanation_of_the_mathematics/
        // x coordinate will move with A times pressed a offset, and B times pressed b offset
        // A * a_x + B * b_x = p_x
        // OR FILLED IN: A PRESSES * clawMachine.AOffset.X + B PRESSES * clawMachine.BOffset.X = clawMachine.Prize.X
        
        // A * a_y + B * b_y = p_y
        // OR FILLED IN: A PRESSES * clawMachine.AOffset.Y + B PRESSES * clawMachine.BOffset.Y = clawMachine.Prize.Y
        
        // Using Cramer's rule to solve this 2 by 2 system (2 unknowns, 2 linear equations (I think xD))
        // Great YouTube video https://www.youtube.com/watch?v=vXqlIOX2itM
        // a1x + b1y = c1
        // a2x + b2y = c2
        
        // for our case, to keep the equations from the yt video / Cramer's rule the same we say that:
        // x is the amount of times the A button needs to be pressed
        // y is the amount of times the B button needs to be pressed
        
        // To find x and y we need the determinant (https://en.wikipedia.org/wiki/Determinant)
        // ab determinant of this 2x2 matrix (second part is on the next line) is a*d - b*c
        // cd
        // If I'm reading this in the future and confused, look at the 2 linear equations I wrote a couple lines
        // then take upper left * lower right - upper right * lower left
        var determinant = clawMachine.AOffset.X * clawMachine.BOffset.Y - clawMachine.BOffset.X * clawMachine.AOffset.Y;

        // Values for x (A button) and y (B button)
        // x = Dx / D
        // y = Dy / D
        
        // Dx = c1*b2 - c2*b1 (see YouTube video)
        var dX = clawMachine.Prize.X * clawMachine.BOffset.Y - clawMachine.Prize.Y * clawMachine.BOffset.X;
        
        // Dy = a1*c2 - a2*c1 (see YouTube video)
        var dY = clawMachine.AOffset.X * clawMachine.Prize.Y - clawMachine.AOffset.Y * clawMachine.Prize.X;

        var x = dX / determinant;
        var y = dY / determinant;

        if (x * clawMachine.AOffset.X + y * clawMachine.BOffset.X == clawMachine.Prize.X &&
            x * clawMachine.AOffset.Y + y * clawMachine.BOffset.Y == clawMachine.Prize.Y)
        {
            // need to do this check to make sure this is a valid solution. In the example, the second and fourth
            // claw machines don't have a solution, and they don't pass this check
            return x * 3 + y;
        }

        // there is no solution for this claw machine
        return null;
    }

    private static List<ClawMachine> ParseInput(string fileName, int part)
    {
        var lines = File.ReadAllLines($"input/{fileName}");
        var offset = 0;
        var regex = new RegexHelper(new Regex(@".+: X.(\d+), Y.(\d+)"), "x", "y");
        List<ClawMachine> clawMachines = [];
        
        while (offset < lines.Length)
        {
            var clawDescription = lines.Skip(offset).TakeWhile(l => !string.IsNullOrWhiteSpace(l)).ToList();
            regex.Match(clawDescription[0]);
            var aOffset = new Point(regex.GetInt("x"), regex.GetInt("y"));
            
            regex.Match(clawDescription[1]);
            var bOffset = new Point(regex.GetInt("x"), regex.GetInt("y"));
            
            regex.Match(clawDescription[2]);
            var prize = part is 1
                ? new LongPoint(regex.GetLong("x"), regex.GetLong("y"))
                : new LongPoint(regex.GetLong("x") + 10_000_000_000_000, regex.GetLong("y") + 10_000_000_000_000);
                
            var clawMachine = new ClawMachine
            {
                AOffset = aOffset,
                BOffset = bOffset,
                Prize = prize
            };
            clawMachines.Add(clawMachine);
            offset += clawDescription.Count + 1;
        }
        
        return clawMachines;
    }
}