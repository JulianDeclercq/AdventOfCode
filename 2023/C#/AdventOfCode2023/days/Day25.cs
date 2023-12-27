namespace AdventOfCode2023.days;

public static class Day25
{
    private static readonly Dictionary<string, HashSet<string>> Connections = new();
    public static void Solve()
    {
        var input = File.ReadAllLines("../../../input/Day25e.txt");
        foreach (var line in input)
        {
            var component = line[..3];
            var connectedComponents = line[5..].Split(' ');

            AddConnections(component, connectedComponents);
            
            foreach (var connected in connectedComponents)
                AddConnections(connected, new[] {component});
        }

        var totalNodes = ConnectedCount("hfx", Connections); // start connected
        var brkpt = 5;
    }

    private static void AddConnections(string component, IEnumerable<string> connections)
    {
        var current = Connections.TryGetValue(component, out var existing) ? existing : new HashSet<string>();
        current.UnionWith(connections);
        Connections[component] = current;
    }

    private static void BreakConnection(string lhs, string rhs, Dictionary<string,HashSet<string>> connections)
    {
        if (!connections.TryGetValue(lhs, out var current))
            return;

        if (!current.Remove(rhs))
            throw new Exception("Failed to remove.");
        
        if (!connections[rhs].Remove(lhs))
            throw new Exception("Failed to remove.");
    }

    private static int ConnectedCount(string component, Dictionary<string,HashSet<string>> connections)
    {
        var connected = new HashSet<string>();
        Connect(component, connections, connected);
        return connected.Count;
    }
    
    private static void Connect(string component, Dictionary<string,HashSet<string>> connections, HashSet<string> connected)
    {
        foreach (var neighbour in connections[component])
        {
            if (connected.Add(neighbour))
                Connect(neighbour, connections, connected);            
        }
    }
}