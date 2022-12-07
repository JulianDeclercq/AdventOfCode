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
        public string Name = "";
        public ItemType Type = ItemType.None;
        public int Size = 0;
    }

    private const string RootDirectory = "";
    private readonly Dictionary<string, List<Item>> _fileSystem = new();
    private readonly Dictionary<string, int> _directorySizes = new();
    
    public void Solve(bool part1 = true)
    {
        var lines = File.ReadAllLines(@"..\..\..\input\day7.txt");
        var currentPath = RootDirectory;

        foreach (var line in lines)
        {
            // command mode
            if (line.First() == '$')
            {
                currentPath = HandleCommand(line, currentPath);
            }
            else // list mode
            {
                HandleListedItem(line, currentPath);
            }
        }
        
        Console.WriteLine($"Day 7 part {(part1 ? 1 : 2)}: {(part1 ? Part1() : Part2())}");   
    }

    private int Part1()
    {
        return _fileSystem.Select(path => CalculateSize(path.Value)).Where(size => size <= 100_000).Sum();
    }

    private int Part2()
    {
        const int totalSpace = 70_000_000, requiredSpace = 30_000_000;
        var usedSpace = CalculateSize("");
        var unusedSpace = totalSpace - usedSpace;
        var minimumDeletionSize = requiredSpace - unusedSpace;
         
        return _fileSystem.Select(path => CalculateSize(path.Value)).Where(size => size > minimumDeletionSize)
            .OrderBy(size => size).First();
    }
     
    private string HandleCommand(string command, string currentPath)
    {
        _commandRegex.Parse(command);
        var cmd = _commandRegex.Get("command");
        if (cmd != "cd") // ls doesn't need to be handled specifically
            return currentPath;
     
        var destination = _commandRegex.Get("destination");
        return destination switch
        {
            "/" => RootDirectory,
            ".." => currentPath[..currentPath.LastIndexOf('/')],
            _ => $"{currentPath}/{destination}"
        };
    }
     
    private void HandleListedItem(string itemDescription, string currentPath)
    {
        Item item;
        if (_directoryRegex.Parse(itemDescription))
        {
            item = new Item
            {
                Name = $"{currentPath}/{_directoryRegex.Get("name")}",
                Type = ItemType.Directory
            };
        }
        else if (_fileRegex.Parse(itemDescription))
        {
            item = new Item
            {
                Name = _fileRegex.Get("name"),
                Size = _fileRegex.GetInt("size"),
                Type = ItemType.Item
            };
        }
        else throw new Exception($"Couldn't parse ${itemDescription}");
     
        // add item
        if (!_fileSystem.TryGetValue(currentPath, out var currentDir))
        {
            _fileSystem.Add(currentPath, new List<Item> {item});
        }
        else currentDir.Add(item);
    }

    private int CalculateSize(IEnumerable<Item> items)
    {
        return items.Sum(item => item.Type switch
        {
            ItemType.Item => item.Size,
            ItemType.Directory => CalculateSize(item.Name),
        });
    }
    
    private int CalculateSize(string directoryName)
    {
        if (_directorySizes.TryGetValue(directoryName, out var foundSize))
            return foundSize;

        var size = CalculateSize(_fileSystem[directoryName]);
        _directorySizes.Add(directoryName, size);
        
        return size;
    }
}