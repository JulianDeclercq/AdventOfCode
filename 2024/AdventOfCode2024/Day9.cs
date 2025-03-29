using System.Numerics;
using System.Text;

namespace AdventOfCode2024;

public class Day9
{
    private class File
    {
        public static readonly BigInteger FreeSpace = -1;

        public BigInteger Size { get; init; } = 0;
        public BigInteger Content { get; init; } = -1;
        public bool MovedOrTriedTo { get; set; } = false;

        public bool IsFreeSpace => Content == FreeSpace;

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (BigInteger i = 0; i < Size; ++i)
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
            System.IO.File.ReadAllLines("input/day9.txt").First()
            // "2333133121414131402"
            .Select(c => (int)char.GetNumericValue(c))
            .ToArray();

        List<char> disk = [];
        List<File> diskFiles = [];

        // Load
        var id = 0;
        var isFile = true;
        foreach (var number in input)
        {
            // part 2 file approach
            var file = new File
            {
                Size = number,
                Content = isFile ? id : File.FreeSpace,
            };
            diskFiles.Add(file);

            // part 1 char approach
            for (var i = 0; i < number; ++i)
                disk.Add(isFile ? (char)(id + '0') : Empty);

            if (isFile)
                id++;

            isFile = !isFile;
        }

        // Process
        if (part is 1)
        {
            Part1(disk);
            var checksum = CheckSum(disk);
            Console.WriteLine($"Part 1 : {checksum}");
        }
        else
        {
            Part2(diskFiles);
            // Console.WriteLine($"Disk: {string.Join("", diskFiles)}");
            Console.WriteLine($"Part 2 : {CheckSum2(diskFiles)}");
        }
    }

    private static BigInteger CheckSum(List<char> disk)
    {
        BigInteger checksum = 0;
        for (var i = 0; i < disk.Count; ++i)
        {
            if (disk[i] is Empty)
                continue;

            var lhs = i;
            var rhs = (disk[i] - '0');
            var result = lhs * rhs;
            checksum += result;
        }

        return checksum;
    }

    private static BigInteger CheckSum2(List<File> disk)
    {
        BigInteger checksum = 0;
        BigInteger offset = 0;
        foreach (var file in disk)
        {
            for (var i = 0; i < file.Size; ++i)
            {
                if (!file.IsFreeSpace)
                    checksum += offset * file.Content;

                offset++;
            }
        }

        return checksum;
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

    private static void Part2(List<File> disk)
    {
        // Console.WriteLine($"INITIAL\n{string.Join("", disk)}");
        for (;;)
        {
            // start at the highest and try to move it
            for (var i = disk.Count - 1; i >= 0; i--)
            {
                // skip the already moved ones
                var highest = disk[i];
                if (highest.IsFreeSpace || highest.MovedOrTriedTo)
                    continue;

                for (var j = 0; j < disk.Count; ++j)
                {
                    var freeSpace = disk[j];
                    if (!freeSpace.IsFreeSpace)
                        continue;

                    if (i <= j)
                        break;

                    if (freeSpace.Size == highest.Size)
                    {
                        // candidate takes space of the free space
                        disk[j] = highest;

                        // free up the space where the candidate originally was
                        disk[i] = new File
                        {
                            Content = File.FreeSpace,
                            Size = highest.Size
                        };
                        // Console.WriteLine($"X {string.Join("", disk)}");

                        highest.MovedOrTriedTo = true;
                        break;
                    }

                    if (freeSpace.Size > highest.Size)
                    {
                        // candidate takes space of the free space
                        disk[j] = highest;
                        // Console.WriteLine($"1 {string.Join("", disk)}");

                        // free up the space where the candidate originally was
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
            }

            break;
        }
    }
}