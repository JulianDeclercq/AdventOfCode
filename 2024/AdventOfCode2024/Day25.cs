using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day25
{
    public static void Solve()
    {
        const int height = 7;
        var lines = File.ReadAllLines("input/real/day25.txt").ToList();
        List<List<int>> locks = [];
        List<List<int>> keys = [];
        for (;;)
        {
            var totalLocksAndKeys = locks.Count + keys.Count;
            var skip = totalLocksAndKeys * height + totalLocksAndKeys;
            var block = lines
                .Skip(skip)
                .TakeWhile(l => !string.IsNullOrWhiteSpace(l))
                .ToList();

            if (block.Count == 0)
                break;
            
            var schematic = new Grid<char>(block[0].Length, height, block.SelectMany(c => c), '@');
            var target = block[0][0] is '#' ? locks : keys;
            target.Add(SchematicToPinHeights(schematic));
        }

        var answer = 0;
        foreach (var @lock in locks)
        {
            foreach (var key in keys)
            {
                if (Fits(@lock, key))
                    answer++;
            }
        }
        Console.WriteLine(answer);
    }
    
    private static List<int> SchematicToPinHeights(Grid<char> schematic)
    {
        List<int> pinHeights = [];
        
        foreach (var column in schematic.Columns())
            pinHeights.Add(column.Count(c => c.Value is '#') - 1); // -1 since the base layer doesn't count

        return pinHeights;
    }

    private static bool Fits(List<int> @lock, List<int> key)
    {
        for (var i = 0; i < @lock.Count; ++i)
        {
            if (@lock[i] + key[i] > 5)
                return false;
        }

        return true;
    }
}