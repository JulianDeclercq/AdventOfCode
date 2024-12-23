namespace AdventOfCode2024.Day23;

public class Day23
{
    public static void Solve(int part)
    {
        if (part != 1 && part != 2)
            throw new Exception($"Invalid part {part}");
        
        var lines = File.ReadAllLines("input/day23.txt");
        Dictionary<string, HashSet<string>> connections = [];
        foreach (var line in lines)
        {
            var split = line.Split('-');
            AddConnection(split[0], split[1], connections);
            AddConnection(split[1], split[0], connections);
        }

        if (part is 1) Part1(connections);
        else Part2(connections);
    }
    
    private static void Part1(Dictionary<string, HashSet<string>> connections)
    {
        // find sets of three connected computers where each computer in the set is connected to the other two computers
        HashSet<TriLink> triLinks = [];
        foreach (var computer in connections.Keys)
        {
            foreach (var connections1 in connections[computer])
            {
                foreach (var connections2 in connections[connections1])
                {
                    var connections3 = connections[connections2];
                    if (connections3.Contains(computer))
                    {
                        triLinks.Add(new TriLink
                        {
                            First = computer,
                            Second = connections1,
                            Third = connections2
                        });
                    }
                }
            }
        }
        
        var answer = triLinks.Count(triLink => triLink.HasComputerThatStartsWithT());
        Console.WriteLine(answer);
    }
    
    // contains partial networks (while they're still in the making) but that shouldn't matter for the solution
    private static readonly List<Network> ValidNetworks = [];

    private static void Part2(Dictionary<string, HashSet<string>> lookup)
    {
        foreach (var computer in lookup.Keys)
        {
            var network = new Network();
            network.Add(computer);
            FindConnectedComputers(network, lookup);
        }

        var largestNetwork = ValidNetworks.MaxBy(network => network.Nodes.Count)!;
        Console.WriteLine(largestNetwork.GetPassword());
    }
    
    private static void FindConnectedComputers(Network network, Dictionary<string, HashSet<string>> lookup)
    {
        var neighbours = lookup[network.LastAddedNode];
        foreach (var neighbour in neighbours)
        {
            // for each computer at the LAN party,
            // that computer will have a connection to every other computer at the LAN party
            if (network.Nodes.All(n => lookup[n].Contains(neighbour)))
            {
                network.Nodes.Add(neighbour);
                ValidNetworks.Add(network.DeepCopy());
                
                // search for further connections
                FindConnectedComputers(network, lookup);
            }
        }
    }

    private static void AddConnection(string first, string second, Dictionary<string, HashSet<string>> connections)
    {
        var hashset = connections.TryGetValue(first, out var current) ? current : [];
        hashset.Add(second);
        connections[first] = hashset;
    }
}