using AdventOfCode2023.helpers;

namespace AdventOfCode2023.days;

public class Day14
{
    private static Dictionary<string, (int count, List<int> cycles, int load)> _occurrences = new();
    public static void Solve()
    {
        var input = File.ReadAllLines("../../../input/Day14.txt");
        var grid = new Grid<char>(input.First().Length, input.Length, input.SelectMany(x => x), '?');

        var modulo182 = 112 % 18;
        var modulo1822 = 130 % 18;
        var modulo18 = 1_000_000_000 % 18;

        for (var i = 0; i < 1_000_000_000; ++i)
        {
            if (i != 0 && i % 1_000_000 == 0)
                Qonsole.OverWrite($"{i / 1_000_000} million iterations{string.Concat(Enumerable.Repeat('.', (int)(i / 1_000_000) % 3 + 1))}");

            Qonsole.OverWrite($"{i}");
            CycleGrid(grid, i + 1);
        }

        var answer = grid.Columns().Sum(col => CalculateLoad(col.Select(c => c.Value).ToArray()));
        Console.WriteLine(answer);
    }

    private static void CycleGrid(Grid<char> grid, int cycle)
    {
        //Console.WriteLine(grid);

        if (cycle == 118)
        {
            int bkpt = 5;
        }
        // north
        //Console.WriteLine("Rolling North");
        var columns = grid.Columns().ToArray();
        for (var i = 0; i < columns.Length; ++i)
        {
            var column = string.Join("", columns[i].Select(r => r.Value));
            var movedBoulders = MoveBoulders(column);
            grid.ReplaceColumn(i, movedBoulders.ToCharArray());
        }
        //Console.WriteLine(grid);
        
        // west
        //Console.WriteLine("Rolling West");
        var rows = grid.Rows().ToArray();
        for (var i = 0; i < rows.Length; ++i)
        {
            var row = string.Join("", rows[i].Select(r => r.Value));
            var movedBoulders = MoveBoulders(row);
            grid.ReplaceRow(i, movedBoulders.ToCharArray());
        }
        //Console.WriteLine(grid);

        // south
        //Console.WriteLine("Rolling South");
        columns = grid.Columns().ToArray();
        for (var i = 0; i < columns.Length; ++i)
        {
            var column = string.Join("", columns[i].Select(r => r.Value));
            var movedBoulders = MoveBoulders(string.Join("", column.Reverse()));
            grid.ReplaceColumn(i, movedBoulders.Reverse().ToArray());
        }
        //Console.WriteLine(grid);

        // east
        //Console.WriteLine("Rolling East");
        rows = grid.Rows().ToArray();
        for (var i = 0; i < rows.Length; ++i)
        {
            var row = string.Join("", rows[i].Select(r => r.Value));
            var movedBoulders = MoveBoulders(string.Join("", row.Reverse()));
            grid.ReplaceRow(i, movedBoulders.Reverse().ToArray());
        }
        //Console.WriteLine(grid);
        //Console.WriteLine("Cycle done");

        var stringified = new string(grid.All().ToArray());
        var occurrence = _occurrences.TryGetValue(stringified, out var existing) ? existing : (0, new List<int>(), 0);
        occurrence.Item1 += 1;
        occurrence.Item2.Add(cycle);
        occurrence.Item3 = grid.Columns().Sum(col => CalculateLoad(col.Select(c => c.Value).ToArray()));
        _occurrences[stringified] = occurrence;
    }

    private static int CalculateLoad(IReadOnlyList<char> column)
    {
        var answer = 0;
        for (var i = 0; i < column.Count; ++i)
        {
            if (column[i] == 'O')
                answer += column.Count - i;
        }
        return answer;
    }

    private static string MoveBoulders(string input)
    {
        var offset = 0;
        var workingCopy = input.ToCharArray();
        while (offset < workingCopy.Length)
        {
            var oIdx = new string(workingCopy)[offset..].IndexOf('O');
            if (oIdx == -1)
                break;

            oIdx += offset;
            var i = oIdx - 1;
            var bestFreeSpot = int.MaxValue;
            while (i >= 0)
            {
                switch (workingCopy[i])
                {
                    case '#': goto Skip;
                    case 'O': goto Skip;
                    case '.':
                        bestFreeSpot = i;
                        break;
                }
                i--;
            }

            Skip:
            if (bestFreeSpot != int.MaxValue)
            {
                workingCopy[bestFreeSpot] = 'O';
                workingCopy[oIdx] = '.';
            }

            offset++;
        }

        return new string(workingCopy);
    }
}