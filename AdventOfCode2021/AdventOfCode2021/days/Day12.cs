using System.Runtime.InteropServices;

namespace AdventOfCode2021.days;

public class Day12
{
    public class Cave
    {
        public Cave(string name)
        {
            Name = name;
            Revisitable = name.Any(char.IsUpper);
        }
        
        public string Name;
        public bool Revisitable;
        public List<string> Connections = new();
    }

    private Dictionary<string, Cave> _caveSystem = new();
    
    public void Part1() => Solve(part1: true);
    public void Part2() => Solve(part1: false);

    private void Solve(bool part1)
    {
        var lines = File.ReadAllLines(@"..\..\..\input\day12.txt");

        // key = name
        _caveSystem = new Dictionary<string, Cave>();
        
        // create the caves based on the names
        foreach (var line in lines)
        {
            var split = line.Split('-');
            _caveSystem[split[0]] = new Cave(split[0]);
            _caveSystem[split[1]] = new Cave(split[1]);
        }
        
        // create the connections
        foreach (var connection in lines)
        {
            var split = connection.Split('-');
            
            var lhsCave = _caveSystem[split[0]];
            var rhsCave = _caveSystem[split[1]];
            
            lhsCave.Connections.Add(rhsCave.Name);
            rhsCave.Connections.Add(lhsCave.Name);
        }
        
        // find all paths
        var allPaths = new List<List<string>>();
        foreach (var cave in _caveSystem["start"].Connections)
        {
            var path = new List<string>(){"start"};

            if (part1) NextStep(allPaths, path, _caveSystem[cave]);
            else NextStep2(allPaths, path, _caveSystem[cave], true);
        }
        Console.WriteLine($"Day 12 part {(part1 ? '1' : '2')}: {allPaths.Count}");
    }

    private void NextStep(List<List<string>> allPaths, List<string> currentPath, Cave cave)
    {
        var path = currentPath.ToList();
        path.Add(cave.Name);
        
        if (cave.Name.Equals("end"))
        {
            allPaths.Add(path);
            return;
        }
        
        foreach (var connection in cave.Connections)
        {
            var c = _caveSystem[connection];
            if (!c.Revisitable && path.Contains(connection))
                continue;
            
            NextStep(allPaths, path, c);
        }
    }
    
    private void NextStep2(List<List<string>> allPaths, List<string> currentPath, Cave cave, bool canRevisitSmall)
    {
        var path = currentPath.ToList();
        path.Add(cave.Name);
        
        if (cave.Name.Equals("end"))
        {
            allPaths.Add(path);
            return;
        }
        
        foreach (var connection in cave.Connections)
        {
            var c = _caveSystem[connection];
            if (!c.Revisitable && path.Contains(connection))
            {
                // make an exception for this small cave if applicable
                if (canRevisitSmall && !connection.Equals("start") && !connection.Equals("end"))
                    NextStep2(allPaths, path, c, false);

                continue;
            }
            
            NextStep2(allPaths, path, c, canRevisitSmall);
        }
    }
}