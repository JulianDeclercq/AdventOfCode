using System.Text;
using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day21(string inputPath)
{
    /*
     * +---+---+---+
       | 7 | 8 | 9 |
       +---+---+---+
       | 4 | 5 | 6 |
       +---+---+---+
       | 1 | 2 | 3 |
       +---+---+---+
           | 0 | A |
           +---+---+
     */
    public void Part1()
    {
        // var lines = File.ReadAllLines(inputPath);
        var example = "029A";

        var invalid = '@';
        var numericPad = new Grid<char>(3, 4,
        [
            '7', '8', '9',
            '4', '5', '6',
            '1', '2', '3',
            invalid, '0', 'A'
        ], invalid);

        var numericToPosition = new Dictionary<char, Point>
        {
            ['7'] = new Point(0, 0), ['8'] = new Point(1, 0), ['9'] = new Point(2, 0),
            ['4'] = new Point(0, 1), ['5'] = new Point(1, 1), ['6'] = new Point(2, 1),
            ['1'] = new Point(0, 2), ['2'] = new Point(1, 2), ['3'] = new Point(2, 2),
            ['0'] = new Point(1, 3), ['A'] = new Point(2, 3),
        };

        // will throw if mapping is not correct
        foreach (var point in numericPad.AllExtendedLookup().Keys)
            TestMapping(point, numericPad, numericToPosition);

        // foreach (var lel in numericToPosition.Keys) Console.WriteLine($"{lel}, {(int)lel}");

        var commands = new StringBuilder();
        var fingerPosition = numericToPosition['A'];
        foreach (var target in example)
        {
            var targetPosition = numericToPosition[target];
            // var diff = fingerPosition - targetPosition;
            var diff = targetPosition - fingerPosition;
            commands.Append(DiffToCommand(diff)); // move
            commands.Append('A'); // confirm
            // TODO: add commands
            fingerPosition = targetPosition;
            int bkp = 5;
        }
        
        Console.WriteLine(commands);
    }

    private static string DiffToCommand(Point diff)
    {
        var result = new StringBuilder();
        for (var i = 0; i < Math.Abs(diff.X); i++)
            result.Append(diff.X > 0 ? '>' : '<');

        for (var i = 0; i < Math.Abs(diff.Y); i++)
            result.Append(diff.Y > 0 ? 'v' : '^');

        return result.ToString();
    }

    private static void TestMapping(Point p, Grid<char> numericPad, Dictionary<char, Point> numericToPosition)
    {
        var value = numericPad.At(p);

        if (value == numericPad._invalid)
            return;

        if (!p.Equals(numericToPosition[value]))
            throw new Exception("lel");
    }
}