using System.Text;

namespace AdventOfCode2024;

public class Day9
{
    private class File
    {
        public const int FreeSpace = -1;

        public int Size { get; init; } = 0;
        public int Content { get; init; } = -1;
        public bool MovedOrTriedTo { get; set; } = false;

        public bool IsFreeSpace => Content == FreeSpace;

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < Size; ++i)
                sb.Append(IsFreeSpace ? "." : Content.ToString());

            return sb.ToString();
        }
    }

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
        List<File> diskFiles = [];

        // Load
        var id = 0;
        var file = true;
        foreach (var number in input)
        {
            // new
            var theehee = new File
            {
                Size = number,
                Content = file ? id : File.FreeSpace,
            };
            diskFiles.Add(theehee);

            // old
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
            // Part2(disk);
            Part2he(diskFiles);
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
        // Console.WriteLine(string.Join("", disk));

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
        var original = disk.ToList();

        var reverseOffset = 0;
        HashSet<char> processedGroups = [];

        // Console.WriteLine(string.Join("", disk));
        for (;;)
        {
            var reversed = original.ToList();
            reversed.Reverse();

            var firstNonEmpty = reversed.Skip(reverseOffset).ToList().FindIndex(c => c is not Empty);
            reverseOffset += firstNonEmpty;
            var groupId = reversed[reverseOffset];

            var progress = (int)groupId;
            if (progress % 100 == 0)
                Console.WriteLine($"{10000 - groupId}");
            // Console.WriteLine($"REVERSE OFFSET {reverseOffset}, group ID: {groupId}");

            // exit condition: this group has already been processed
            if (!processedGroups.Add(groupId))
                break;

            var fileSize = reversed.Skip(reverseOffset).TakeWhile(c => c == groupId).Count();

            // find an empty fitting space
            var searchOffset = 0;
            for (;;)
            {
                // no match
                var noLongerToTheLeft = searchOffset >= disk.FindIndex(c => c == groupId);
                if (searchOffset >= disk.Count || noLongerToTheLeft)
                {
                    reverseOffset += fileSize;
                    break;
                }

                var relativeIndex = disk.Skip(searchOffset + 1).ToList().FindIndex(c => c is Empty);
                var emptyStart = searchOffset + relativeIndex + 1; // sure this is also +1?
                var emptySpace = disk.Skip(emptyStart).TakeWhile(c => c is Empty).Count(); // skip off by one?

                if (fileSize > emptySpace)
                {
                    searchOffset = emptyStart + emptySpace;
                    continue;
                }

                for (var i = 0; i < fileSize; ++i)
                {
                    disk[emptyStart + i] = groupId;
                    disk[disk.Count - 1 - reverseOffset - i] = Empty;
                    // Console.WriteLine(string.Join("", disk));
                }
                // Console.WriteLine(string.Join("", disk));

                reverseOffset += fileSize;
                break;
            }
        }
    }

    private static void Part2he(List<File> disk)
    {
        for (;;)
        {
            Console.WriteLine($"BEFORE\n {string.Join("", disk)}");

            // start at the highest and try to move it
            for (var i = disk.Count - 1; i >= 0; i--)
            {
                // skip the already moved ones
                var highest = disk[i];
                if (highest.MovedOrTriedTo)
                    continue;

                for (var j = 0; j < disk.Count; ++j)
                {
                    var freeSpace = disk[j];
                    if (!freeSpace.IsFreeSpace)
                        continue;

                    if (freeSpace.Size == highest.Size)
                    {
                        // candidate takes space of the free space
                        disk[j] = highest;

                        // free up the space where the candidate originally was (TODO: Is this needed?)
                        disk[i] = new File
                        {
                            Content = File.FreeSpace,
                            Size = highest.Size
                        };
                        
                        highest.MovedOrTriedTo = true;
                        break;
                    }
                    else if (freeSpace.Size > highest.Size)
                    {
                        // candidate takes space of the free space
                        disk[j] = highest;
                        // Console.WriteLine($"1 {string.Join("", disk)}");

                        // free up the space where the candidate originally was (TODO: Is this needed?)
                        disk[i] = new File
                        {
                            Content = File.FreeSpace,
                            Size = highest.Size
                        };
                        // Console.WriteLine($"2 {string.Join("", disk)}");

                        // need to add free space after with the remainder
                        var leftover = new File
                        {
                            Content = File.FreeSpace,
                            Size = freeSpace.Size - highest.Size
                        };
                        disk.Insert(j + 1, leftover);
                        // Console.WriteLine($"3 {string.Join("", disk)}");
                        
                        highest.MovedOrTriedTo = true;
                        break;
                    }

                    highest.MovedOrTriedTo = true;
                }
                
                Console.WriteLine($"{string.Join("", disk)}");
            }
            break;
        }
    }
}