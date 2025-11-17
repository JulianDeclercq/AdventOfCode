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

        var numericMapping = new Dictionary<char, Point>
        {
            ['7'] = new Point(0, 0), ['8'] = new Point(1, 0), ['9'] = new Point(2, 0),
            ['4'] = new Point(0, 1), ['5'] = new Point(1, 1), ['6'] = new Point(2, 1),
            ['1'] = new Point(0, 2), ['2'] = new Point(1, 2), ['3'] = new Point(2, 2),
            ['0'] = new Point(1, 3), ['A'] = new Point(2, 3),
        };

        /*
         *     +---+---+
               | ^ | A |
           +---+---+---+
           | < | v | > |
           +---+---+---+
         */
        var directionalPad = new Grid<char>(3, 2,
        [
            invalid, '^', 'A',
            '<', 'v', '>',
        ], invalid);

        var directionalMapping = new Dictionary<char, Point>
        {
            ['^'] = new Point(1, 0),
            ['A'] = new Point(2, 0),
            ['<'] = new Point(0, 1),
            ['v'] = new Point(1, 1),
            ['>'] = new Point(2, 1)
        };

        foreach (var point in numericPad.AllExtendedLookup().Keys)
            TestMapping(point, numericPad, numericMapping);

        foreach (var point in directionalPad.AllExtendedLookup().Keys)
            TestMapping(point, directionalPad, directionalMapping);

        var commands = new StringBuilder();
        var first = ControlArm(example, numericMapping);
        commands.Append(first);
        Console.WriteLine(commands);
        var second = ControlArm(first, directionalMapping);
        commands.Append(second);
        Console.WriteLine(commands);
    }

    // targetOutput is what you want the next robot to enter
    private static string ControlArm(string targetOutput, Dictionary<char, Point> mapping)
    {
        var commands = new StringBuilder();
        var fingerPosition = mapping['A'];
        foreach (var target in targetOutput)
        {
            var targetPosition = mapping[target];
            var diff = targetPosition - fingerPosition;
            commands.Append(DiffToCommand(diff)); // move
            commands.Append('A'); // confirm
            fingerPosition = targetPosition;
            int bkp = 5;
        }

        return commands.ToString();
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

    private static void TestMapping(Point p, Grid<char> pad, Dictionary<char, Point> mapping)
    {
        var value = pad.At(p);

        if (value == pad._invalid)
            return;

        if (!p.Equals(mapping[value]))
            throw new Exception($"Expected value {value} at {p} instead of at {mapping[value]}");
    }

    private static void TestDirectionalMapping(Point p, Grid<char> numericPad,
        Dictionary<char, Point> numericToPosition)
    {
        var value = numericPad.At(p);

        if (value == numericPad._invalid)
            return;

        if (!p.Equals(numericToPosition[value]))
            throw new Exception("lel");
    }
}