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

    private const int NumericPadId = 0;
    private const int DirectionalPadId = 1;
    private static int _numericLayers = -1;

    public void Solve(bool part2 = false)
    {
        _numericLayers = part2 ? 25 : 2;
        // I had a nice solution but then gaps came in and I became lazy, so I'm hardcoding the paths
        // Let's ignore the gaps for now? it's never further with gaps involved, just different inputs but same dist
        // I also just realised you only need the diff, you don't actually need the path so you could just add up
        // a number instead haha :)

        var codes = File.ReadAllLines(inputPath);
        // var example = "029A";

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

        var memo = new Dictionary<(int padId, int layersRemaining, char from, char to), long>();
        long result = 0;
        foreach (var code in codes)
            result += CodeComplexity(code, numericMapping, directionalMapping, numericPad, directionalPad, memo);

        Console.WriteLine(result);
    }

    private static long CodeComplexity(string code, Dictionary<char, Point> numericMapping,
        Dictionary<char, Point> directionalMapping, Grid<char> numericPad, Grid<char> directionalPad,
        Dictionary<(int padId, int layersRemaining, char from, char to), long> memo)
    {
        var length = SequenceCost(code, numericMapping, numericPad, NumericPadId, _numericLayers, directionalMapping,
            directionalPad, memo);
        var numeric = int.Parse(code[..3]);
        Console.WriteLine($"{code}: {length} * {numeric} = {length * numeric}");
        return length * numeric;
    }

    private static long SequenceCost(string target,
        Dictionary<char, Point> mapping, Grid<char> pad, int padId, int layersRemaining,
        Dictionary<char, Point> directionalMapping, Grid<char> directionalPad,
        Dictionary<(int padId, int layersRemaining, char from, char to), long> memo)
    {
        var finger = 'A';
        long total = 0;
        foreach (var symbol in target)
        {
            total += MoveCost(finger, symbol, mapping, pad, padId, layersRemaining, directionalMapping,
                directionalPad, memo);
            finger = symbol;
        }

        return total;
    }

    private static long MoveCost(char from, char to, Dictionary<char, Point> mapping, Grid<char> pad, int padId,
        int layersRemaining, Dictionary<char, Point> directionalMapping, Grid<char> directionalPad,
        Dictionary<(int padId, int layersRemaining, char from, char to), long> memo)
    {
        var key = (padId, layersRemaining, from, to);
        if (memo.TryGetValue(key, out var cached))
            return cached;

        var fromPoint = mapping[from];
        var toPoint = mapping[to];
        var candidates = GetMoveCandidates(fromPoint, toPoint, pad).ToArray();

        if (candidates.Length == 0)
            throw new Exception($"No valid path found from {from} to {to} on pad {padId}.");

        long best = long.MaxValue;
        foreach (var candidate in candidates)
        {
            var command = candidate + "A";
            var cost = layersRemaining == 0
                ? command.Length
                : CommandCost(command, layersRemaining, directionalMapping, directionalPad, memo);

            best = Math.Min(best, cost);
        }

        memo[key] = best;
        return best;
    }

    private static long CommandCost(string commands, int layersRemaining, Dictionary<char, Point> directionalMapping,
        Grid<char> directionalPad, Dictionary<(int padId, int layersRemaining, char from, char to), long> memo)
    {
        if (layersRemaining == 0)
            return commands.Length;

        long total = 0;
        var finger = 'A';
        foreach (var symbol in commands)
        {
            total += MoveCost(finger, symbol, directionalMapping, directionalPad, DirectionalPadId,
                layersRemaining - 1, directionalMapping, directionalPad, memo);
            finger = symbol;
        }

        return total;
    }

    private static IEnumerable<string> GetMoveCandidates(Point from, Point to, Grid<char> pad)
    {
        var options = new List<string>();
        var horizontalFirst = TryBuildPath(from, to, pad, horizontalFirst: true);
        if (horizontalFirst != null)
            options.Add(horizontalFirst);

        var verticalFirst = TryBuildPath(from, to, pad, horizontalFirst: false);
        if (verticalFirst != null && (horizontalFirst == null || verticalFirst != horizontalFirst))
            options.Add(verticalFirst);

        return options;
    }

    private static string? TryBuildPath(Point from, Point to, Grid<char> pad, bool horizontalFirst)
    {
        var builder = new StringBuilder();
        var currentX = from.X;
        var currentY = from.Y;

        if (horizontalFirst)
        {
            if (!AppendSteps(ref currentX, ref currentY, to.X - currentX, true, pad, builder))
                return null;
            if (!AppendSteps(ref currentX, ref currentY, to.Y - currentY, false, pad, builder))
                return null;
        }
        else
        {
            if (!AppendSteps(ref currentX, ref currentY, to.Y - currentY, false, pad, builder))
                return null;
            if (!AppendSteps(ref currentX, ref currentY, to.X - currentX, true, pad, builder))
                return null;
        }

        return builder.ToString();
    }

    private static bool AppendSteps(ref int currentX, ref int currentY, int steps, bool horizontal, Grid<char> pad,
        StringBuilder builder)
    {
        if (steps == 0)
            return true;

        var stepChar = horizontal
            ? steps > 0 ? '>' : '<'
            : steps > 0 ? 'v' : '^';

        var deltaX = horizontal ? (steps > 0 ? 1 : -1) : 0;
        var deltaY = horizontal ? 0 : (steps > 0 ? 1 : -1);

        for (var i = 0; i < Math.Abs(steps); i++)
        {
            currentX += deltaX;
            currentY += deltaY;

            if (pad.At(currentX, currentY) == pad._invalid)
                return false;

            builder.Append(stepChar);
        }

        return true;
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