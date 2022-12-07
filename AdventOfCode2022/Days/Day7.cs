using System.Text.RegularExpressions;

namespace AdventOfCode2022.Days;

public class Day7
{
    private readonly Helpers.RegexHelper _commandRegex = new(new Regex(@"\$ (\w+) ?(.+)?"), "command", "destination");
    private readonly Helpers.RegexHelper _directoryRegex = new(new Regex(@"dir (\w+)"), "name");
    private readonly Helpers.RegexHelper _fileRegex = new(new Regex(@"(\d+) (.+)"), "size", "name");

    private enum ItemType
    {
        None = 0,
        Item = 1,
        Directory = 2
    }

    private class Item
    {
        public Item()
        {
        }
        
        public string Name = "";
        public ItemType Type = ItemType.None;
        public int Size = 0;
    }
    
    Dictionary<string, List<Item>> _fileSystem = new();
    Dictionary<string, int> _directorySizes = new();
    
    public void Solve()
    {
        var currentPath = "";
        // var lines = File.ReadAllLines(@"..\..\..\input\day7_example.txt");
        var lines = File.ReadAllLines(@"..\..\..\input\day7.txt");

        foreach (var line in lines)
        {
            // command
            if (line.First() == '$')
            {
                _commandRegex.Parse(line);
                var cmd = _commandRegex.Get("command");
                switch (cmd)
                {
                    case "cd":
                    {
                        var destination = _commandRegex.Get("destination");
                        switch (destination)
                        {
                            case "/" :
                                currentPath = "";
                                break;
                            case ".." :
                                currentPath = currentPath[..currentPath.LastIndexOf('/')];
                                break;
                            default:
                                currentPath += $"/{destination}";
                                break;
                        }
                    }
                        break;
                        // case "ls": lsMode = true;
                        break;
                }
            }
            else // not a command but a listing instead
            {
                // define item
                Item item;
                if (_directoryRegex.Parse(line))
                {
                    item = new Item
                    {
                        Name = $"{currentPath}/{_directoryRegex.Get("name")}",
                        Type = ItemType.Directory
                    };
                }
                else if (_fileRegex.Parse(line))
                {
                    item = new Item
                    {
                        Name = _fileRegex.Get("name"),
                        Size = _fileRegex.GetInt("size"),
                        Type = ItemType.Item
                    };
                }
                else throw new Exception($"Couldn't parse ${line}");
                
                // add item
                if (!_fileSystem.TryGetValue(currentPath, out var currentDir))
                {
                    _fileSystem.Add(currentPath, new List<Item>
                    {
                        item
                    });
                }
                else
                {
                    _fileSystem[currentPath].Add(item);
                }
            }
        }

        var result = _fileSystem.Select(path => CalculateSize(path.Value)).Where(size => size <= 100_000).Sum();
        Console.WriteLine($"Day 7 part 1: {result}");
    }
    
    private int CalculateDirectorySize(string directoryName)
    {
        if (_directorySizes.TryGetValue(directoryName, out var foundSize))
            return foundSize;

        var size = CalculateSize(_fileSystem[directoryName]);
        _directorySizes.Add(directoryName, size);
        return size;
    }

    private int CalculateSize(IEnumerable<Item> path) // path = directory?
    {
        return path.Sum(item => item.Type switch
        {
            ItemType.Item => item.Size,
            ItemType.Directory => CalculateDirectorySize(item.Name),
        });
    }
    
     public void Solve2()
    {
        var currentPath = "";
        // var lines = File.ReadAllLines(@"..\..\..\input\day7_example.txt");
        var lines = File.ReadAllLines(@"..\..\..\input\day7.txt");

        foreach (var line in lines)
        {
            // command
            if (line.First() == '$')
            {
                _commandRegex.Parse(line);
                var cmd = _commandRegex.Get("command");
                switch (cmd)
                {
                    case "cd":
                    {
                        var destination = _commandRegex.Get("destination");
                        switch (destination)
                        {
                            case "/" :
                                currentPath = "";
                                break;
                            case ".." :
                                currentPath = currentPath[..currentPath.LastIndexOf('/')];
                                break;
                            default:
                                currentPath += $"/{destination}";
                                break;
                        }
                    }
                        break;
                        // case "ls": lsMode = true;
                        break;
                }
            }
            else // not a command but a listing instead
            {
                // define item
                Item item;
                if (_directoryRegex.Parse(line))
                {
                    item = new Item
                    {
                        Name = $"{currentPath}/{_directoryRegex.Get("name")}",
                        Type = ItemType.Directory
                    };
                }
                else if (_fileRegex.Parse(line))
                {
                    item = new Item
                    {
                        Name = _fileRegex.Get("name"),
                        Size = _fileRegex.GetInt("size"),
                        Type = ItemType.Item
                    };
                }
                else throw new Exception($"Couldn't parse ${line}");
                
                // add item
                if (!_fileSystem.TryGetValue(currentPath, out var currentDir))
                {
                    _fileSystem.Add(currentPath, new List<Item>
                    {
                        item
                    });
                }
                else
                {
                    _fileSystem[currentPath].Add(item);
                }
            }
        }

        var result = _fileSystem.Select(path => CalculateSize(path.Value)).Where(size => size <= 100_000).Sum();
        Console.WriteLine($"Day 7 part 1: {result}");

        const int totalSpace = 70_000_000;
        const int requiredSpace = 30_000_000;
        var usedSpace = CalculateDirectorySize("");
        var unusedSpace = totalSpace - usedSpace;
        var minimumDeletionSize = requiredSpace - unusedSpace;
        // Console.WriteLine($"used space: {usedSpace}");
        // Console.WriteLine($"unused space: {unusedSpace}");
        // Console.WriteLine($"minimumDeletionSize: {minimumDeletionSize}");

        var result2 = _fileSystem.Select(path => CalculateSize(path.Value)).Where(size => size > minimumDeletionSize)
            .OrderBy(size => size).First();
        Console.WriteLine($"Day 7 part 2: {result2}");
    }
}