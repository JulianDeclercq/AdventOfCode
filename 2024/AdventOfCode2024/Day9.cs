namespace AdventOfCode2024;

public class Day9
{
    public static void Solve()
    {
        var input =
            File
            .ReadAllLines("input/day9.txt")
            .Single()
            // "2333133121414131402"
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
        for (var i = 0; i < disk.Count; ++i)
        {
            // Console.WriteLine(string.Join("", disk));
            
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
        }

        long checksum = 0;
        for (var i = 0; i < disk.Count; ++i)
        {
            if (disk[i] is empty)
                continue; // could probably break but don't want to risk it now before part 2 :)

            checksum += i * (disk[i] - '0');
        }
        
        Console.WriteLine(checksum);
    }
}