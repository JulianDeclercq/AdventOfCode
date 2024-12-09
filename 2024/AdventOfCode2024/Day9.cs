namespace AdventOfCode2024;

public class Day9
{
    public static void Solve()
    {
        var input =
            // File
            // .ReadAllLines("input/day9e.txt")
            // .Single()
            // "12345"
            "2333133121414131402"
            .Select(c => (int)char.GetNumericValue(c))
            .ToArray();
        
        const char empty = '.'; 
        List<char> disk = [];

        // Load
        var id = 0;
        var file = true;
        foreach (var number in input)
        {
            for (var i = 0; i < number; ++i)
                disk.Add(file ? (char)(id + '0') : empty);

            if (file)
                id++;
            
            file = !file;
        }
        
        // Process
        var lastIndexTaken = disk.Count;
        Console.WriteLine(string.Join("", disk));
        for (var i = 0; i < disk.Count; ++i)
        {
            // break if we've reached the last thing we've taken, this means all empty spots up until now have been filled
            if (lastIndexTaken <= i)
                break;

            if (disk[i] is not empty)
                continue;
            
            // find the last non-empty character
            // for (var j = disk.Count - 1; j > i; --j)
            for (var j = lastIndexTaken - 1; j > i; --j)
            {
                if (disk[j] is not empty)
                {
                    lastIndexTaken = j;
                    disk[i] = disk[lastIndexTaken];
                    disk[lastIndexTaken] = '.';
                    break;
                }
            }
            Console.WriteLine(string.Join("", disk));
        }
    }
}