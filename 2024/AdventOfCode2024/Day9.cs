namespace AdventOfCode2024;

public class Day9
{
    private const char Empty = '.'; 
    public static void Solve(int part)
    {
        if (part != 1 && part != 2)
            throw new Exception($"Invalid part {part}");
        
        var input =
            // File.ReadAllLines("input/day9.txt").Single()
            "2333133121414131402"
            .Select(c => (int)char.GetNumericValue(c))
            .ToArray();
        
        List<char> disk = [];

        // Load
        var id = 0;
        var file = true;
        foreach (var number in input)
        {
            for (var i = 0; i < number; ++i)
                disk.Add(file ? (char)(id + '0') : Empty);

            if (file)
                id++;
            
            file = !file;
        }
        
        // Process
        if (part is 1)
        {
            Part1(disk);
        }
        else
        {
            Part2(disk);
        }

        long checksum = 0;
        for (var i = 0; i < disk.Count; ++i)
        {
            if (disk[i] is Empty)
                continue;
            
            var lhs = i;
            var rhs = (disk[i] - '0');
            var result = lhs * rhs;
            checksum += result;
        }
        
        Console.WriteLine(checksum);
    }

    private static void Part1(List<char> disk)
    {
        var lastIndexTaken = disk.Count;
        for (var i = 0; i < disk.Count; ++i)
        {
            // Console.WriteLine(string.Join("", disk));
            
            // break if we've reached the last thing we've taken, this means all empty spots up until now have been filled
            if (lastIndexTaken <= i)
                break;

            if (disk[i] is not Empty)
                continue;
            
            // find the last non-empty character
            // for (var j = disk.Count - 1; j > i; --j)
            for (var j = lastIndexTaken - 1; j > i; --j)
            {
                if (disk[j] is not Empty)
                {
                    lastIndexTaken = j;
                    disk[i] = disk[lastIndexTaken];
                    disk[lastIndexTaken] = '.';
                    break;
                }
            }
        }
    }
    
    private static void Part2(List<char> disk)
    {
        var lastIndexTaken = disk.Count;
        for (var i = 0; i < disk.Count; ++i)
        {
            // Console.WriteLine(string.Join("", disk));
            
            // break if we've reached the last thing we've taken, this means all empty spots up until now have been filled
            if (lastIndexTaken <= i)
                break;

            if (disk[i] is not Empty)
                continue;
            
            // find the last non-empty character
            // for (var j = disk.Count - 1; j > i; --j)
            for (var j = lastIndexTaken - 1; j > i; --j)
            {
                if (disk[j] is not Empty)
                {
                    lastIndexTaken = j;
                    disk[i] = disk[lastIndexTaken];
                    disk[lastIndexTaken] = '.';
                    break;
                }
            }
        }
    }
}