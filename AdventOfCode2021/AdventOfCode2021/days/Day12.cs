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
    private List<List<string>> _allPaths = new();
    
    public void Part1()
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
        
        // find distinct paths
        foreach (var cave in _caveSystem["start"].Connections)
        {
            var path = new List<string>(){"start"};
            NextStep(path, _caveSystem[cave]);
        }
        
        Console.WriteLine($"Day 12 part 1: {_allPaths.Count}");
    }
    
    public void Part2()
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
        
        // find distinct paths
        foreach (var cave in _caveSystem["start"].Connections)
        {
            var path = new List<string>(){"start"};
            NextStep2(path, _caveSystem[cave], true);
        }
        
        Console.WriteLine($"Day 12 part 2: {_allPaths.Count}");
    }

    private void NextStep(List<string> currentPath, Cave cave)
    {
        var path = currentPath.ToList();
        path.Add(cave.Name);
        
        if (cave.Name.Equals("end"))
        {
            _allPaths.Add(path);
            return;
        }
        
        foreach (var connection in cave.Connections)
        {
            var c = _caveSystem[connection];
            if (!c.Revisitable && path.Contains(connection))
                continue;
            
            NextStep(path, c);
        }
    }
    
    private void NextStep2(List<string> currentPath, Cave cave, bool canRevisitSmall)
    {
        var path = currentPath.ToList();
        path.Add(cave.Name);
        
        if (cave.Name.Equals("end"))
        {
            _allPaths.Add(path);
            return;
        }
        
        foreach (var connection in cave.Connections)
        {
            var c = _caveSystem[connection];
            if (!c.Revisitable && path.Contains(connection))
            {
                if(!canRevisitSmall || connection.Equals("start") || connection.Equals("end"))
                    continue;
                
                // make an exception for this small cave
                NextStep2(path, c, false);
                continue;
            }
            
            NextStep2(path, c, canRevisitSmall);
        }
    }
}