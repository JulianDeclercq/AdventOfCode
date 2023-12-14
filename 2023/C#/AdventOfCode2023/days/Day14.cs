using System.Text;
using AdventOfCode2023.helpers;

namespace AdventOfCode2023.days;

public class Day14
{
    public void Solve()
    {
        var input = File.ReadAllLines("../../../input/Day14.txt");
        var grid = new Grid<char>(input.First().Length, input.Length, input.SelectMany(x => x), '?');
        
        Console.WriteLine(grid);

        var columns = grid.Columns().ToArray();
        for (var i = 0; i < columns.Length; ++i)
        {
            var column = string.Join("", columns[i].Select(r => r.Value));
            var movedBoulders = MoveBoulders(column);
            grid.ReplaceColumn(i, movedBoulders.ToCharArray());
        }

        var answer = grid.Columns().Sum(col => CalculateLoad(col.Select(c => c.Value).ToArray()));
        Console.WriteLine(grid);
        Console.WriteLine(answer);
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
        while (offset < workingCopy.Length) // TODO: off by 1?
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