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

        foreach (var point in numericPad.AllExtendedLookup().Keys)
            TestMapping(point, numericPad, numericToPosition);

        Console.WriteLine("nice!");
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